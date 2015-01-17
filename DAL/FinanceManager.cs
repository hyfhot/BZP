using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using COMM;

namespace DAL
{
    /// <summary>
    /// 与财务数据相关的数据层逻辑
    /// </summary>
    public class FinanceManager
    {
        #region 收费项目相关

        /// <summary>
        /// 取所有收费项目
        /// </summary>
        /// <param name="roomlsit"></param>
        /// <returns></returns>
        public static IList<financeclass> GetFinanceClassList(long appid)
        {
            List<financeclass> classlist = new List<financeclass>();

            using (Entities db = new Entities())
            {
                //获取账号的全局默认收费设定
                classlist = (from g in db.financeclass
                             where appid == g.appid && g.isenable == true &&
                             g.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK
                             select g).ToList();
            }

            return classlist;
        }

        /// <summary>
        /// 取指定收费项目信息
        /// </summary>
        /// <param name="roomlsit"></param>
        /// <returns></returns>
        public static financeclass GetFinanceClassByID(long classid)
        {
            financeclass fclass = null;

            using (Entities db = new Entities())
            {
                //获取默认收费设定
                fclass = db.financeclass.FirstOrDefault(c => c.classid == classid && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
                if (fclass == null)
                {
                    throw new DBException(ErrorCode.DB_FINANCECLASS_RECORD_NOEXIST, "收费项目不存在！");
                }
            }

            return fclass;
        }
        #endregion


        #region 收费设定相关

        /// <summary>
        /// 根据房间列表取房间的收费设定
        /// </summary>
        /// <param name="roomlsit"></param>
        /// <returns></returns>
        public static IList<calcconfig> GetRoomCalcConfigs(long[] roomidlist)
        {
            List<calcconfig> configlist = new List<calcconfig>();

            using (Entities db = new Entities())
            {
                //获取房间的收费设定
                var roomconfigist = (from g in db.calcconfig
                                     where roomidlist.Any(c => c == g.roomid) &&
                                         g.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK
                                     select g).ToList();
                //整理账号和楼房信息
                var roomlist = (from g in db.rooms
                                where roomidlist.Any(c => c == g.roomid) &&
                                    g.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK
                                select g).ToList();
                var appidlist = (from g in roomlist
                                 group g by g.appid into p
                                 select p.Key);
                var buildidlist = (from g in roomlist
                                   group g by g.buildid into p
                                   select p.Key);
                //获取楼房的默认收费设定
                var buildconfigist = (from g in db.calcconfig
                                      where buildidlist.Any(c => c == g.buildid) && g.roomid == 0 &&
                                      g.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK
                                      select g).ToList();
                //获取账号的默认收费设定
                var appconfigist = (from g in db.calcconfig
                                    where appidlist.Any(c => c == g.appid) && g.buildid == 0 && g.roomid == 0 &&
                                    g.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK
                                    select g).ToList();

                //整合数据
                configlist.AddRange(roomconfigist);
                configlist.AddRange(buildconfigist);
                configlist.AddRange(appconfigist);
            }

            return configlist;
        }

        /// <summary>
        /// 更新收费设置
        /// </summary>
        /// <param name="appid">账号id</param>
        /// <param name="buildid">楼房id</param>
        /// <param name="roomid">出租屋id</param>
        /// <param name="classid">收费项目id</param>
        /// <param name="price">单价</param>
        /// <param name="customprice">自定义定价</param>
        /// <param name="userid">操作者id</param>
        /// <returns></returns>
        public static bool UpdateCalcConfig(long appid, long buildid, long roomid, long classid, decimal price, string customprice, long userid)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    var updateitem = db.calcconfig.FirstOrDefault(c => c.appid == appid && c.buildid == buildid && c.roomid == roomid && c.classid == classid
                        && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
                    if (updateitem == null) //需新增
                    {
                        updateitem = new calcconfig()
                        {
                            appid = appid,
                            buildid = buildid,
                            roomid = roomid,
                            classid = classid,
                            price = price,
                            customprice = customprice,
                            createuserid = userid,
                            createtime = DateTime.Now
                        };
                        db.calcconfig.Add(updateitem);
                    }
                    else
                    {
                        //更新
                        updateitem.price = price;
                        updateitem.customprice = customprice;
                        updateitem.createuserid = userid;
                    }
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    //记录错误日志
                    return false;
                }
            }
        }

        /// <summary>
        /// 删除收费设置
        /// </summary>
        /// <param name="appid">账号id</param>
        /// <param name="buildid">楼房id</param>
        /// <param name="roomid">出租屋id</param>
        /// <param name="classid">收费项目id</param>
        /// <returns></returns>
        public static bool DeleteCalcConfig(long appid, long buildid, long roomid, long classid)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    var updateitem = db.calcconfig.FirstOrDefault(c => c.appid == appid && c.buildid == buildid && c.roomid == roomid && c.classid == classid
                        && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
                    if (updateitem != null)
                    {
                        updateitem.STATUS = ConstantDefine.DB_COMM_COL_STATUS_DELETE;
                        db.SaveChanges();
                    }
                    return true;
                }
                catch
                {
                    return false;

                }
            }
        }

        /// <summary>
        /// 作废收费设置
        /// </summary>
        /// <param name="appid">账号id</param>
        /// <param name="buildid">楼房id</param>
        /// <param name="roomid">出租屋id</param>
        /// <param name="classid">收费项目id</param>
        /// <returns></returns>
        public static bool InvalidCalcConfig(long appid, long buildid, long roomid, long classid)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    var updateitem = db.calcconfig.FirstOrDefault(c => c.appid == appid && c.buildid == buildid && c.roomid == roomid && c.classid == classid
                        && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
                    if (updateitem != null)
                    {
                        updateitem.STATUS = ConstantDefine.DB_COMM_COL_STATUS_INVALID;
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        Warning.NewDBWarn(WarnCode.DB_CALCCONFIG_NORECORD, string.Format("待删除的收费设置记录不存在！{0}-{1}-{2}-{3}", appid, buildid, roomid, classid), null);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw BLLException.NewDBError(ErrorCode.DB_CALCCONFIG_INVALIDERROR, "作废收费设置出错！", ex);
                }
            }
        }

        /// <summary>
        /// 设置出租屋使用自定义收费设置
        /// </summary>
        /// <param name="appid">账号id</param>
        /// <param name="buildid">楼房id</param>
        /// <param name="roomid">出租屋id</param>
        /// <param name="classid">收费项目id</param>
        /// <returns></returns>
        public static bool SetRoomUseCustomCalcConfig(long appid, long buildid, long roomid, long classid, long userid)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    var updateitem = db.calcconfig.FirstOrDefault(c => c.appid == appid && c.buildid == buildid && c.roomid == roomid && c.classid == classid
                        && (c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_INVALID || c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK));
                    if (updateitem != null) //有记录存在？
                    {
                        //作废的记录?
                        if (updateitem.STATUS == ConstantDefine.DB_COMM_COL_STATUS_INVALID)
                        {
                            updateitem.STATUS = ConstantDefine.DB_COMM_COL_STATUS_OK;
                        }//正常的记录?
                        else if (updateitem.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK)
                        {
                            Warning.NewDBWarn(WarnCode.DB_CALCCONFIG_NORECORD, string.Format("出租屋的自定义收费设置记录已存在！{0}-{1}-{2}-{3}", appid, buildid, roomid, classid), null);
                            return false;
                        }
                    }
                    else //无记录存在？新增！
                    {
                        calcconfig defaulitem = GetRoomDefaultCalcConfig(appid, buildid, roomid, classid);

                        calcconfig insertitem = new calcconfig()
                        {
                            appid = appid,
                            buildid = buildid,
                            roomid = roomid,
                            classid = classid,
                            price = defaulitem.price,
                            customprice = defaulitem.customprice,
                            createtime = DateTime.Now,
                            createuserid = userid
                        };
                        db.calcconfig.Add(insertitem);
                    }

                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw BLLException.NewDBError(ErrorCode.DB_CALCCONFIG_INVALIDERROR, "设置收费设置出错！", ex);

                }
            }
        }

        /// <summary>
        /// 设置出租屋使用默认收费设置
        /// </summary>
        /// <param name="appid">账号id</param>
        /// <param name="buildid">楼房id</param>
        /// <param name="roomid">出租屋id</param>
        /// <param name="classid">收费项目id</param>
        /// <returns></returns>
        public static bool SetRoomUseDefaultCalcConfig(long appid, long buildid, long roomid, long classid)
        {
            return InvalidCalcConfig(appid, buildid, roomid, classid);
        }

        /// <summary>
        /// 获取收费设定
        /// </summary>
        /// <param name="roomid">出租屋id</param>
        /// <param name="classid">收费项目id</param>
        /// <returns></returns>
        public static calcconfig GetRoomCalcConfig(long roomid, long classid)
        {
            calcconfig result = null;

            using (Entities db = new Entities())
            {
                //获取默认收费设定
                result = db.calcconfig.FirstOrDefault(c => c.roomid == roomid && c.classid == classid
                    && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);

                return result;
            }
        }

        /// <summary>
        /// 获取默认收费设定
        /// </summary>
        /// <param name="appid">账号id</param>
        /// <param name="buildid">楼房id</param>
        /// <param name="roomid">出租屋id</param>
        /// <param name="classid">收费项目id</param>
        /// <returns></returns>
        public static calcconfig GetRoomDefaultCalcConfig(long appid, long buildid, long roomid, long classid)
        {
            using (Entities db = new Entities())
            {
                calcconfig appdefine = null, appconfig = null, buildconfig = null, defaulitem = null;
                financeclass appclassdefine = null;
                buildconfig = db.calcconfig.FirstOrDefault(c => c.appid == appid && c.buildid == buildid && c.roomid == 0 && c.classid == classid
                && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
                if (buildconfig == null)
                {
                    appconfig = db.calcconfig.FirstOrDefault(c => c.appid == appid && c.buildid == 0 && c.roomid == 0 && c.classid == classid
                    && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
                    if (appconfig == null)
                    {
                        appclassdefine = db.financeclass.FirstOrDefault(c => c.appid == appid && c.classid == classid
                        && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
                        if (appclassdefine == null)
                        {
                            throw BLLException.NewConfigError(ErrorCode.CF_CALCCONFIG_DEFAULSETTING_NOANYRECORD, "获取自定义收费设置出错，无法找到任何默认设置填充自定义设置，请先设置账号或楼房的默认设置！", null);
                        }
                        else
                        {
                            appdefine = new calcconfig()
                            {
                                appid = appid,
                                buildid = buildid,
                                roomid = roomid,
                                classid = classid,
                                price = appclassdefine.price,
                                customprice = appclassdefine.customprice
                            };
                        }
                    }
                }
                if (buildconfig != null) { defaulitem = buildconfig; }
                else if (appconfig != null) { defaulitem = appconfig; }
                else if (appdefine != null) { defaulitem = appdefine; }

                return defaulitem;
            }
        }

        /// <summary>
        /// 获取默认收费设定
        /// </summary>
        /// <param name="appid">账号id</param>
        /// <param name="buildid">楼房id</param>
        /// <param name="roomid">出租屋id</param>
        /// <param name="classid">收费项目id</param>
        /// <returns></returns>
        public static calcconfig GetRoomDefaultCalcConfig(long roomid, long classid)
        {
            using (Entities db = new Entities())
            {
                //获取出租屋信息
                rooms room = db.rooms.FirstOrDefault(c => c.roomid == roomid && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
                if (room == null)
                {
                    throw new DBException(ErrorCode.DB_FINANCECLASS_RECORD_NOEXIST, "指定出租屋信息不存在！");
                }
                long appid = room.appid;
                long buildid = room.buildid;

                return GetRoomDefaultCalcConfig(appid, buildid, roomid, classid);
            }
        }
        
        #endregion


        #region 抄表试算相关

        /// <summary>
        /// 根据房间ID列表取试算表
        /// </summary>
        /// <param name="roomidlist">房间列表</param>
        /// <param name="year">试算年份</param>
        /// <param name="month">试算月份</param>
        /// <param name="classid">试算类目，0代表试算所有类目</param>
        /// <returns></returns>
        public static IList<calcvaluelog> GetCalcValueLogs(long[] roomidlist, short year, short month, long classid = 0)
        {
            IList<calcvaluelog> calcvalues = new List<calcvaluelog>();

            using (Entities db = new Entities())
            {
                calcvalues = (from g in db.calcvaluelog
                              where g.year == year && g.month == month && roomidlist.Any(c => c == g.roomid) &&
                              (classid == 0 || g.classid == classid) &&
                              g.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK
                              select g).ToList();
            }

            return calcvalues;
        }

        /// <summary>
        /// 获取抄表(试算)
        /// </summary>
        /// <param name="roomid">出租屋代码</param>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="classid">计费项目ID</param>
        /// <returns></returns>
        public static calcvaluelog GetCalcValueLogs(long roomid, short year, short month, long classid)
        {
            calcvaluelog calcvaluelog = null;

            using (Entities db = new Entities())
            {
                calcvaluelog = (from g in db.calcvaluelog
                                where g.year == year && g.month == month && roomid == g.roomid && classid == g.classid &&
                                g.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK
                                orderby g.calclogid descending
                                select g).FirstOrDefault();
            }

            return calcvaluelog;
        }

        /// <summary>
        /// 获取最近一次抄表(试算)
        /// </summary>
        /// <param name="roomid">出租屋代码</param>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="classid">计费项目ID</param>
        /// <returns></returns>
        public static calcvaluelog GetLastCalcValueLogs(long roomid, short year, short month, long classid)
        {
            calcvaluelog calcvaluelog = null;

            using (Entities db = new Entities())
            {
                DateTime lastday = new DateTime(year, month, 1).AddMonths(-1);
                calcvaluelog = (from g in db.calcvaluelog
                                where g.year == year && g.month == month && roomid == g.roomid && classid == g.classid &&
                                g.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK
                                orderby g.calclogid descending
                                select g).FirstOrDefault();
            }

            return calcvaluelog;
        }

        /// <summary>
        /// 插入试算记录表
        /// </summary>
        /// <param name="valuelog"></param>
        /// <returns></returns>
        public static bool InsertCalcValueLog(calcvaluelog valuelog)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    //新增?
                    if (valuelog.calclogid == 0)
                    {
                        valuelog = db.calcvaluelog.Add(valuelog);
                        db.SaveChanges();

                        return true;
                    }
                    else return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 更改试算表(更新金额)
        /// </summary>
        /// <param name="calclogid">需修改记录的ID</param>
        /// <param name="money">最终算出来的金额</param>
        /// <returns></returns>
        public static bool UpdateCalcValueLog(long calclogid, decimal money)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    var updateitem = db.calcvaluelog.FirstOrDefault(c => c.calclogid == calclogid);
                    if (updateitem == null) return false;
                    //更新
                    updateitem.money = money;
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 更改试算表(更新所有数据)
        /// </summary>
        /// <param name="calclogid">需修改记录的ID</param>
        /// <param name="money">最终算出来的金额</param>
        /// <returns></returns>
        public static bool UpdateCalcValueLog(long calclogid, decimal value_init, decimal value_end, decimal price, string customprice, decimal money)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    var updateitem = db.calcvaluelog.FirstOrDefault(c => c.calclogid == calclogid);
                    if (updateitem == null) return false;
                    //更新
                    updateitem.value_init = value_init;
                    updateitem.value_end = value_end;
                    updateitem.value_real = value_end - value_init;
                    updateitem.price = price;
                    updateitem.customprice = customprice;
                    updateitem.money = money;
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion


        #region 月租记录相关

        /// <summary>
        /// 更新月度租金表
        /// </summary>
        /// <param name="roomid">需修改记录的ID</param>
        /// <param name="money">最终算出来的金额</param>
        /// <returns></returns>
        public static bool InsertUpdateRoomRentMonth(long roomid, short year, short month, decimal money)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    rentmonth myitem = db.rentmonth.FirstOrDefault(c => c.roomid == roomid && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
                    if (myitem == null) //没有则新增
                    {
                        var room = db.rooms.FirstOrDefault(c => c.roomid == roomid && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
                        if (room == null) return false;

                        myitem = new rentmonth()
                        {
                            appid = room.appid,
                            buildid = room.buildid,
                            roomid = room.roomid,
                            year = year,
                            month = month,
                            payment_money = money,
                            payment_status = false,
                            calc_time = DateTime.Now,
                            STATUS = ConstantDefine.DB_COMM_COL_STATUS_OK
                        };

                        db.rentmonth.Add(myitem);
                    }
                    else
                    {
                        myitem.payment_money = money;
                    }

                    //更新
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static rentmonth GetRoomRentMoonth(long roomid, short year, short month )
        {
            using (Entities db = new Entities())
            {
                return db.rentmonth.FirstOrDefault(c => c.roomid == roomid && c.year == year && c.month == month
                    && c.STATUS == ConstantDefine.DB_COMM_COL_STATUS_OK);
            }
        }

        #endregion


        #region 资金日志相关
        #endregion
    }
}
