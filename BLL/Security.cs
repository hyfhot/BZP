using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using COMM;

namespace BLL
{
    /// <summary>
    /// 安全逻辑类
    /// </summary>
    public class Security
    {
        /// <summary>
        /// 给Session相关属性赋值
        /// </summary>
        /// <param name="session"></param>
        /// <param name="useritem"></param>
        private static void CopyAttrToSession(ref Session session, user useritem)
        {
            session.LoginTime = DateTime.Now;
            session.AppID = useritem.appid;
            session.UserID = useritem.userid;
            session.UserName = useritem.username;
            session.Mobile = useritem.mobile;
            session.Token = BuildToken(session.SessionID, session.LoginTime,session.UserID,session.RndStr);
        }

        /// <summary>
        /// 校验密文
        /// </summary>
        /// <param name="useritem"></param>
        /// <param name="rndstr"></param>
        /// <param name="encryptstr"></param>
        /// <returns></returns>
        private static bool CheckEncrypt(user useritem, string rndstr, string encryptstr)
        {
            string scr = useritem.username + useritem.password + rndstr;
            string desc = Helper.StringLib.MD5Encrypt(scr);
            if (desc.Equals(encryptstr))
            {
                return true;
            }
            else
            {
                scr = useritem.mobile + useritem.password + rndstr;
                desc = Helper.StringLib.MD5Encrypt(scr);
                return desc.Equals(encryptstr);
            }
        }

        /// <summary>
        /// 校验手机验证码
        /// </summary>
        /// <param name="verifycode"></param>
        /// <returns></returns>
        private static bool CheckMobileVerifyCode(string verifycode)
        {
            return true;
        }

        /// <summary>
        /// 生成Token访问令牌
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="dateTime"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        private static string BuildToken(long sessionid, DateTime logintime, long userid, string rndstr)
        {
            string scr = string.Format("{0}-{1}-{2}-{3}", sessionid, logintime, userid, rndstr) ;
            return Helper.StringLib.MD5Encrypt(scr);
        }

        /// <summary>
        /// 开始连接，获取连接令牌
        /// </summary>
        /// <returns></returns>
        public static Session Connect()
        {
            return new Session();
        }

        /// <summary>
        /// 登陆系统(明文登陆)
        /// </summary>
        /// <returns></returns>
        public static bool UserLogin(Session session, string username, string password)
        {
            user useritem = DAL.SecurityManager.CheckUserPassword(username, password);
            if (useritem != null)
            {
                CopyAttrToSession(ref session, useritem);
                return true;
            }
            else return false;
        }
        /// <summary>
        /// 登陆系统(密文登陆)
        /// 用户名(手机号码)+密码+随机字符串 经MD5加密后的字符串进行对比
        /// </summary>
        /// <returns></returns>
        public static bool UserLogin(Session session, string loginstr)
        {
            try {
                //通过用户名或手机号码获取用户对象
                user useritem = DAL.SecurityManager.GetUserByName(session.UserName);
                if (useritem == null)
                {
                    useritem = DAL.SecurityManager.GetUserByMobile(session.Mobile);
                }
                //不存在则返回false
                if (useritem == null) return false;

                //校验密文
                if (CheckEncrypt(useritem, session.RndStr, loginstr))
                {
                    CopyAttrToSession(ref session, useritem);
                    return true;
                }
                else
                {
                    Warning.NewAuthWarn(WarnCode.AU_LOGIN_PASSWORD_ERR, string.Format("用户:{0}登陆系统，但输入了错误的密码！", session.Mobile));
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw BLLException.NewDBError(ErrorCode.DB_USER_LOGIN_ERROR, "登陆时出现系统错误，请联系管理员！", ex);
            }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="session"></param>
        /// <param name="verifycode"></param>
        /// <returns></returns>
        internal static bool UserRegister(Session session, string verifycode)
        {
            //校验验证码
            if (CheckMobileVerifyCode(verifycode))
            {
                try
                {
                    //新增包租婆账号
                    appaccount newapp = SecurityManager.NewAppAccount(session.Mobile);
                    session.AppID = newapp.appid;
                    session.LoginTime = DateTime.Now;

                    //新增管理员账号
                    user newuser = SecurityManager.CreateAppAdminUser(newapp.appid, session.Mobile);
                    session.UserID = newuser.userid;
                    session.UserName = newuser.username;
                    session.Token = BuildToken(session.SessionID, session.LoginTime, session.UserID, session.RndStr);
                    return true;
                }
                catch(Exception ex) 
                {
                    throw BLLException.NewDBError(ErrorCode.DB_APP_NEWACCOUNT_ERROR, "注册账号出现错误！", ex);
                }
            }
            else
            {
                Warning.NewAuthWarn(WarnCode.AU_REGISTER_VERIFYCODE_ERR, string.Format("用户尝试手机号码{0}注册，但输入了错误的验证码！", session.Mobile));
                return false;
            }
        }
    }
}
