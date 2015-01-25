using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Security;

namespace WebApi.Attributes
{
    /// <summary>  
    /// 基本验证Attribtue，用以Action的权限处理  
    /// </summary>  
    public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        /// <summary>  
        /// 检查用户是否有该Action执行的操作权限  
        /// </summary>  
        /// <param name="actionContext"></param>  
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //判断是否是支持匿名调用  
            var attr = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
            bool isAnonymous = attr.Any(a => a is AllowAnonymousAttribute);

            //支持匿名调用，则继续执行；
            if (isAnonymous)
                base.OnActionExecuting(actionContext);
            else //否则校验授权信息  
            {
                var sessionidheader = actionContext.Request.Headers.FirstOrDefault(c => c.Key == "sessionid");
                var tokenheader = actionContext.Request.Headers.FirstOrDefault(c => c.Key == "token");
                //请求头中包含sessionid和token，并且能通过验证
                if (sessionidheader.Key != null && tokenheader.Key != null && ValidateUserTicket(sessionidheader, tokenheader))
                {
                    base.OnActionExecuting(actionContext);
                }
                else //否则返回未认证消息
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }
        }

        /// <summary>
        /// 动作执行完成后，增加跨域调用的支持
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Headers", "X-Requested-With");
        }

        /// <summary>
        /// 校验用户合法性
        /// </summary>
        /// <param name="sessionidheader"></param>
        /// <param name="tokenheader"></param>
        /// <returns></returns>
        private bool ValidateUserTicket(KeyValuePair<string, IEnumerable<string>> sessionidheader, KeyValuePair<string, IEnumerable<string>> tokenheader)
        {
            //检查Session池中是否存在对应Sessionid和token的对象
            long sessionid = 0;
            string token = "";
            if (sessionidheader.Value.Count() == 1 && tokenheader.Value.Count() == 1
                && long.TryParse(sessionidheader.Value.First(), out sessionid)
                && !string.IsNullOrEmpty(sessionidheader.Value.First()))
            {
                token = sessionidheader.Value.First();

                return null != Session.Get(sessionid, token);
            }
            else
                return false;
        }
    }
}
