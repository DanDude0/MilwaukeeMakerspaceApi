using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Mms.Api.Controllers
{
	[ApiExplorerSettings(IgnoreApi = true)]

	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Service()
		{
			return Content(Program.SsdpDescription, "application/xml");
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}
