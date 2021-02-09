using System;
using PetaPoco;

namespace Mms.Database
{
	public partial class BillingDatabase : global::PetaPoco.Database
	{
		public static new string ConnectionString = "";

		public BillingDatabase()
			: base(ConnectionString,
				new MySql.Data.MySqlClient.MySqlClientFactory())
		{
		}
	}
}
