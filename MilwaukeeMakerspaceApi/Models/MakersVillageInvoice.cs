using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mms.Api.Models
{
	public class MakersVillageInvoice
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public decimal TotalMmsBilled { get; set; }
		public decimal TotalMmsPrePaid { get; set; }
		public decimal TotalMmsPaid { get; set; }
		public decimal TotalMmsOutstanding { get; set; }
		public decimal TotalMvOwed { get; set; }
		public decimal TotalMvPrePaid { get; set; }
		public decimal TotalMvAdjustments { get; set; }
		public decimal TotalMvPaid { get; set; }
		public decimal TotalMvOutstanding { get; set; }
		public DateTime CreatedDate { get; set; }
		public List<MemberInvoice> Invoices { get; set; }
		public List<Adjustment> Adjustments { get; set; }

		public class MemberInvoice
		{
			public int Id { get; set; }
			public DateTime Created { get; set; }
			public string Name { get; set; }
			public string PublicNotes { get; set; }
			public string PrivateNotes { get; set; }
			public decimal MmsBilled { get; set; }
			public decimal MmsPrePaid { get; set; }
			public decimal MmsPaid { get; set; }
			public decimal MmsOutstanding { get; set; }
			public decimal MvOwed { get; set; }
			public decimal MvPrePaid { get; set; }
			public decimal MvPaid { get; set; }
			public decimal MvOutstanding { get; set; }
			public List<InvoiceLine> Lines { get; set; }
			public string StorageNotes { get; set; }
		}

		public class InvoiceLine
		{
			public string Notes { get; set; }
			public decimal Amount { get; set; }
		}

		public class Adjustment
		{
			public int Id { get; set; }
			public DateTime Date { get; set; }
			public string Reason { get; set; }
			public decimal Amount { get; set; }
		}
	}
}
