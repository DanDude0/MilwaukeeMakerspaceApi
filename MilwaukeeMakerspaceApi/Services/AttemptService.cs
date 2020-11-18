using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mms.Api.Models;
using Mms.Database;

namespace Mms.Api.Services
{
	public class AttemptService
	{
		const int pageSize = 100;

		private class DbListResult
		{
			public int attempt_id { get; set; }
			public DateTime attempt_time { get; set; }
			public string reader { get; set; }
			public string member { get; set; }
			public bool access_granted { get; set; }
			public bool login { get; set; }
			public bool logout { get; set; }
			public string action { get; set; }
		};

		public async Task<Attempt[]> List(int page)
		{
			await Task.Yield();

			Attempt[] list = null;
			List<DbListResult> results = null;

			using (var db = new AccessControlDatabase()) {
				var sql = @"
					SELECT 
						a.attempt_id,
						a.attempt_time,
						r.name AS reader,
						m.name AS member,
						a.access_granted,
						a.login,
						a.logout,
						a.action
					FROM
						attempt a
						INNER JOIN reader r
							ON a.reader_id = r.reader_id
						INNER JOIN member m
							ON a.member_id = m.member_id
					ORDER BY
						a.attempt_time DESC
					LIMIT
						@0, @1";

				results = await db.FetchAsync<DbListResult>(sql, page * pageSize, pageSize);
			}

			list = new Attempt[results.Count];

			for (int i = 0; i < list.Length; i++) {
				var item = new Attempt {
					Id = results[i].attempt_id,
					Time = results[i].attempt_time,
					Reader = results[i].reader,
					Member = results[i].member,
				};

				if (results[i].access_granted)
					item.Action = "";
				else
					item.Action = "Denied: ";

				if (!string.IsNullOrEmpty(results[i].action))
					item.Action += results[i].action;
				else if (results[i].login)
					item.Action += "Login";
				else if (results[i].logout)
					item.Action += "Logout";
				else
					item.Action += "N/A (Legacy Reader)";

				list[i] = item;
			}

			return list;
		}

		public Task<attempt> Get(int id)
		{
			using (var db = new AccessControlDatabase()) {
				return db.SingleAsync<attempt>(id);
			}
		}
	}
}
