using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mms.Api.Models
{
	public class TimeResult
	{
		public DateTime ServerUTC { get; set; } = DateTime.UtcNow;
	}
}
