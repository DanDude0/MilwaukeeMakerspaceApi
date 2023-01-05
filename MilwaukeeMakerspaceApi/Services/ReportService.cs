using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mms.Api.Models;
using Mms.Database;

namespace Mms.Api.Services
{
	public class ReportService
	{
		public OperationStatus loadStatus { get; private set; } = new OperationStatus();

		private class DbInvoiceResult
		{
			public int invoice_id { get; set; }
			public int document_number { get; set; }
			public DateTime created_date { get; set; }
			public string contact_name { get; set; }
			public string public_notes { get; set; }
			public string private_notes { get; set; }
			public decimal billed_amount { get; set; }
			public decimal payment_amount { get; set; }
			public decimal paid_amount { get; set; }
			public string notes { get; set; }
		};

		public async Task<bank_statement[]> ListStatements()
		{
			using var db = new AreaFundingDatabase();

			var list = await db.FetchAsync<bank_statement>("ORDER BY time DESC, account_name ASC");

			return list.ToArray();
		}

		public void RecordStatement(UpdateStatementRequest request)
		{
			var date = DateTime.ParseExact(request.YearMonth, "yyyy-MM", null);

			if (request.Name != "PayPal" && request.Name != "Landmark Checking" && request.Name != "Landmark Savings")
				throw new Exception("Account Name is Invalid");

			var statement = new bank_statement {
				account_name = request.Name,
				time = date,
				starting_balance = request.StartingBalance,
				ending_balance = request.EndingBalance,
				income = request.Income,
				spending = request.Spending,
				transfers = request.Transfers,
				fees = request.Fees,
			};

			using var db = new AreaFundingDatabase();

			db.Insert(statement);
		}
	}
}
