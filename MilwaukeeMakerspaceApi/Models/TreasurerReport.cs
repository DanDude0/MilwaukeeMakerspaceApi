using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mms.Api.Models
{
	public class TreasurerReport
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public List<MembershipCounts> MembershipHistory { get; set; }

		public decimal CurrentAssets { get; set; }
		public decimal OutstandingMortgagePrincipal { get; set; }
		public decimal CurrentFunds { get; set; }
		public decimal ReserveFunds { get; set; }
		public decimal CommittedAreaFunding { get; set; }
		public decimal GeneralFunds { get; set; }

		public decimal LastMonthIncome { get; set; }
		public decimal LastMonthSpending { get; set; }
		public decimal LastMonthNetTotal { get; set; }
		public decimal LastMonthNetGeneralFunds { get; set; }
		public decimal LastMonthNetAreaFunds { get; set; }


		public decimal LastYearIncome { get; set; }
		public decimal LastYearSpending { get; set; }
		public decimal LastYearNetTotal { get; set; }
		public decimal LastYearNetGeneralFunds { get; set; }
		public decimal LastYearNetAreaFunds { get; set; }


		public class MembershipCounts
		{
			public DateTime Date { get; set; }
			public int Total { get; set; }
			public int General { get; set; }
			public int Family { get; set; }
			public int New { get; set; }
			public int Suspended { get; set; }
			public int Net { get; set; }
			public double RetentionPecentage { get; set; }
		}
	}
}
