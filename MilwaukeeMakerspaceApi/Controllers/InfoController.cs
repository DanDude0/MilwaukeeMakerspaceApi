using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mms.Database;

namespace Mms.Api.Controllers
{
	public class InfoController : Controller
	{
		public IActionResult Index()
		{
			fund data = null;

			using (var db = new AreaFundingDatabase()) {
				var sql = @"
					SELECT 
						* 
					FROM 
						funds 
					ORDER BY 
						month DESC 
					LIMIT 1;";

				data = db.Single<fund>(sql);
			}

			return View(data);
		}

		public IActionResult Balances()
		{
			balance data = null;

			using (var db = new AreaFundingDatabase()) {
				var sql = @"
				SELECT
					*
				FROM
					balances
				LIMIT 1;";

				data = db.Single<balance>(sql);
			}

			return View(data);
		}
	}
}
