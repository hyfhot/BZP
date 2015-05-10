using BLL;
using COMM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Attributes;
using WebApi.Controllers;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class SecurityController : BaseController
    {
        /// <summary>
        /// 请求连接，返回SessionID与随机字符串
        /// </summary>
        /// <returns>SessionID与随机字符串rndstr</returns>
        /// Post: api/Security/Connect
        [HttpPost]
        [AllowAnonymous]
        public ApiResult Connect()
        {
            ApiResult result = null;
            try
            {
                Session session = Security.Connect();
                result = new ApiResult(new { sessionid = session.SessionID, rndstr = session.RndStr });
            }
            catch (Exception ex)
            {
                result = BuildErrorResult(ex);
            }

            return result;
        }
        
        ///
        /// <summary>
        /// 请求登陆，返回Token
        /// </summary>
        /// <param name="sessionid">回话ID</param>
        /// <param name="loginname">登陆用户名</param>
        /// <param name="loginstr">登陆字符串，由手机号、密码、RndStr通过MD5加密而成</param>
        /// <returns>Token</returns>
        /// Post: api/Security/Login
        [HttpPost]
        [AllowAnonymous]
        public ApiResult Login(LoginBindingModels login)
        {
            try
            {
                //ParseRequestFormDataParams();
                long sessionid = login.sessionid;
                string loginname = login.loginname, loginstr = login.loginstr;
                Session session = Session.Get(sessionid);
                if (session == null)
                {
                    return BuildErrorResult(new BLLException(ErrorType.SystemError, ErrorCode.SY_CONNECT_UNDO, "请先发起连接命令获取令牌！"));
                }
                else
                {
                    session.Mobile = loginname;
                    if (session.Login(loginstr))
                    {
                        return new ApiResult(new { token = session.Token, app = session._app });
                    }
                    else
                    {
                        return BuildErrorResult(new BLLException(ErrorType.SystemError, ErrorCode.AU_LOGIN_ERROR, "用户名或密码错误！"));
                    }
                }
            }
            catch (Exception ex)
            {
                return BuildErrorResult(ex);
            }
        }

        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        /// Post: api/Security/GetVerifyCode
        [HttpPost]
        [AllowAnonymous]
        public ApiResult GetVerifyCode(GetVerifyCodeBindingModel inmodel)
        {
            try
            {
                Session session = Session.Get(inmodel.sessionid);
                if (session == null)
                {
                    return BuildErrorResult(new BLLException(ErrorType.SystemError, ErrorCode.SY_CONNECT_UNDO, "请先发起连接命令获取令牌！"));
                }
                else
                {
                    session.Mobile = inmodel.mobile;
                    return ApiResult.Ok();
                }
            }
            catch (Exception ex)
            {
                return BuildErrorResult(ex);
            }

        }

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="inmodel"></param>
        /// <returns></returns>
        /// Post: api/Security/Register
        [HttpPost]
        [AllowAnonymous]
        public ApiResult Register(RegisterBindingModel inmodel)
        {
            try
            {
                Session session = Session.Get(inmodel.sessionid);
                if (session == null)
                {
                    return BuildErrorResult(new BLLException(ErrorType.SystemError, ErrorCode.SY_CONNECT_UNDO, "请先发起连接命令获取令牌！"));
                }
                else if (session.Mobile != inmodel.mobile)
                {
                    return BuildErrorResult(new BLLException(ErrorType.SystemError, ErrorCode.SY_REGISTER_UNGETCODE, "请先获取验证码！"));
                }
                else
                {
                    if (session.Register(inmodel.code))
                    {
                        return new ApiResult(new { appid = session.AppID, token = session.Token });
                    }
                    else
                    {
                        return BuildErrorResult(new BLLException(ErrorType.SystemError, ErrorCode.SY_REGISTER_ERRORCODE, "校验码错误！"));
                    }
                }
            }
            catch (Exception ex)
            {
                return BuildErrorResult(ex);
            }
        }
        
    }
}
