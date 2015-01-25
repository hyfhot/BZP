using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Attributes;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class HelperController : BaseController
    {
        /// <summary>
        /// 获取Action帮助
        /// </summary>
        /// <param name="inmodel"></param>
        /// <returns></returns>
        /// Get: api/Helper/InModel
        [HttpPost]
        [AllowAnonymous]
        public object InModel(InModelBindingModels inmodel)
        {
            //InModelBindingModels inmodel = new InModelBindingModels() { actionname = "api/Helper/InModel" };
            HttpContent content = Request.Content;
            //string formdata = asyc content.ReadAsFormDataAsync();
            string actionname = inmodel.actionname;
            string action = actionname.Trim().ToUpper();
            if(action == "api/Security/Connect".ToUpper())
            {
                return new { Auth  = false, model = new object()};
            }
            else if (action == "api/Security/Login".ToUpper())
            {
                return new { Auth = false, model = new LoginBindingModels() };
            }
            else if (action == "api/Security/GetVerifyCode".ToUpper())
            {
                return new { Auth = false, model = new GetVerifyCodeBindingModel() };
            }
            else if (action == "api/Security/Register".ToUpper())
            {
                return new { Auth = false, model = new RegisterBindingModel() };
            }
            else if (action == "api/Init/InitApp".ToUpper())
            {
                return new { Auth = true, model = new InitAppBindingModels() };
            }
            else if (action == "api/Helper/InModel".ToUpper())
            {
                return new { Auth = false, model = new InModelBindingModels() };
            }
            else
            {
                return new object();
            }

        }
    }
}
