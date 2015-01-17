//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class rent
    {
        public long rentid { get; set; }
        public long appid { get; set; }
        public long buildid { get; set; }
        public long roomid { get; set; }
        public Nullable<System.DateTime> pre_date { get; set; }
        public Nullable<decimal> pre_money { get; set; }
        public string pre_customername { get; set; }
        public string pre_credentials_type { get; set; }
        public string pre_credentials_code { get; set; }
        public string pre_credentials_pic { get; set; }
        public string pre_mobile { get; set; }
        public Nullable<System.DateTime> pre_expired { get; set; }
        public Nullable<long> pre_operator_id { get; set; }
        public string pre_operator_name { get; set; }
        public Nullable<decimal> pre_backmoney { get; set; }
        public Nullable<long> pre_back_id { get; set; }
        public string pre_back_name { get; set; }
        public Nullable<System.DateTime> contract_begin { get; set; }
        public Nullable<System.DateTime> contract_end { get; set; }
        public string contract_pic { get; set; }
        public Nullable<System.DateTime> contract_date { get; set; }
        public string contract_memo { get; set; }
        public Nullable<decimal> contract_room_price { get; set; }
        public string contract_room_pic { get; set; }
        public Nullable<decimal> contract_room_pledge { get; set; }
        public Nullable<short> contract_room_peoples { get; set; }
        public Nullable<decimal> contract_room_payment { get; set; }
        public Nullable<decimal> contract_water_value { get; set; }
        public Nullable<decimal> contract_elec_value { get; set; }
        public Nullable<long> contract_customer_id { get; set; }
        public string contract_customer_name { get; set; }
        public Nullable<long> contract_operator_id { get; set; }
        public string contract_operator_name { get; set; }
        public Nullable<System.DateTime> back_date { get; set; }
        public Nullable<decimal> back_water_value { get; set; }
        public Nullable<decimal> back_elec_value { get; set; }
        public Nullable<decimal> back_deduction { get; set; }
        public Nullable<decimal> back_deduction_water { get; set; }
        public Nullable<decimal> back_deduction_elec { get; set; }
        public Nullable<decimal> back_deduction_cleaning { get; set; }
        public Nullable<decimal> back_deduction_mar { get; set; }
        public Nullable<decimal> back_deduction_other { get; set; }
        public string back_deduction_memo { get; set; }
        public Nullable<decimal> back_money { get; set; }
        public string back_memo { get; set; }
        public Nullable<long> back_operator_id { get; set; }
        public string back_operator_name { get; set; }
        public Nullable<long> transfer_fromid { get; set; }
        public Nullable<long> transfer_toid { get; set; }
        public Nullable<decimal> transfer_money { get; set; }
        public string transfer_memo { get; set; }
        public Nullable<long> transfer_operator_id { get; set; }
        public string transfer_operator_name { get; set; }
        public short rentstatus { get; set; }
        public short STATUS { get; set; }
    }
}
