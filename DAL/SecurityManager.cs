using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COMM;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace DAL
{
    public class SecurityManager
    {
        /// <summary>
        /// 新增一个session数据，并返回sessionid值
        /// </summary>
        /// <param name="rndstr">令牌，用于客户端加密</param>
        /// <returns>sessionid</returns>
        public static long NewSession(string rndstr)
        {
            using (Entities db = new Entities())
            {
                session newsession = new session();
                newsession.token = rndstr;
                newsession.time_connect = DateTime.Now;
                db.session.Add(newsession);
                db.SaveChanges();

                return newsession.sessionid;
            }
        }

        /// <summary>
        /// 校验用户名与密码,校验成功返回用户对象
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static user CheckUserPassword(string username, string password)
        {
            using (Entities db = new Entities())
            {
                return db.user.FirstOrDefault(c => c.username == username && c.password == password && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
            }
        }

        /// <summary>
        /// 根据用户名获取密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public static user GetUserByName(string username)
        {
            using (Entities db = new Entities())
            {
                return db.user.FirstOrDefault(c => c.username == username && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
            }
        }

        /// <summary>
        /// 根据手机号码获取密码
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns></returns>
        public static user GetUserByMobile(string mobile)
        {
            using (Entities db = new Entities())
            {
                return db.user.FirstOrDefault(c => c.mobile == mobile && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
            }
        }

        /// <summary>
        /// 根据AppID获取App对象
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public static appaccount GetAppAccount(long appid)
        {
            using (Entities db = new Entities())
            {
                return db.appaccount.FirstOrDefault(c => c.appid == appid && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
            }
        }

        //根据AppID获取账号下所有用户信息
        public static IList<user> GetAppUsers(long appid)
        {
            using (Entities db = new Entities())
            {
                return (from g in db.user where g.appid == appid && g.STATUS ==ConstantDefine.DB_COMM_COL_STATUS_OK select g).ToList();
            }
        }

        /// <summary>
        /// 新增包租婆账号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static appaccount NewAppAccount(string name)
        {
            using (Entities db = new Entities())
            {
                appaccount app = new appaccount();
                app.appname = name;
                app.adminuserid = 0;
                app.STATUS = ConstantDefine.DB_COMM_COL_STATUS_OK;

                db.appaccount.Add(app);
                db.SaveChanges();

                return app;
            }
        }

        /// <summary>
        /// 创建新账号下的管理员账号
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static user CreateAppAdminUser(long appid, string mobile)
        {
            using (Entities db = new Entities())
            {
                DbConnection con = ((IObjectContextAdapter)db).ObjectContext.Connection;
                con.Open();
                using(var tran = con.BeginTransaction())
                {
                    try {
                        //新增用户
                        user user = new user();
                        user.appid = appid;
                        user.create_time = DateTime.Now;
                        user.mobile = mobile;
                        user.username = mobile;
                        user.password = "";
                        user.realname = "";
                        user.credentials_type = "";
                        user.credentials_code = "";
                        user.role = "";
                        user.roleright = "";
                        user.STATUS = ConstantDefine.DB_COMM_COL_STATUS_OK;
                        db.user.Add(user);
                        db.SaveChanges();
                        //修改账号表中管理员ID的值
                        appaccount meapp = db.appaccount.FirstOrDefault(c => c.appid == appid && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
                        if (meapp == null) return null;
                        meapp.adminuserid = user.userid;
                        db.SaveChanges();
                        //提交事务
                        tran.Commit();
                        return user;
                    }
                    catch(Exception ex) {
                        //回滚
                        tran.Rollback();
                        throw ex;
                    }
                }

            }
        }
    }
}
