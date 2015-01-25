using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;

namespace BLL
{
    /// <summary>
    /// 管家婆账号类
    /// </summary>
    public class App
    {
        private Session _session;
        public int appid { get; set; }
        public string appname { get; set; }
        public long adminuserid { get; set; }

        public IList<Building> _builds { get; set; }
        public IList<User> _users { get; set; }
        public IList<FinanceClass> _financeclasslist { get; set; }

        public App(Session session,long appid)
        {
            this._session = session;
            appaccount app = SecurityManager.GetAppAccount(appid);
            if(app != null)
            {
                appid = app.appid;
                appname = app.appname;
                adminuserid = app.adminuserid;

                //加载用户数据
                var dalusers = SecurityManager.GetAppUsers(appid);
                _users = dalusers.Select(c => new User(c)).ToList();

                //加载楼房屋数据
                var dalbuilds = RentManager.GetAppBuilds(appid);
                _builds = dalbuilds.Select(c => new Building(_session, c)).ToList();

                //加载收费项目数据
                var dalfinanceclass = FinanceManager.GetFinanceClassList(appid);
                _financeclasslist = dalfinanceclass.Select(c => new FinanceClass(_session, c)).ToList();
            }
        }
    }
}
