using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;

namespace BLL
{
    public class FinanceClass
    {
        private Session _session;
        public long classid { get; set; }
        public long appid { get; set; }
        public string classname { get; set; }
        public short calctype { get; set; }
        public string unit { get; set; }
        public decimal price { get; set; }
        public string customprice { get; set; }
        public bool isenable { get; set; }

        public FinanceClass(Session session, financeclass fclass)
        {
            this._session = session;
            this.classid = fclass.classid;
            this.appid = fclass.appid;
            this.classname = fclass.classname;
            this.calctype = fclass.calctype;
            this.unit = fclass.unit;
            this.price = fclass.price;
            this.customprice = fclass.customprice;
            this.isenable = fclass.isenable;
        }
    }
}
