using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class User
    {
        public long userid { get; set; }
        public long appid { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string realname { get; set; }
        public string credentials_type { get; set; }
        public string credentials_code { get; set; }
        public string credentials_pic { get; set; }
        public string mobile { get; set; }
        public string wechat { get; set; }
        public string qq { get; set; }
        public string role { get; set; }
        public string roleright { get; set; }
        public string description { get; set; }
        public DateTime create_time { get; set; }

        public User(DAL.user user)
        {
            this.userid = user.userid;
            this.appid = user.appid;
            this.username = user.username;
            this.email = user.email;
            this.password = user.password;
            this.realname = user.realname;
            this.credentials_type = user.credentials_type;
            this.credentials_code = user.credentials_code;
            this.credentials_pic = user.credentials_pic;
            this.mobile = user.mobile;
            this.wechat = user.wechat;
            this.qq = user.qq;
            this.role = user.role;
            this.description = user.description;
            this.create_time = user.create_time.HasValue ? user.create_time.Value : DateTime.MinValue;
        }
    }
}
