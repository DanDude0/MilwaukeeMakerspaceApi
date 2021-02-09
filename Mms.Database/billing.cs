// <auto-generated />
// This file was automatically generated by the PetePocoGenerator
// 
// The following connection settings were used to generate this file
// 
//     Connection String: `Server=mmsaccess.vfdworld.com;Port=3306;User=accessuser;password=**zapped**;convert zero datetime=True`
//     Provider:               `MySql`

// <auto-generated />
namespace Mms.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PetaPoco;

    [TableName("`billing`.`event`")]
    [PrimaryKey("event_id")]
    [ExplicitColumns]
    public partial class @event
    {
        [Column]
        public uint event_id { get; set; }

        [Column]
        public string parameters { get; set; }

        [Column]
        public string type { get; set; }

        [Column]
        public int wa_key { get; set; }

    }

    [TableName("`billing`.`invoice`")]
    [PrimaryKey("invoice_id", AutoIncrement=false)]
    [ExplicitColumns]
    public partial class invoice
    {
        [Column]
        public decimal amount { get; set; }

        [Column]
        public long contact_id { get; set; }

        [Column]
        public string contact_name { get; set; }

        [Column]
        public DateTime created_date { get; set; }

        [Column]
        public long creator_id { get; set; }

        [Column]
        public string document_number { get; set; }

        [Column]
        public DateTime invoice_date { get; set; }

        [Column]
        public long invoice_id { get; set; }

        [Column]
        public sbyte is_paid { get; set; }

        [Column]
        public decimal paid_amount { get; set; }

        [Column]
        public string private_notes { get; set; }

        [Column]
        public string public_notes { get; set; }

        [Column]
        public string type { get; set; }

        [Column]
        public DateTime updated_date { get; set; }

        [Column]
        public long updater_id { get; set; }

        [Column]
        public DateTime voided_date { get; set; }

    }

    [TableName("`billing`.`invoice_line`")]
    [PrimaryKey("invoice_line_id", AutoIncrement=false)]
    [ExplicitColumns]
    public partial class invoice_line
    {
        [Column]
        public decimal amount { get; set; }

        [Column]
        public long invoice_id { get; set; }

        [Column]
        public long invoice_line_id { get; set; }

        [Column]
        public string notes { get; set; }

        [Column]
        public string type { get; set; }

    }

    [TableName("`billing`.`makers_village_invoice`")]
    [PrimaryKey("makers_village_invoice")]
    [ExplicitColumns]
    public partial class makers_village_invoice
    {
        [Column]
        public DateTime created_date { get; set; }

        [Column]
        public string details { get; set; }

        [Column(Name = "makers_village_invoice")]
        public int _makers_village_invoice { get; set; }

        [Column]
        public int month { get; set; }

        [Column]
        public string title { get; set; }

        [Column]
        public decimal total_billed { get; set; }

        [Column]
        public decimal total_owed { get; set; }

        [Column]
        public decimal total_paid { get; set; }

        [Column]
        public decimal total_remainder { get; set; }

        [Column]
        public int year { get; set; }

    }

    [TableName("`billing`.`storage_notes`")]
    [PrimaryKey("storage_notes_id")]
    [ExplicitColumns]
    public partial class storage_note
    {
        [Column]
        public long contact_id { get; set; }

        [Column]
        public long invoice_id { get; set; }

        [Column]
        public string notes { get; set; }

        [Column]
        public DateTime snapshot_date { get; set; }

        [Column]
        public long storage_notes_id { get; set; }

    }

}

