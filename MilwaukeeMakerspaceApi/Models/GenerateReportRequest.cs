using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mms.Api.Models
{
	public class GenerateReportRequest
	{
		[Required]
		public string YearMonth { get; set; }
	}
}
