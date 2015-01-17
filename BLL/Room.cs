using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    /// <summary>
    /// 房间管理对象，包括增删改查等基本操作
    /// </summary>
    public class Room
    {
        private Session _session;

        public long roomid { get; set; }
        public long appid { get; set; }
        public long buildid { get; set; }
        public string code { get; set; }
        public string roomtype { get; set; }
        public string packlevel { get; set; }
        public string direction { get; set; }
        public short floor { get; set; }
        public decimal area { get; set; }
        public decimal price { get; set; }
        public string deploy { get; set; }
        public string images { get; set; }
        public string pledge { get; set; }
        public decimal payment { get; set; }
        public string description { get; set; }

        public IList<RentItem> RentItemList = new List<RentItem>();

        public Room(Session session,rooms room)
        {
            this._session = session;
            this.roomid = room.roomid;
            this.appid = room.appid;
            this.buildid = room.buildid;
            this.code = room.code;
            this.roomtype = room.roomtype;
            this.packlevel = room.packlevel;
            this.direction = room.direction;
            this.floor = room.floor.HasValue ? room.floor.Value : (short)0;
            this.area = room.area.HasValue ? room.area.Value : (decimal)0; ;
            this.price = room.price.HasValue ? room.price.Value : (decimal)0;
            this.deploy = room.deploy;
            this.images = room.images;
            this.pledge = room.pledge;
            this.payment = room.payment.HasValue ? room.payment.Value : (decimal)0;
            this.description = room.description;

            //获取最新一期收费信息
            GetNewMonthRentItem();
        }

        public bool GetMonthRentItem(short year, short month)
        {
            //缓存中存在，则直接返回True
            foreach(RentItem item in RentItemList)
            {
                if (item.year == year && item.month == month) return true;
            }

            //加载收费信息
            try
            {
                RentItem newitem = new RentItem(_session, year, month, this.roomid);
                RentItemList.Add(newitem);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool GetNewMonthRentItem()
        {
            DateTime newmonth = DateTime.Now.Day > 5 ? DateTime.Now : DateTime.Now.AddMonths(-1);

            return GetMonthRentItem((short)newmonth.Year, (short)newmonth.Month);
        }
    }
}
