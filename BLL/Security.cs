using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;

namespace BLL
{
    /// <summary>
    /// 安全逻辑类
    /// </summary>
    public class Security
    {
        /// <summary>
        /// 登陆系统
        /// </summary>
        /// <returns></returns>
        public static bool UserLogin(Session session, string username, string password)
        {
            user useritem = DAL.SecurityManager.CheckUserPassword(username, password);
            if (useritem != null)
            {
                session.LoginTime = DateTime.Now;
                session.AppID = useritem.appid;
                session.UserID = useritem.userid;
                session.UserName = useritem.username;

                return true;
            }
            else return false;
        }

        /// <summary>
        /// 开始连接，获取连接令牌
        /// </summary>
        /// <returns></returns>
        public static Session Connect()
        {
            return new Session();
        }
    }
}
