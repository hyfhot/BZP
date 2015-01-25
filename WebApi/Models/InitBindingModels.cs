using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// 初始化账号信息的参数实体
    /// </summary>
    public class InitAppBindingModels //sessionid,appname,password
    {
        [Required]
        [Display(Name = "SessionID", Description = "会话ID，通过api/security/connect接口获取")]
        public long sessionid { get; set; }

        [Required]
        [Display(Name = "AppName", Description = "账号名称，一般是房东姓名")]
        public string appname { get; set; }

        [Required]
        [Display(Name = "Password", Description = "超级管理员密码，对应账号的主账号密码")]
        public string password { get; set; }
    }
}