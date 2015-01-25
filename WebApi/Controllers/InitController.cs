using BLL;
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
    /// <summary>
    /// 初始化接口类
    /// </summary>
    public class InitController : BaseController
    {
        /// <summary>
        /// 初始化账号信息
        /// </summary>
        /// <param name="inmodel"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ApiResult InitApp(InitAppBindingModels inmodel)
        {
            ApiResult result = null;
            try
            {
                Session session = Session.Get(inmodel.sessionid);
                session._app.appname = inmodel.appname;

            }
            catch (Exception ex)
            {
                result = BuildErrorResult(ex);
            }

            return result;
        }
    }
}
