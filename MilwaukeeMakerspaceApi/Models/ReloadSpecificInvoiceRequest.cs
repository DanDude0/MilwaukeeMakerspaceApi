using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mms.Api.Models
{
	public class ReloadSpecificInvoiceRequest
	{
		[Required]
		public string InvoiceNumber { get; set; }
	}
}
