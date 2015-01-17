using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMM
{
    public enum WarnCode
    {
        #region 数据库警告代码常量
        DB_CONNECT_CANT = 1001,
        DB_CALCCONFIG_NORECORD = 1002,
        #endregion
        #region 配置警告代码常量
        CF_CALCCONFIG_CUSTOMPRICE_INVALID = 2001,
        CF_CALCCONFIG_CUSTOMPRICE_CANTDESERIALIZE = 2002,
        #endregion
        #region 操作权限警告代码常量
        //该出租屋使用默认计费设置，请到系统设置中修改！
        OP_CALCCONFIG_MODIFY_CANT = 3011,
        #endregion
        #region 认证警告代码常量
        AU_LOGIN_NO = 4001,
        #endregion
        #region 系统警告代码常量
        //无法保存日志
        SY_LOG_CANTWRITE = 5001,
        #endregion
    }

    /// <summary>
    /// 警告类型
    /// </summary>
    public enum WarnType
    {
        /// <summary>
        /// 数据库警告
        /// </summary>
        DBWarn = 1,
        /// <summary>
        /// 设置警告
        /// </summary>
        ConfigWarn = 2,
        /// <summary>
        /// 操作权限警告
        /// </summary>
        OperationWarn = 3,
        /// <summary>
        /// 认证警告
        /// </summary>
        AuthWarn = 4,
        /// <summary>
        /// 系统警告
        /// </summary>
        SystemWarn = 5
    };

    public class Warning
    {
        public static Warning New(WarnType warntype, WarnCode warncode, string warnmsg, Exception innerException = null)
        {
            return new Warning(warntype,warncode,warnmsg,innerException);
        }
        public static Warning NewDBWarn(WarnCode warncode, string warnmsg, Exception innerException = null)
        {
            return new Warning(WarnType.DBWarn, warncode, warnmsg, innerException);
        }
        public static Warning NewConfigWarn(WarnCode warncode, string warnmsg, Exception innerException = null)
        {
            return new Warning(WarnType.ConfigWarn, warncode, warnmsg, innerException);
        }
        public static Warning NewOperationWarn(WarnCode warncode, string warnmsg, Exception innerException = null)
        {
            return new Warning(WarnType.OperationWarn, warncode, warnmsg, innerException);
        }
        public static Warning NewAuthWarn(WarnCode warncode, string warnmsg, Exception innerException = null)
        {
            return new Warning(WarnType.AuthWarn, warncode, warnmsg, innerException);
        }
        public static Warning NewSystemWarn(WarnCode warncode, string warnmsg, Exception innerException = null)
        {
            return new Warning(WarnType.SystemWarn, warncode, warnmsg, innerException);
        }

        public static void AddWarning(Warning warn)
        {
            string strlog = "";
            if (warn.innerException == null)
                strlog = string.Format("WARNING#{0}#{1}#{2}#{3}", DateTime.Now.ToShortTimeString(), warn.warntype.ToString(), warn.warncode.ToString(), warn.warnmsg);
            else
                strlog = string.Format("WARNING#{0}#{1}#{2}#{3}#{4}#{5}", DateTime.Now.ToShortTimeString(), warn.warntype.ToString(), warn.warncode.ToString(), warn.warnmsg, warn.innerException.Message, warn.innerException.StackTrace);

            FileLoger.ErrorLoger.AddLog(strlog);
        }

        private WarnType warntype;
        private WarnCode warncode;
        private string warnmsg;
        private Exception innerException;

        public Warning(WarnType warntype, WarnCode warncode, string warnmsg, Exception innerException = null)
        {
            this.warntype = warntype;
            this.warncode = warncode;
            this.warnmsg = warnmsg;
            this.innerException = innerException;

            Warning.AddWarning(this);
        }
    }
}
