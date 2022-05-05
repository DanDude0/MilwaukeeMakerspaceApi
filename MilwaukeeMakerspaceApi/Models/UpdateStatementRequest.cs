using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mms.Api.Models
{
	public class UpdateStatementRequest
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string YearMonth { get; set; }
		[Required]
		public decimal StartingBalance { get; set; }
		[Required]
		public decimal EndingBalance { get; set; }
		[Required]
		public decimal Income { get; set; }
		[Required]
		public decimal Spending { get; set; }
		[Required]
		public decimal Transfers { get; set; }
		[Required]
		public decimal Fees { get; set; }
	}
}
