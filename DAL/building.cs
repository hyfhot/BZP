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
    
    public partial class building
    {
        public long buildid { get; set; }
        public long appid { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Nullable<int> floors { get; set; }
        public Nullable<int> rooms { get; set; }
        public string images { get; set; }
        public string description { get; set; }
        public short STATUS { get; set; }
    }
}
