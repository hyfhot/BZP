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
    
    public partial class calcvaluelog
    {
        public long calclogid { get; set; }
        public long appid { get; set; }
        public long buildid { get; set; }
        public long roomid { get; set; }
        public long classid { get; set; }
        public short year { get; set; }
        public short month { get; set; }
        public System.DateTime logtime { get; set; }
        public System.DateTime beginday { get; set; }
        public System.DateTime endday { get; set; }
        public Nullable<decimal> value_init { get; set; }
        public Nullable<decimal> value_end { get; set; }
        public Nullable<decimal> value_real { get; set; }
        public decimal price { get; set; }
        public string customprice { get; set; }
        public decimal money { get; set; }
        public long loger_id { get; set; }
        public string loger_name { get; set; }
        public short STATUS { get; set; }
    }
}