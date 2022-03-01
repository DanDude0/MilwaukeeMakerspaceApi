using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mms.Api.Models
{
	public class LoadInvoiceRequest
	{
		[Required]
		public DateTimeOffset? Start { get; set; }
		[Required]
		public DateTimeOffset? End { get; set; }
	}
}
