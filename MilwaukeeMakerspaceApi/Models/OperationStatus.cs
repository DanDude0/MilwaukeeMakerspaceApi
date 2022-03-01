using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mms.Api.Models
{
	public class OperationStatus
	{
		public int progress { get; set; }
		public int range { get; set; }
		public string status { get; set; }
	}
}
