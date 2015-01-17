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
        /// 账号ID，登陆成功后保存
        /// </summary>
        public long AppID;
        /// <summary>
        /// 令牌，首次连接时生成，用户加密敏感数据。
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
            Token = Helper.String.GetRndString(TOKEN_LEN);
            //连接数据库并新增session对象,同时获取SessionID
            SessionID = DAL.SecurityManager.NewSession(Token);
        }

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
        /// 静态构造函数
        /// </summary>
        /// <returns></returns>
        public static Session New()
        {
            return new Session();
        }
    }
}
