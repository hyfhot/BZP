using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class Building
    {
        private Session _session;
        public long buildid { get; set; }
        public long appid { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public int floors { get; set; }
        public int rooms { get; set; }
        public string images { get; set; }
        public string description { get; set; }

        public IList<Room> _rooms { get; set; }

        public Building(Session session,  building build)
        {
            this._session = session;
            this.buildid = build.buildid;
            this.appid = build.appid;
            this.name = build.name;
            this.address = build.address;
            this.floors = build.floors.HasValue ? build.floors.Value : 0;
            this.rooms = build.rooms.HasValue ? build.rooms.Value : 0;
            this.images = build.images;
            this.description = build.description;

            //加载出租屋数据
            var dalrooms = RentManager.GetBuildRooms(buildid);
            _rooms = dalrooms.Select(c => new Room(_session,c)).ToList();
        }
    }
}
