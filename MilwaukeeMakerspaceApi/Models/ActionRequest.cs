using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mms.Api.Models
{
	public class ActionRequest
	{
		public int Id { get; set; }
		public string Key { get; set; }
		public string Type { get; set; }
		public string Action { get; set; }
	}
}
