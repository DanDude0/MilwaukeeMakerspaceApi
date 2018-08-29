



















// This file was automatically generated by the PetaPoco T4 Template
// Do not make changes directly to this file - edit the template instead
// 
// The following connection settings were used to generate this file
// 
//     Connection String Name: `access_control`
//     Provider:               `MySql.Data.MySqlClient`
//     Connection String:      `Server=192.168.86.32;Port=9587;User=accessuser;password=**zapped**;SslMode=none`
//     Schema:                 ``
//     Include Views:          `True`



using System;
using System.Collections.Generic;
using NPoco;

namespace Mms.Database
{

	public partial class AccessControlDatabase : global::NPoco.Database
	{
		public AccessControlDatabase() 
			: base("Server=127.0.0.1;Port=3306;User=accessuser;Password=mkemaker!34;Database=access_control;SslMode=none", 
				DatabaseType.MySQL, 
				new MySql.Data.MySqlClient.MySqlClientFactory())
		{
			CommonConstruct();
		}

		public AccessControlDatabase(string connectionStringName) 
			: base(connectionStringName, 
				DatabaseType.MySQL, 
				new MySql.Data.MySqlClient.MySqlClientFactory())
		{
			CommonConstruct();
		}
		
		partial void CommonConstruct();
		
		public interface IFactory
		{
			AccessControlDatabase GetInstance();
		}
		
		public static IFactory Factory { get; set; }
        public static AccessControlDatabase GetInstance()
        {
			if (_instance!=null)
				return _instance;
				
			if (Factory!=null)
				return Factory.GetInstance();
			else
				return new AccessControlDatabase();
        }

		[ThreadStatic] static AccessControlDatabase _instance;
		
		protected override void OnBeginTransaction()
		{
			if (_instance==null)
				_instance=this;
		}
		
		protected override void OnAbortTransaction()
		{
			if (_instance==this)
				_instance=null;
		}
		
		protected override void OnCompleteTransaction()
		{
			if (_instance==this)
				_instance=null;
		}
		

	}
	



    

	[TableName("access_control.attempt")]



	[PrimaryKey("attempt_id")]




	[ExplicitColumns]

    public partial class attempt  
    {



		[Column] public int attempt_id { get; set; }





		[Column] public int member_id { get; set; }





		[Column] public int reader_id { get; set; }





		[Column] public sbyte access_granted { get; set; }





		[Column] public DateTime attempt_time { get; set; }



	}

    

	[TableName("access_control.keycode")]



	[PrimaryKey("keycode_id", AutoIncrement=false)]


	[ExplicitColumns]

    public partial class keycode  
    {



		[Column] public string keycode_id { get; set; }





		[Column] public int member_id { get; set; }





		[Column] public DateTime updated { get; set; }



	}

    

	[TableName("access_control.member")]



	[PrimaryKey("member_id", AutoIncrement=false)]


	[ExplicitColumns]

    public partial class member  
    {



		[Column] public int member_id { get; set; }





		[Column] public string name { get; set; }





		[Column] public string type { get; set; }





		[Column] public sbyte apricot_admin { get; set; }





		[Column] public DateTime joined { get; set; }





		[Column] public DateTime expires { get; set; }





		[Column] public DateTime updated { get; set; }



	}

    

	[TableName("access_control.network_event")]



	[PrimaryKey("network_event_id")]




	[ExplicitColumns]

    public partial class network_event  
    {



		[Column] public int network_event_id { get; set; }





		[Column] public int reader_id { get; set; }





		[Column] public sbyte online { get; set; }





		[Column] public DateTime event_time { get; set; }



	}

    

	[TableName("access_control.reader")]



	[PrimaryKey("reader_id", AutoIncrement=false)]


	[ExplicitColumns]

    public partial class reader  
    {



		[Column] public int reader_id { get; set; }





		[Column] public string name { get; set; }



	}


}
