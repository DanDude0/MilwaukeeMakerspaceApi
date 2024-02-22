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

	public class InvoiceController : Controller
	{
		[HttpGet]
		[Route("mvinvoice/{id}")]
		public IActionResult MvInvoice(int id)
		{
			var invoice = GetMakersVillageInvoice(id);

			return View(invoice);
		}

		private MakersVillageInvoice GetMakersVillageInvoice(int id)
		{
			using var db = new BillingDatabase();

			var sql = @"
					SELECT 
						details
					FROM 
						makers_village_invoice
					WHERE
						makers_village_invoice_id = @0";

			var serializedInvoice = db.ExecuteScalar<string>(sql, id);

			var invoice = JsonConvert.DeserializeObject<MakersVillageInvoice>(serializedInvoice);

			return invoice;
		}
	}
}
