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
    
    public partial class rooms
    {
        public long roomid { get; set; }
        public long appid { get; set; }
        public long buildid { get; set; }
        public string code { get; set; }
        public string roomtype { get; set; }
        public string packlevel { get; set; }
        public string direction { get; set; }
        public Nullable<short> floor { get; set; }
        public Nullable<decimal> area { get; set; }
        public Nullable<decimal> price { get; set; }
        public string deploy { get; set; }
        public string images { get; set; }
        public string pledge { get; set; }
        public Nullable<decimal> payment { get; set; }
        public string description { get; set; }
        public short STATUS { get; set; }
        public long customerid { get; set; }
    }
}
