using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mms.Api.Models
{
	public class ReaderResult
	{
		public string Name { get; set; }
		public int Timeout { get; set; }
		public bool Enabled { get; set; }
		public string Group { get; set; }
	}
}
