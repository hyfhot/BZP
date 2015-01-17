using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMM
{
    /// <summary>
    /// 数据库字段相关的常量定义，如记录状态
    /// </summary>
    public class ConstantDefine
    {
        //通用字段，STATUS字段定义:-1删除；0正常；1锁定；2作废
        /// <summary>
        /// 删除
        /// </summary>
        public const short DB_COMM_COL_STATUS_DELETE = -1;
        /// <summary>
        /// 正常
        /// </summary>
        public const short DB_COMM_COL_STATUS_OK = 0;
        /// <summary>
        /// 锁定
        /// </summary>
        public const short DB_COMM_COL_STATUS_LOCK = 1;
        /// <summary>
        /// 作废
        /// </summary>
        public const short DB_COMM_COL_STATUS_INVALID = 2;

        //出租表，rentstatus字段定义:出租状态：0已预订；1已签合同；2已退房；3已转让；4已续签
        /// <summary>
        /// 已预订
        /// </summary>
        public const short DB_RENT_COL_RENTSTATUS_PRE = 0; //已预订
        /// <summary>
        /// 已签合同
        /// </summary>
        public const short DB_RENT_COL_RENSTATUS_RENT = 1; //签订合同
        /// <summary>
        /// 已退房
        /// </summary>
        public const short DB_RENT_COL_RENSTATUS_BACK = 2; //退租
        /// <summary>
        /// 已转让
        /// </summary>
        public const short DB_RENT_COL_RENSTATUS_TRANSFER = 3; //已转让
        /// <summary>
        /// 已续签
        /// </summary>
        public const short DB_RENT_COL_RENSTATUS_CONTINUE = 4; //已续租

        //收费类目表，calctype字段定义:计费类型：1，每月固定金额；2，每月按量计费；3，每月按量阶梯计费；4，手动收款。
        /// <summary>
        /// 每月固定金额
        /// </summary>
        public const short DB_CLASS_COL_CALCTYPE_MONTHSTATIC = 1;
        /// <summary>
        /// 每月按量计费
        /// </summary>
        public const short DB_CLASS_COL_CALCTYPE_MONTHAMOUNT = 2;
        /// <summary>
        /// 每月按量阶梯计费
        /// </summary>
        public const short DB_CLASS_COL_CALCTYPE_MONTHSTEPAMOUNT = 3;
        /// <summary>
        /// 手动收款
        /// </summary>
        public const short DB_CLASS_COL_CALCTYPE_MANUAL = 4;

    }
}
