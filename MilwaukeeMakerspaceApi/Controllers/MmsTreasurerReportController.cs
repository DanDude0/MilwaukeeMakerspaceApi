using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mms.Api;
using Mms.Api.Models;
using Mms.Database;
using Newtonsoft.Json;

namespace Mms.Api.Controllers
{
	[ApiExplorerSettings(IgnoreApi = true)]

	public class MmsTreasurerReportController : Controller
	{
		[HttpGet]
		[Route("mmstreasurerreport/{month}")]
		public IActionResult MmsTreasurerReport(string month)
		{
			var date = DateTime.ParseExact(month, "yyyy-MM", null);

			var model = new TreasurerReport();

			model.Date = date;

			using var fundingDb = new AreaFundingDatabase();
			using var accessDb = new AccessControlDatabase();

			var sql = "SELECT `month` AS 'Date', `general` AS 'General', family AS 'Family', members AS 'Total' FROM `history` WHERE `month` > @0 AND `month` < @1 ORDER BY month ASC";

			var membershipHistory = fundingDb.Fetch<TreasurerReport.MembershipCounts>(sql, date.AddMonths(-6), date.AddMonths(1));

			for (var i = 1; i < membershipHistory.Count; i++) {
				membershipHistory[i].Net = membershipHistory[i].Total - membershipHistory[i - 1].Total;

				sql = "SELECT MIN(members) AS 'Min', MAX(members) AS 'Max' FROM funds WHERE YEAR(MONTH) = @0 AND MONTH(MONTH) = @1 AND members > 0";

				var membershipRangeInMonth = fundingDb.Single<TreasurerReport.MembershipRange>(sql, membershipHistory[i].Date.Year, membershipHistory[i].Date.Month);

				membershipHistory[i].Suspended = membershipRangeInMonth.Max - membershipRangeInMonth.Min;

				sql = "SELECT COUNT(*) FROM member WHERE joined BETWEEN @0 AND @1";

				var newMembersInMonth = accessDb.ExecuteScalar<int>(sql, membershipHistory[i].Date, membershipHistory[i].Date.AddMonths(1));

				membershipHistory[i].New = newMembersInMonth;

				membershipHistory[i].RetentionPecentage = ((double)membershipHistory[i].Total - membershipHistory[i].New) / membershipHistory[i - 1].Total * 100;
			}

			membershipHistory.RemoveAt(0);

			model.MembershipHistory = membershipHistory;

			var lastMonth = fundingDb.Single<FlowResult>("SELECT SUM(income) AS 'Income', SUM(spending+fees) AS 'Spending', SUM(ending_balance) AS 'Balance' FROM bank_statement WHERE `time` = @0", date.AddMonths(-1));

			model.CurrentFunds = lastMonth.Balance;
			model.ReserveFunds = 64000;
			model.CommittedAreaFunding = fundingDb.ExecuteScalar<int>("SELECT total FROM total_committed");
			model.GeneralFunds = model.CurrentFunds - model.ReserveFunds - model.CommittedAreaFunding;

			model.LastMonthIncome = lastMonth.Income;
			model.LastMonthSpending = lastMonth.Spending;
			model.LastMonthNetTotal = lastMonth.Income - lastMonth.Spending;

			var last12 = fundingDb.Single<FlowResult>("SELECT SUM(income) AS 'Income', SUM(spending+fees) AS 'Spending' FROM bank_statement WHERE `time` >= @0 AND `time` < @1", date.AddMonths(-12), date);

			model.LastYearIncome = last12.Income;
			model.LastYearSpending = last12.Spending;
			model.LastYearNetTotal = last12.Income - last12.Spending;
			model.LastYearNetTotalLessBuilding = model.LastYearNetTotal + 123764.47m;

			model.LastYearNetAreaFunds = fundingDb.ExecuteScalar<decimal>(@"SELECT
  SUM(`ledger`.`threed_printer`) +
  SUM(`ledger`.`bicycle_repair`) +
  SUM(`ledger`.`casting`) +
  SUM(`ledger`.`ceramic`) +
  SUM(`ledger`.`cnc`) +
  SUM(`ledger`.`cosplay`) +
  SUM(`ledger`.`craft`) +
  SUM(`ledger`.`dalek`) +
  SUM(`ledger`.`electronic`) +
  SUM(`ledger`.`finishing`) +
  SUM(`ledger`.`forge`) +
  SUM(`ledger`.`ham_radio`) +
  SUM(`ledger`.`jewelry`) +
  SUM(`ledger`.`lampworking`) +
  SUM(`ledger`.`laser`) +
  SUM(`ledger`.`leather`) +
  SUM(`ledger`.`long_arm`) +
  SUM(`ledger`.`makerfaire`) +
  SUM(`ledger`.`metal`) +
  SUM(`ledger`.`neon`) +
  SUM(`ledger`.`paint`) +
  SUM(`ledger`.`power_wheel`) +
  SUM(`ledger`.`print`) +
  SUM(`ledger`.`soda`) +
  SUM(`ledger`.`stained_glass`) +
  SUM(`ledger`.`tiger_lily`) +
  SUM(`ledger`.`vacuum`) +
  SUM(`ledger`.`welding`) +
  SUM(`ledger`.`wood`)
FROM `ledger`
WHERE `time` >= @0 AND `time` < @1", date.AddMonths(-12), date);

			model.LastYearNetGeneralFunds = model.LastYearNetTotal - model.LastYearNetAreaFunds;
			model.LastYearNetGeneralFundsLessBuilding = model.LastYearNetGeneralFunds + 123764.47m;

			return View(model);
		}
		public class FlowResult
		{
			public decimal Income { get; set; }
			public decimal Spending { get; set; }
			public decimal Balance { get; set; }
		}
	}
}
