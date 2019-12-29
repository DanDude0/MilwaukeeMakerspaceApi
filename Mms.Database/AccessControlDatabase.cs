using System;
using PetaPoco;

namespace Mms.Database
{
	public partial class AccessControlDatabase : global::PetaPoco.Database
	{
		public static new string ConnectionString = "";

		public AccessControlDatabase()
			: base(ConnectionString,
				new MySql.Data.MySqlClient.MySqlClientFactory())
		{
		}
	}
}
