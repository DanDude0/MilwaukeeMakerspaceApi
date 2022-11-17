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

    [TableName("`area_funding`.`balances`")]
    [ExplicitColumns]
    public partial class balance
    {
        [Column]
        public decimal? casting { get; set; }

        [Column]
        public decimal? ceramic { get; set; }

        [Column]
        public decimal? cnc { get; set; }

        [Column]
        public decimal? cosplay { get; set; }

        [Column]
        public decimal? craft { get; set; }

        [Column]
        public decimal? dalek { get; set; }

        [Column]
        public decimal? electronic { get; set; }

        [Column]
        public decimal? finishing { get; set; }

        [Column]
        public decimal? forge { get; set; }

        [Column]
        public decimal? glass_fusing { get; set; }

        [Column]
        public decimal? ham_radio { get; set; }

        [Column]
        public decimal? jewelry { get; set; }

        [Column]
        public decimal? lampworking { get; set; }

        [Column]
        public decimal? laser { get; set; }

        [Column]
        public decimal? leather { get; set; }

        [Column]
        public decimal? long_arm { get; set; }

        [Column]
        public decimal? makerfaire { get; set; }

        [Column]
        public decimal? metal { get; set; }

        [Column]
        public decimal? neon { get; set; }

        [Column]
        public decimal? paint { get; set; }

        [Column]
        public decimal? power_wheel { get; set; }

        [Column]
        public decimal? print { get; set; }

        [Column]
        public decimal? soda { get; set; }

        [Column]
        public decimal? stained_glass { get; set; }

        [Column]
        public decimal? threed_printer { get; set; }

        [Column]
        public decimal? tiger_lily { get; set; }

        [Column]
        public decimal? vacuum { get; set; }

        [Column]
        public decimal? welding { get; set; }

        [Column]
        public decimal? wood { get; set; }

    }

    [TableName("`area_funding`.`bank_statement`")]
    [PrimaryKey("bank_statement_id")]
    [ExplicitColumns]
    public partial class bank_statement
    {
        [Column]
        public string account_name { get; set; }

        [Column]
        public int bank_statement_id { get; set; }

        [Column]
        public decimal ending_balance { get; set; }

        [Column]
        public decimal fees { get; set; }

        [Column]
        public decimal income { get; set; }

        [Column]
        public decimal spending { get; set; }

        [Column]
        public decimal starting_balance { get; set; }

        [Column]
        public DateTime time { get; set; }

        [Column]
        public decimal transfers { get; set; }

    }

    [TableName("`area_funding`.`current`")]
    [ExplicitColumns]
    public partial class current
    {
        [Column]
        public decimal building_purchase { get; set; }

        [Column]
        public decimal casting { get; set; }

        [Column]
        public decimal ceramic { get; set; }

        [Column]
        public decimal cnc { get; set; }

        [Column]
        public decimal cosplay { get; set; }

        [Column]
        public decimal craft { get; set; }

        [Column]
        public decimal dalek { get; set; }

        [Column]
        public decimal electronic { get; set; }

        [Column]
        public int family { get; set; }

        [Column]
        public decimal finishing { get; set; }

        [Column]
        public decimal forge { get; set; }

        [Column]
        public int general { get; set; }

        [Column]
        public decimal glass_fusing { get; set; }

        [Column]
        public decimal ham_radio { get; set; }

        [Column]
        public decimal jewelry { get; set; }

        [Column]
        public decimal lampworking { get; set; }

        [Column]
        public decimal laser { get; set; }

        [Column]
        public decimal leather { get; set; }

        [Column]
        public decimal long_arm { get; set; }

        [Column]
        public decimal makerfaire { get; set; }

        [Column]
        public int members { get; set; }

        [Column]
        public decimal metal { get; set; }

        [Column]
        public DateTime month { get; set; }

        [Column]
        public decimal neon { get; set; }

        [Column]
        public decimal paint { get; set; }

        [Column]
        public decimal power_wheel { get; set; }

        [Column]
        public decimal print { get; set; }

        [Column]
        public decimal soda { get; set; }

        [Column]
        public decimal stained_glass { get; set; }

        [Column]
        public decimal threed_printer { get; set; }

        [Column]
        public decimal tiger_lily { get; set; }

        [Column]
        public decimal total { get; set; }

        [Column]
        public decimal vacuum { get; set; }

        [Column]
        public decimal welding { get; set; }

        [Column]
        public decimal wood { get; set; }

    }

    [TableName("`area_funding`.`funds`")]
    [PrimaryKey("funds_id")]
    [ExplicitColumns]
    public partial class fund
    {
        [Column]
        public decimal building_purchase { get; set; }

        [Column]
        public decimal casting { get; set; }

        [Column]
        public decimal ceramic { get; set; }

        [Column]
        public decimal cnc { get; set; }

        [Column]
        public decimal cosplay { get; set; }

        [Column]
        public decimal craft { get; set; }

        [Column]
        public decimal dalek { get; set; }

        [Column]
        public decimal electronic { get; set; }

        [Column]
        public int family { get; set; }

        [Column]
        public decimal finishing { get; set; }

        [Column]
        public decimal forge { get; set; }

        [Column]
        public uint funds_id { get; set; }

        [Column]
        public int general { get; set; }

        [Column]
        public decimal glass_fusing { get; set; }

        [Column]
        public decimal ham_radio { get; set; }

        [Column]
        public decimal jewelry { get; set; }

        [Column]
        public decimal lampworking { get; set; }

        [Column]
        public decimal laser { get; set; }

        [Column]
        public decimal leather { get; set; }

        [Column]
        public decimal long_arm { get; set; }

        [Column]
        public decimal makerfaire { get; set; }

        [Column]
        public int members { get; set; }

        [Column]
        public decimal metal { get; set; }

        [Column]
        public DateTime month { get; set; }

        [Column]
        public decimal neon { get; set; }

        [Column]
        public decimal paint { get; set; }

        [Column]
        public decimal power_wheel { get; set; }

        [Column]
        public decimal print { get; set; }

        [Column]
        public decimal soda { get; set; }

        [Column]
        public decimal stained_glass { get; set; }

        [Column]
        public decimal threed_printer { get; set; }

        [Column]
        public decimal tiger_lily { get; set; }

        [Column]
        public decimal total { get; set; }

        [Column]
        public decimal vacuum { get; set; }

        [Column]
        public decimal welding { get; set; }

        [Column]
        public decimal wood { get; set; }

    }

    [TableName("`area_funding`.`history`")]
    [ExplicitColumns]
    public partial class history
    {
        [Column]
        public decimal building_purchase { get; set; }

        [Column]
        public decimal casting { get; set; }

        [Column]
        public decimal ceramic { get; set; }

        [Column]
        public decimal cnc { get; set; }

        [Column]
        public decimal cosplay { get; set; }

        [Column]
        public decimal craft { get; set; }

        [Column]
        public decimal dalek { get; set; }

        [Column]
        public decimal electronic { get; set; }

        [Column]
        public int family { get; set; }

        [Column]
        public decimal finishing { get; set; }

        [Column]
        public decimal forge { get; set; }

        [Column]
        public int general { get; set; }

        [Column]
        public decimal glass_fusing { get; set; }

        [Column]
        public decimal ham_radio { get; set; }

        [Column]
        public decimal jewelry { get; set; }

        [Column]
        public decimal lampworking { get; set; }

        [Column]
        public decimal laser { get; set; }

        [Column]
        public decimal leather { get; set; }

        [Column]
        public decimal long_arm { get; set; }

        [Column]
        public decimal makerfaire { get; set; }

        [Column]
        public int members { get; set; }

        [Column]
        public decimal metal { get; set; }

        [Column]
        public DateTime month { get; set; }

        [Column]
        public decimal neon { get; set; }

        [Column]
        public decimal paint { get; set; }

        [Column]
        public decimal power_wheel { get; set; }

        [Column]
        public decimal print { get; set; }

        [Column]
        public decimal soda { get; set; }

        [Column]
        public decimal stained_glass { get; set; }

        [Column]
        public decimal threed_printer { get; set; }

        [Column]
        public decimal tiger_lily { get; set; }

        [Column]
        public decimal total { get; set; }

        [Column]
        public decimal vacuum { get; set; }

        [Column]
        public decimal welding { get; set; }

        [Column]
        public decimal wood { get; set; }

    }

    [TableName("`area_funding`.`ledger`")]
    [ExplicitColumns]
    public partial class ledger
    {
        [Column]
        public decimal casting { get; set; }

        [Column]
        public decimal ceramic { get; set; }

        [Column]
        public decimal cnc { get; set; }

        [Column]
        public decimal cosplay { get; set; }

        [Column]
        public decimal craft { get; set; }

        [Column]
        public decimal dalek { get; set; }

        [Column]
        public decimal electronic { get; set; }

        [Column]
        public decimal finishing { get; set; }

        [Column]
        public decimal forge { get; set; }

        [Column]
        public decimal glass_fusing { get; set; }

        [Column]
        public decimal ham_radio { get; set; }

        [Column]
        public decimal jewelry { get; set; }

        [Column]
        public decimal lampworking { get; set; }

        [Column]
        public decimal laser { get; set; }

        [Column]
        public decimal leather { get; set; }

        [Column]
        public decimal long_arm { get; set; }

        [Column]
        public decimal makerfaire { get; set; }

        [Column]
        public decimal metal { get; set; }

        [Column]
        public decimal neon { get; set; }

        [Column]
        public decimal paint { get; set; }

        [Column]
        public decimal power_wheel { get; set; }

        [Column]
        public decimal print { get; set; }

        [Column]
        public string reason { get; set; }

        [Column]
        public decimal soda { get; set; }

        [Column]
        public decimal stained_glass { get; set; }

        [Column]
        public decimal threed_printer { get; set; }

        [Column]
        public decimal tiger_lily { get; set; }

        [Column]
        public DateTime time { get; set; }

        [Column]
        public decimal vacuum { get; set; }

        [Column]
        public decimal welding { get; set; }

        [Column]
        public decimal wood { get; set; }

    }

    [TableName("`area_funding`.`spending`")]
    [PrimaryKey("spending_id")]
    [ExplicitColumns]
    public partial class spending
    {
        [Column]
        public decimal casting { get; set; }

        [Column]
        public decimal ceramic { get; set; }

        [Column]
        public decimal cnc { get; set; }

        [Column]
        public decimal cosplay { get; set; }

        [Column]
        public decimal craft { get; set; }

        [Column]
        public decimal dalek { get; set; }

        [Column]
        public decimal electronic { get; set; }

        [Column]
        public decimal finishing { get; set; }

        [Column]
        public decimal forge { get; set; }

        [Column]
        public decimal glass_fusing { get; set; }

        [Column]
        public decimal ham_radio { get; set; }

        [Column]
        public decimal jewelry { get; set; }

        [Column]
        public decimal lampworking { get; set; }

        [Column]
        public decimal laser { get; set; }

        [Column]
        public decimal leather { get; set; }

        [Column]
        public decimal long_arm { get; set; }

        [Column]
        public decimal makerfaire { get; set; }

        [Column]
        public decimal metal { get; set; }

        [Column]
        public decimal neon { get; set; }

        [Column]
        public decimal paint { get; set; }

        [Column]
        public decimal power_wheel { get; set; }

        [Column]
        public decimal print { get; set; }

        [Column]
        public string reason { get; set; }

        [Column]
        public decimal soda { get; set; }

        [Column]
        public uint spending_id { get; set; }

        [Column]
        public decimal stained_glass { get; set; }

        [Column]
        public decimal threed_printer { get; set; }

        [Column]
        public decimal tiger_lily { get; set; }

        [Column]
        public DateTime time { get; set; }

        [Column]
        public decimal vacuum { get; set; }

        [Column]
        public decimal welding { get; set; }

        [Column]
        public decimal wood { get; set; }

    }

    [TableName("`area_funding`.`total_committed`")]
    [ExplicitColumns]
    public partial class total_committed
    {
        [Column]
        public decimal? total { get; set; }

    }

}

