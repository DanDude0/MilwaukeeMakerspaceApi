using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mms.Api.Models;
using Mms.Database;

namespace Mms.Api.Controllers
{
	public class ReaderController : Controller
	{
		private class DbResult
		{
			public string name { get; set; }
			public int timeout { get; set; }
			public bool enabled { get; set; }
			public string groupName { get; set; }
			public string address { get; set; }
			public string settings { get; set; }
		};

		public IActionResult Lookup(string id)
		{
			try {
				DbResult result;

				using (var db = new AccessControlDatabase()) {
					var sql = @"
						SELECT 
							r.name,
							r.timeout,
							r.enabled,
							g.name AS groupName,
							r.address,
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

				if (result == null)
					return StatusCode(403);

				var clientAddress = HttpContext.Connection.RemoteIpAddress.ToString();

				if (result.address != clientAddress)
					RecordClientAddress(id, clientAddress);

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

		private void RecordClientAddress(string id, string address)
		{
			using (var db = new AccessControlDatabase()) {
				db.Execute(@"
					UPDATE
						reader
					SET
						address = @0
					WHERE
						reader_id = @1
					LIMIT 1;",
					address,
					id);
			}
		}
	}
}
