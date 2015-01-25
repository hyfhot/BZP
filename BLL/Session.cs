using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    /// <summary>
    /// 会话对象
    /// </summary>
    public class Session
    {
        /// <summary>
        /// 令牌字符串的长度
        /// </summary>
        private const int TOKEN_LEN = 32;
        /// <summary>
        /// 用户编码，用户登陆成功后保存至该值。
        /// </summary>
        public long UserID;
        /// <summary>
        /// 用户姓名，用户登陆成功后保存至该值。
        /// </summary>
        public string UserName;
        /// <summary>
        /// 用户手机号码，用户登陆成功后保存至该值。
        /// </summary>
        public string Mobile;
        /// <summary>
        /// 账号ID，登陆成功后保存
        /// </summary>
        public long AppID;
        /// <summary>
        /// 随机字符串，首次连接时生成，用户加密敏感数据。
        /// </summary>
        public string RndStr;
        /// <summary>
        /// 令牌，登陆后生成，用于判断用户合法性。
        /// 生成规则：sessionid，time_login，userid，rndstr，通过MD5加密后字符串
        /// </summary>
        public string Token;
        /// <summary>
        /// 首次连接时间
        /// </summary>
        public DateTime ConnectTime;
        /// <summary>
        /// 登陆时间
        /// </summary>
        public DateTime LoginTime;
        /// <summary>
        /// 注销时间
        /// </summary>
        public System.DateTime LogoutTime;
        /// <summary>
        /// 租客ID，租客登陆成功后保存至该值。
        /// </summary>
        public long CustomerID;
        /// <summary>
        /// 会话ID，首次连接时自动生成
        /// </summary>
        public long SessionID;

        /// <summary>
        /// 管家婆账号对象
        /// </summary>
        public App _app = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Session()
        {
            ConnectTime = DateTime.Now;
            RndStr = Helper.StringLib.GetRndString(TOKEN_LEN);
            //连接数据库并新增session对象,同时获取SessionID
            SessionID = DAL.SecurityManager.NewSession(RndStr);

            //保存到全局静态Session池中
            SessionList.Add(this);
        }

        /// <summary>
        /// 登陆(明文)
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public bool Login(string username, string password)
        {
            if (Security.UserLogin(this, username, password))
            { 
                //如果登陆成功，session对象中已经有appid和用户信息了
                _app = new App(this,this.AppID);

                return true;
            }
            else { 
                return false; 
            }
        }

        /// <summary>
        /// 登陆(密文)
        /// 用户名(手机号码)+密码+随机字符串 经MD5加密后的字符串进行对比
        /// </summary>
        /// <param name="loginstr">加密后的密文</param>
        /// <returns>是否成功</returns>
        public bool Login(string loginstr)
        {
            if (Security.UserLogin(this, loginstr))
            {
                //如果登陆成功，session对象中已经有appid和用户信息了
                _app = new App(this, this.AppID);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="verifycode">手机校验码</param>
        /// <returns></returns>
        public bool Register(string verifycode)
        {
            if (Security.UserRegister(this, verifycode))
            {
                //如果注册成功，session对象中已经有appid和管理用户信息了
                _app = new App(this, this.AppID);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 创建新Session静态函数
        /// </summary>
        /// <returns></returns>
        public static Session New()
        {
            return new Session();
        }

        /// <summary>
        /// 获取经验证的合法用户
        /// </summary>
        /// <returns></returns>
        public static Session Get(long sessionid, string token)
        {
            return SessionList.FirstOrDefault(c => c.SessionID == sessionid && c.Token == token);
        }

        /// <summary>
        /// 获取用户session,用于注册登录时
        /// </summary>
        /// <param name="sessionid"></param>
        /// <returns></returns>
        public static Session Get(long sessionid)
        {
            return SessionList.FirstOrDefault(c => c.SessionID == sessionid);
        }

        private static IList<Session> SessionList;

        /// <summary>
        /// 静态构造函数；
        /// </summary>
        static Session()
        {
            SessionList = new List<Session>();
        }
    }
}
