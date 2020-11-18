using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mms.Api.Models
{
	public class Attempt
	{
		public int Id { get; set; }
		public DateTime Time { get; set; }
		public string Reader { get; set; }
		public string Member { get; set; }
		public string Action { get; set; }
	}
}
