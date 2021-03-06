using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mms.Api.Models;
using Mms.Database;
using SQLite;

namespace Mms.Api.Controllers
{
	public class ReaderController : ControllerBase
	{
		private class DbResult
		{
			public string name { get; set; }
			public int timeout { get; set; }
			public bool enabled { get; set; }
			public string groupName { get; set; }
			public string settings { get; set; }
		};

		[HttpPost]
		[Route("reader/initialize")]
		public IActionResult Initialize([FromBody] string payload)
		{
			try {
				var request = JsonDocument.Parse(payload).RootElement;

				var id = request.GetProperty("Id").GetInt32();
				var version = request.GetProperty("Version").GetString();

				DbResult result;

				using (var db = new AccessControlDatabase()) {
					var sql = @"
						SELECT 
							r.name,
							r.timeout,
							r.enabled,
							g.name AS groupName,
							r.settings
						FROM
							reader r
							INNER JOIN `group` g
								ON r.group_id = g.group_id
						WHERE
							r.reader_id = @0
						LIMIT 1;";

					result = db.SingleOrDefault<DbResult>(sql, id);
				}

				if (id < 1 || result == null)
					return StatusCode(401);

				var clientAddress = HttpContext.Connection.RemoteIpAddress.ToString();

				RecordClient(id, clientAddress, version, payload);

				var output = new ReaderResult {
					Name = result.name,
					Timeout = result.timeout,
					Enabled = result.enabled,
					Group = result.groupName,
					Settings = result.settings,
				};

				return new JsonResult(output);
			}
			catch (Exception ex) {
				Console.Write(ex.ToString());

				return StatusCode(500);
			}
		}

		[HttpPost]
		[Route("reader/action")]
		public IActionResult Action([FromBody] string payload)
		{
			try {
				var request = JsonDocument.Parse(payload).RootElement;

				var id = request.GetProperty("Id").GetInt32();
				var key = request.GetProperty("Key").GetString() ?? "";
				var type = request.GetProperty("Type").GetString() ?? "";
				var action = request.GetProperty("Action").GetString() ?? "";

				if (id < 1)
					return StatusCode(401);

				var credentials = Authenticate(key, id);

				switch (type) {
					case "Login":
						RecordAttempt(id, key, credentials, true, false, action);

						if (credentials == null)
							return StatusCode(403);

						return new JsonResult(credentials);
					case "Logout":
						RecordAttempt(id, key, credentials, false, true, action);

						return StatusCode(200);
					case "Action":
						RecordAttempt(id, key, credentials, false, false, action);

						return StatusCode(200);
				}

				return StatusCode(400);
			}
			catch (Exception ex) {
				Console.Write(ex.ToString());

				return StatusCode(500);
			}
		}

		[HttpGet]
		[Route("reader/snapshot")]
		public IActionResult Database()
		{
			var tmpFile = Path.GetTempPath() + "snapshot.sqlite3";

			System.IO.File.Delete(tmpFile);

			var snapshot = new SQLiteConnection(tmpFile, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite, false);

			snapshot.BeginTransaction();

			snapshot.CreateTable<attempt>();
			snapshot.CreateTable<group>();
			snapshot.CreateTable<group_member>();
			snapshot.CreateTable<keycode>();
			snapshot.CreateTable<member>();
			snapshot.CreateTable<reader>();
			
			using (var db = new AccessControlDatabase()) {
				snapshot.InsertAll(db.Query<group>());
				snapshot.InsertAll(db.Query<group_member>());
				snapshot.InsertAll(db.Query<member>());
				snapshot.InsertAll(db.Query<reader>());

				// We have to get a bit more creative with the keycodes, cleaning up the data
				foreach (var code in db.Query<keycode>()) {
					var key = code.keycode_id.Replace("#", "");

					snapshot.Insert(new keycode {
						keycode_id = key,
						member_id = code.member_id,
						updated = code.updated
					});

					// Create 2nd truncated copy for W26 crap
					if (key.Length == 10) {
						key = key.Substring(4, 6);

						snapshot.Insert(new keycode {
							keycode_id = key,
							member_id = code.member_id,
							updated = code.updated
						});
					}
				}
			}

			snapshot.Commit();
			snapshot.Close();

			var stream = new FileStream(tmpFile, FileMode.Open, FileAccess.ReadWrite);

			return File(stream, "application/vnd.sqlite3", "snapshot.sqlite3");
		}

		private AuthenticationResult Authenticate(string key, int readerId)
		{
			if (!string.IsNullOrEmpty(key)) {
				int? groupId = null;
				string sql = null;

				using (var db = new AccessControlDatabase()) {
					sql = @"
						SELECT 
							group_id
						FROM
							reader
						WHERE
							reader_id = @0
							AND enabled = 1
						LIMIT 1;";

					groupId = db.SingleOrDefault<int?>(sql, readerId);

					if (groupId == null)
						return null;

				//Support checking only the lower 26 bits of the key, because of stupid Wiegand protocol!
				if (key.StartsWith("W26#")) {
						if (groupId != 0)
							sql = @"
							SELECT 
								m.member_id AS 'Id',
								m.name AS 'Name',
								m.type AS 'Type',
								m.apricot_admin AS Admin,
								m.joined AS Joined,
								m.expires AS Expiration,
								DATE_ADD(m.expires, INTERVAL 7 DAY) > NOW() AS AccessGranted
							FROM
								member m
								INNER JOIN keycode k
									ON m.member_id = k.member_id 
								INNER JOIN group_member gm
									ON m.member_id = gm.member_id
							WHERE
								0x00FFFFFF & CONV(k.keycode_id, 16, 10) = CONV(@0, 16, 10)
								AND gm.group_id = @1
							LIMIT 1;";
						else
							sql = @"
							SELECT 
								m.member_id AS 'Id',
								m.name AS 'Name',
								m.type AS 'Type',
								m.apricot_admin AS Admin,
								m.joined AS Joined,
								m.expires AS Expiration,
								DATE_ADD(m.expires, INTERVAL 7 DAY) > NOW() AS AccessGranted
							FROM
								member m
								INNER JOIN keycode k
									ON m.member_id = k.member_id
							WHERE
								0x00FFFFFF & CONV(k.keycode_id, 16, 10) = CONV(@0, 16, 10)
							LIMIT 1;";

						return db.SingleOrDefault<AuthenticationResult>(sql, key.Substring(6), groupId);
					}
				else {
						if (groupId != 0)
							sql = @"
							SELECT 
								m.member_id AS 'Id',
								m.name AS 'Name',
								m.type AS 'Type',
								m.apricot_admin AS Admin,
								m.joined AS Joined,
								m.expires AS Expiration,
								DATE_ADD(m.expires, INTERVAL 7 DAY) > NOW() AS AccessGranted
							FROM
								member m
								INNER JOIN keycode k
									ON m.member_id = k.member_id 
								INNER JOIN group_member gm
									ON m.member_id = gm.member_id
							WHERE
								(k.keycode_id = @0 OR k.keycode_id = @1)
								AND gm.group_id = @2
							LIMIT 1;";
						else
							sql = @"
							SELECT 
								m.member_id AS 'Id',
								m.name AS 'Name',
								m.type AS 'Type',
								m.apricot_admin AS Admin,
								m.joined AS Joined,
								m.expires AS Expiration,
								DATE_ADD(m.expires, INTERVAL 7 DAY) > NOW() AS AccessGranted
							FROM
								member m
								INNER JOIN keycode k
									ON m.member_id = k.member_id 
							WHERE
								(k.keycode_id = @0 OR k.keycode_id = @1)
							LIMIT 1;";

						// Check for older style keys with the trailing # in the database
						return db.SingleOrDefault<AuthenticationResult>(sql, key, $"{key}#", groupId);
					}
				}
			}

			return null;
		}

		private void RecordAttempt(int readerId, string key, AuthenticationResult credentials, bool login, bool logout, string action)
		{
			using (var db = new AccessControlDatabase()) {
				db.Execute(@"
					INSERT INTO
						attempt (
							reader_id,
							keycode,
							member_id,
							access_granted,
							login,
							logout,
							action,
							attempt_time
						)
					VALUES	(
						@0,
						@1,
						@2,
						@3,
						@4,
						@5,
						@6,
						NOW()
					);",
					readerId,
					key,
					credentials?.Id ?? -1,
					credentials?.AccessGranted ?? false,
					login,
					logout,
					action);
			}
		}

		private void RecordClient(int readerId, string address, string version, string status)
		{
			using (var db = new AccessControlDatabase()) {
				db.Execute(@"
					UPDATE
						reader
					SET
						address = @0,
						version = @1,
						status = @2
					WHERE
						reader_id = @3
					LIMIT 1;",
					address,
					version,
					status,
					readerId);
			}
		}
	}
}
