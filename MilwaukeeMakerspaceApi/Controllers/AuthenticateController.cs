using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mms.Api.Models;
using Mms.Database;

namespace Mms.Api.Controllers
{
	public class AuthenticateController : Controller
	{
		public IActionResult Csv(string id, string key)
		{
			try {
				var result = Authenticate(id, key);
				var output = $"{result.Id}|{result.Name}|{result.Type}|{result.Admin}|{result.Joined.ToString("yyyy-MM-dd")}|{result.Expiration.ToString("yyyy-MM-dd")}|{result.AccessGranted}";

				return Content(output);
			}
			catch {
				return StatusCode(500);
			}
		}

		public IActionResult Json(string id, string key)
		{
			try {
				var result = Authenticate(id, key);

				return new JsonResult(result);
			}
			catch {
				return StatusCode(500);
			}
		}
		public IActionResult Logout(string id)
		{
			try {
				RecordAttempt(id, "-1", -1, true, false, true);

				return StatusCode(200);
			}
			catch {
				return StatusCode(500);
			}
		}

		private AuthenticationResult Authenticate(string id, string key)
		{
			AuthenticationResult result = null;

			//Support checking only the lower 26 bits of the key, because of stupid Wiegand protocol!
			if (key.StartsWith("W26#")) {
				using (var db = new AccessControlDatabase()) {
					var sql = @"
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
							0x00FFFFFE & CONV(k.keycode_id, 16, 10) = CONV(@0, 16, 10)
						LIMIT 1;";

					result = db.SingleOrDefault<AuthenticationResult>(sql, key.Substring(6));
				}
			}
			else {
				using (var db = new AccessControlDatabase()) {
					var sql = @"
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
							k.keycode_id = @1
						LIMIT 1;";

					result = db.SingleOrDefault<AuthenticationResult>(sql, id, key);
				}
			}
			RecordAttempt(id, key, result?.Id ?? -1, result?.AccessGranted ?? false, true, false);

			if (result == null)
				throw new Exception("Code not found");

			return result;
		}

		private void RecordAttempt(string readerId, string key, int memberId, bool granted, bool login, bool logout)
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
							attempt_time
						)
					VALUES	(
						@0,
						@1,
						@2,
						@3,
						@4,
						@5,
						NOW()
					);",
					readerId,
					key,
					memberId,
					granted,
					login,
					logout);
			}
		}
	}
}
