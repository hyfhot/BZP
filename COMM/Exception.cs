using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMM
{
    public enum ErrorCode
    {
        #region 数据库错误代码常量
        DB_CONNECT_CANT = 1001,
        DB_CALCCONFIG_INVALIDERROR = 1002,
        DB_FINANCECLASS_RECORD_NOEXIST = 1003,
        DB_ROOM_RECORD_NOEXIST = 1004,
        DB_CALCCONFIG_RECORD_NOEXIST = 1005,
        DB_RENT_RECORD_NOEXIST = 1006,
        DB_APP_NEWACCOUNT_ERROR = 1007,
        DB_USER_LOGIN_ERROR = 1008,
        #endregion
        #region 配置错误代码常量
        CF_CALCCONFIG_CUSTOMPRICE_INVALID = 2001,
        CF_CALCCONFIG_CUSTOMPRICE_CANTDESERIALIZE = 2002,
        CF_CALCCONFIG_DEFAULSETTING_NOANYRECORD = 2003,
        #endregion
        #region 操作权限错误代码常量
        //该出租屋使用默认计费设置，请到系统设置中修改！
        OP_CALCCONFIG_MODIFY_CANT = 3011,
        #endregion
        #region 认证错误代码常量
        /// <summary>
        /// 未登陆
        /// </summary>
        AU_LOGIN_NO = 4001,
        /// <summary>
        /// 登陆错误，用户名或密码错误
        /// </summary>
        AU_LOGIN_ERROR = 4002,
        #endregion
        #region 系统错误代码常量
        //无法保存日志
        SY_LOG_CANTWRITE = 5001,
        //未连接服务器获取令牌
        SY_CONNECT_UNDO = 5002,
        //未获取手机验证码
        SY_REGISTER_UNGETCODE = 5003,
        //手机验证码错误
        SY_REGISTER_ERRORCODE = 5004,
        //无法识别的错误
        SY_UNKNOW = 5999,
        #endregion
    }

    /// <summary>
    /// 错误类型
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// 数据库错误
        /// </summary>
        DBError = 1,
        /// <summary>
        /// 设置错误
        /// </summary>
        ConfigError = 2,
        /// <summary>
        /// 操作权限错误
        /// </summary>
        OperationError = 3,
        /// <summary>
        /// 认证错误
        /// </summary>
        AuthError = 4,
        /// <summary>
        /// 系统错误
        /// </summary>
        SystemError = 5
    };

    /// <summary>
    /// 逻辑层错误类
    /// </summary>
    public class BLLException : Exception
    {
        public static BLLException New(ErrorType type, ErrorCode errcode, string errmsg, Exception inner = null)
        {
            return new BLLException(type, errcode, errmsg, inner);
        }
        public static DBException NewDBError(ErrorCode errcode, string errmsg, Exception inner = null)
        {
            return new DBException(errcode, errmsg, inner);
        }
        public static ConfigException NewConfigError(ErrorCode errcode, string errmsg, Exception inner = null)
        {
            return new ConfigException(errcode, errmsg, inner);
        }
        public static OperationException NewOperationError(ErrorCode errcode, string errmsg, Exception inner = null)
        {
            return new OperationException(errcode, errmsg, inner);
        }
        public static AuthException NewAuthError(ErrorCode errcode, string errmsg, Exception inner = null)
        {
            return new AuthException(errcode, errmsg, inner);
        }
        public static SysException NewSystemError(ErrorCode errcode, string errmsg, Exception inner = null)
        {
            return new SysException(errcode, errmsg, inner);
        }

        public static void AddException(BLLException ex)
        {
            string strlog = "";
            if(ex.InnerException == null)
                strlog = string.Format("ERROR#{0}#{1}#{2}#{3}", DateTime.Now.ToShortTimeString(), ex._errtype.ToString(), ex._errcode.ToString(), ex.Message);
            else
                strlog = string.Format("ERROR#{0}#{1}#{2}#{3}#{4}#{5}", DateTime.Now.ToShortTimeString(), ex._errtype.ToString(), ex._errcode.ToString(), ex.Message, ex.InnerException.Message, ex.InnerException.StackTrace);
            
            FileLoger.ErrorLoger.AddLog(strlog);
        }

        private ErrorType _errtype;
        private ErrorCode _errcode;

        public ErrorType ErrType {
            get
            {
                return _errtype;
            }
        }

        public ErrorCode ErrCode
        {
            get
            {
                return _errcode;
            }
        }

        public string ErrMsg
        {
            get
            {
                return this.Message;
            }
        }

        public BLLException(ErrorType type, ErrorCode errcode, string errmsg, Exception inner = null)
            : base(errmsg, inner)
        {
            _errtype = type;
            _errcode = errcode;

            BLLException.AddException(this);
        }
    }

    /// <summary>
    /// 数据库异常类
    /// </summary>
    public class DBException : BLLException
    {
        public DBException(ErrorCode errcode, string errmsg, Exception inner = null)
            : base(ErrorType.DBError, errcode,errmsg, inner)
        {
        }
    }

    /// <summary>
    /// 配置异常类
    /// </summary>
    public class ConfigException : BLLException
    {
        public ConfigException(ErrorCode errcode, string errmsg, Exception inner = null)
            : base(ErrorType.ConfigError, errcode,errmsg, inner)
        {
        }
    }

    /// <summary>
    /// 操作权限异常类
    /// </summary>
    public class OperationException : BLLException
    {
        public OperationException(ErrorCode errcode, string errmsg, Exception inner = null)
            : base(ErrorType.OperationError, errcode,errmsg, inner)
        {
        }
    }

    /// <summary>
    /// 认证异常类
    /// </summary>
    public class AuthException : BLLException
    {
        public AuthException(ErrorCode errcode, string errmsg, Exception inner = null)
            : base(ErrorType.AuthError, errcode,errmsg, inner)
        {
        }
    }

    /// <summary>
    /// 系统异常类
    /// </summary>
    public class SysException : BLLException
    {
        public SysException(ErrorCode errcode, string errmsg, Exception inner = null)
            : base(ErrorType.SystemError, errcode, errmsg, inner)
        {
        }
    }
}
