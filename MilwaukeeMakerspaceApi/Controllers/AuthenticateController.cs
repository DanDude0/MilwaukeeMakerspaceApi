using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mms.Database;

namespace Mms.Api
{
	public class AuthenticateController : Controller
	{
		public IActionResult Csv(string id, string key)
		{
			try
			{
				var result = Authenticate(id, key);
				var output = $"{result.Reader}|{result.Id}|{result.Name}|{result.Type}|{result.Admin}|{result.Joined.ToString("yyyy-MM-dd")}|{result.Expiration.ToString("yyyy-MM-dd")}|{result.AccessGranted}";

				return Content(output);
			}
			catch
			{
				return StatusCode(500);
			}
		}

		public IActionResult Json(string id, string key)
		{
			try
			{
				var result = Authenticate(id, key);

				return new JsonResult(result);
			}
			catch
			{
				return StatusCode(500);
			}
		}

		private AuthenticationResult Authenticate(string id, string key)
		{
			AuthenticationResult result = null;

			using (var db = new AccessControlDatabase())
			{
				var sql = @"
						SELECT 
							r.name AS Reader, 
							m.member_id AS 'Id',
							m.name AS 'Name',
							m.type AS 'Type',
							m.apricot_admin AS Admin,
							m.joined AS Joined,
							m.expires AS Expiration,
							DATE_ADD(m.expires, INTERVAL 7 DAY) > NOW() AS AccessGranted
						FROM
							reader r
							INNER JOIN
							member m
							INNER JOIN keycode k
								ON m.member_id = k.member_id 
						WHERE
							r.reader_id = @0
							AND k.keycode_id = @1
						LIMIT 1;";

				result = db.Single<AuthenticationResult>(sql, id, key);
			}

			RecordAttempt(id, result.Id, result.AccessGranted);

			return result;
		}

		private void RecordAttempt(string readerId, int memberId, bool granted)
		{
			using (var db = new AccessControlDatabase())
			{
				db.Execute(@"
					INSERT INTO
						attempt (
							reader_id,
							member_id,
							access_granted,
							attempt_time
						)
					VALUES	(
						@0,
						@1,
						@2,
						NOW()
					);",
					readerId,
					memberId,
					granted);
			}
		}
	}
}
