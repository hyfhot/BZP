using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// 登陆接口的参数实体
    /// </summary>
    public class LoginBindingModels //sessionid,loginname,loginstr
    {
        [Required]
        [Display(Name = "SessionID", Description = "会话ID，通过api/security/connect接口获取")]
        public long sessionid { get; set; }

        [Required]
        [Display(Name = "LoginName", Description = "登陆用户名，可以是注册的手机号码、账号")]
        public string loginname { get; set; }

        [Required]
        [Display(Name = "LoginStr", Description = "登陆密钥，使用用户名(或手机号码)+密码+随机字符串")]
        public string loginstr { get; set; }

        /* 加密方法如下：1，组合元素；2，MD5加密
            string scr = useritem.username + useritem.password + rndstr;
            string desc = Helper.StringLib.MD5Encrypt(scr);
         * or
            scr = useritem.mobile + useritem.password + rndstr;
            desc = Helper.StringLib.MD5Encrypt(scr);
        */
    }

    /// <summary>
    /// 获取手机验证码的参数实体
    /// </summary>
    public class GetVerifyCodeBindingModel //sessionid,mobile
    {
        [Required]
        [Display(Name = "SessionID", Description = "会话ID，通过api/security/connect接口获取")]
        public long sessionid { get; set; }

        [Required]
        [Display(Name = "Mobile", Description = "手机号码，调用api/security/getverifycode接口时使用的手机号码")]
        public string mobile { get; set; }
    }

    /// <summary>
    /// 手机注册的参数实体
    /// </summary>
    public class RegisterBindingModel //sessionid,mobile,code
    {
        [Required]
        [Display(Name = "SessionID",Description ="会话ID，通过api/security/connect接口获取")]
        public long sessionid { get; set; }

        [Required]
        [Display(Name = "Mobile",Description ="手机号码，调用api/security/getverifycode接口时使用的手机号码")]
        public string mobile { get; set; }

        [Required]
        [Display(Name = "Code",Description ="验证码，通过api/security/getverifycode接口获取的验证码，会发送到请求的手机号。")]
        public string code { get; set; }
    }
}