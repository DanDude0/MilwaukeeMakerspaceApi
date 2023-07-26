using System;
using PetaPoco;

namespace Mms.Database
{
	public partial class AccessControlDatabase : global::PetaPoco.Database
	{
		public static new string ConnectionString = "";

		public AccessControlDatabase()
			: base(ConnectionString,
				  MySqlConnector.MySqlConnectorFactory.Instance)
		{
		}
	}
}
