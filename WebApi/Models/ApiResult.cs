using COMM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// API服务器返回数据的封装
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 返回代码,0代表正常执行,>0为错误代码
        /// </summary>
        public int ResultCode = 0;
        /// <summary>
        /// 返回的提示信息
        /// </summary>
        public string ResultMessage = "";
        /// <summary>
        /// 返回的业务对象
        /// </summary>
        public object ResultData = null;
        /// <summary>
        /// 触发的错误信息
        /// </summary>
        public IList<BLLException> Exception = null;
        /// <summary>
        /// 触发的警告信息
        /// </summary>
        public IList<Warning> Warnings = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        public ApiResult(object data)
        {
            ResultData = data;
        }

        /// <summary>
        /// 空构造函数
        /// </summary>
        public ApiResult()
        {
        }

        /// <summary>
        /// 返回执行OK
        /// </summary>
        /// <returns></returns>
        public static ApiResult Ok()
        {
            return new ApiResult();
        }
    }
}