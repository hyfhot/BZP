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
    
    public partial class session
    {
        public long sessionid { get; set; }
        public string rndstr { get; set; }
        public string token { get; set; }
        public Nullable<System.DateTime> time_connect { get; set; }
        public Nullable<System.DateTime> time_login { get; set; }
        public Nullable<System.DateTime> time_logout { get; set; }
        public Nullable<long> appid { get; set; }
        public Nullable<long> userid { get; set; }
        public Nullable<long> customerid { get; set; }
        public short STATUS { get; set; }
    }
}
