// <auto-generated />
// This file was automatically generated by the PetePocoGenerator
// 
// The following connection settings were used to generate this file
// 
//     Connection String: `Server=192.168.86.32;Port=10586;User=accessuser;password=**zapped**;SslMode=none`
//     Provider:               `MySql`

// <auto-generated />
namespace Mms.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PetaPoco;

    [TableName("`access_control`.`attempt`")]
    [PrimaryKey("attempt_id")]
    [ExplicitColumns]
    public partial class attempt
    {
        [Column]
        public sbyte access_granted { get; set; }

        [Column]
        public string action { get; set; }

        [Column]
        public int attempt_id { get; set; }

        [Column]
        public DateTime attempt_time { get; set; }

        [Column]
        public string keycode { get; set; }

        [Column]
        public sbyte login { get; set; }

        [Column]
        public sbyte logout { get; set; }

        [Column]
        public int member_id { get; set; }

        [Column]
        public int reader_id { get; set; }

    }

    [TableName("`access_control`.`group`")]
    [PrimaryKey("group_id")]
    [ExplicitColumns]
    public partial class group
    {
        [Column]
        public int group_id { get; set; }

        [Column]
        public string name { get; set; }

    }

    [TableName("`access_control`.`group_member`")]
    [PrimaryKey("member_id", AutoIncrement=false)]
    [ExplicitColumns]
    public partial class group_member
    {
        [Column]
        public int group_id { get; set; }

        [Column]
        public int member_id { get; set; }

    }

    [TableName("`access_control`.`keycode`")]
    [ExplicitColumns]
    public partial class keycode
    {
        [Column]
        public string keycode_id { get; set; }

        [Column]
        public int member_id { get; set; }

        [Column]
        public DateTime updated { get; set; }

    }

    [TableName("`access_control`.`member`")]
    [PrimaryKey("member_id", AutoIncrement=false)]
    [ExplicitColumns]
    public partial class member
    {
        [Column]
        public sbyte apricot_admin { get; set; }

        [Column]
        public DateTime expires { get; set; }

        [Column]
        public DateTime joined { get; set; }

        [Column]
        public int member_id { get; set; }

        [Column]
        public string name { get; set; }

        [Column]
        public string type { get; set; }

        [Column]
        public DateTime updated { get; set; }

    }

    [TableName("`access_control`.`network_event`")]
    [PrimaryKey("network_event_id")]
    [ExplicitColumns]
    public partial class network_event
    {
        [Column]
        public DateTime event_time { get; set; }

        [Column]
        public int network_event_id { get; set; }

        [Column]
        public sbyte online { get; set; }

        [Column]
        public int reader_id { get; set; }

    }

    [TableName("`access_control`.`reader`")]
    [PrimaryKey("reader_id", AutoIncrement=false)]
    [ExplicitColumns]
    public partial class reader
    {
        [Column]
        public string address { get; set; }

        [Column]
        public sbyte enabled { get; set; }

        [Column]
        public int group_id { get; set; }

        [Column]
        public string name { get; set; }

        [Column]
        public int reader_id { get; set; }

        [Column]
        public string settings { get; set; }

        [Column]
        public string status { get; set; }

        [Column]
        public int timeout { get; set; }

        [Column]
        public string version { get; set; }

    }

}

