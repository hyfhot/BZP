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
    public class InModelBindingModels //sessionid,appname,password
    {
        [Required]
        [Display(Name = "ActionName", Description = "动作名称")]
        public string actionname { get; set; }
    }
}