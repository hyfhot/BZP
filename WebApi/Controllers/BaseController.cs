using WebApi.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Text;
using System.IO;
using WebApi.Models;
using COMM;
using System.Collections.Specialized;

namespace WebApi.Controllers
{/// <summary>  
    /// Controller的基类，用于实现适合业务场景的基础功能  
    /// </summary>  
    /// <typeparam name="T"></typeparam>  
    [BasicAuthentication]
    public abstract class BaseController : ApiController
    {
        /*
            ApiResult result = null;
            try
            {
                //do something
            }
            catch (Exception ex)
            {
                result = BuildErrorResult(ex);
            }

            return result;
        */

        /// <summary>
        /// 构造错误返回值
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected ApiResult BuildErrorResult(Exception ex)
        {
            ApiResult result = null;
            if(ex is BLLException)
            {
                result = new ApiResult();
                result.ResultCode = (int)((BLLException)ex).ErrCode;
                result.ResultMessage += ((BLLException)ex).ErrMsg + "\n";
                result.Exception = new List<BLLException>();
                result.Exception.Add((BLLException)ex);
            }else
            {
                result = new ApiResult();
                result.ResultCode = (int)ex.HResult;
                result.ResultMessage += ex.Message + "\n";
                result.Exception = new List<BLLException>();
                result.Exception.Add(new BLLException(ErrorType.SystemError, ErrorCode.SY_UNKNOW, ex.Message, ex));
            }

            return result;
        }

        //protected async void ParseRequestFormDataParams()
        //{
        //    HttpContent content = Request.Content;
        //    if (content.IsFormData())
        //    {
        //        //return await content.ReadAsFormDataAsync();
        //    }
        //    else
        //    {
        //        //return null;
        //    }
        //}
    } 
}