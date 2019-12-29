using System;
using PetaPoco;

namespace Mms.Database
{
	public partial class AreaFundingDatabase : global::PetaPoco.Database
	{
		public static new string ConnectionString = "";

		public AreaFundingDatabase()
			: base(ConnectionString,
				new MySql.Data.MySqlClient.MySqlClientFactory())
		{
		}
	}
}
