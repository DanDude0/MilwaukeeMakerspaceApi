using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mms.Api
{
    public class AuthenticationResult
    {
		public string Reader { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public bool Admin { get; set; }
		public DateTime Joined { get; set; }
		public DateTime Expiration { get; set; }
		public bool AccessGranted { get; set; }
    }
}
