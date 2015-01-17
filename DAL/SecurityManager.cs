using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COMM;

namespace DAL
{
    public class SecurityManager
    {
        /// <summary>
        /// 新增一个session数据，并返回sessionid值
        /// </summary>
        /// <param name="token">令牌，用于客户端加密</param>
        /// <returns>sessionid</returns>
        public static long NewSession(string token)
        {
            using (Entities db = new Entities())
            {
                session newsession = new session();
                newsession.token = token;
                newsession.time_connect = DateTime.Now;
                db.session.Add(newsession);
                db.SaveChanges();

                return newsession.sessionid;
            }
        }

        public static user CheckUserPassword(string username, string password)
        {
            using (Entities db = new Entities())
            {
                return db.user.FirstOrDefault(c => c.username == username && c.password == password && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
            }
        }

        public static appaccount GetAppAccount(long appid)
        {
            using (Entities db = new Entities())
            {
                return db.appaccount.FirstOrDefault(c => c.appid == appid && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
            }
        }

        public static IList<user> GetAppUsers(long appid)
        {
            using (Entities db = new Entities())
            {
                return (from g in db.user where g.appid == appid && g.STATUS ==ConstantDefine.DB_COMM_COL_STATUS_OK select g).ToList();
            }
        }
    }
}
