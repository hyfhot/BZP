using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Lib.CalcFun;
using DAL;
using COMM;

namespace BLL
{
    /// <summary>
    /// 价格定义级别 
    /// 
    /// </summary>
    public enum PriceLevel
    {
        /// <summary>
        /// 针对全局的价格
        /// </summary>
        DefaultLevel = 0,
        /// <summary>
        /// 针对全局的价格
        /// </summary>
        AppLevel = 1,
        /// <summary>
        /// 针对楼房的价格
        /// </summary>
        BuildLevel = 2,
        /// <summary>
        /// 针对出租屋的价格
        /// </summary>
        RoomLevel = 3,
    };

    public class Finance
    {

        /// <summary>
        /// 计算租金
        /// </summary>
        /// <param name="session">用户登录信息</param>
        /// <param name="year">计算租金的年份</param>
        /// <param name="month">计算租金的月份</param>
        /// <param name="buildid">需计算楼房</param>
        /// <param name="roomid">需计算的出租屋</param>
        /// <returns></returns>
        public static bool Calc(Session session, short year, short month, long buildid = 0, long roomid = 0) 
        {
            //判断是否登陆
            if(session == null || session.AppID <= 0)
            {
                throw new AuthException(ErrorCode.AU_LOGIN_NO, "未登录用户无法计算租金,请登录后再重试！");

            }
            //生产合同截至日期(月份的最后一天)
            DateTime calctime = DateTime.Parse(string.Format("{0}/{1}/1", year, month)).AddMonths(1).AddDays(-1);

            //计算结果存储列表
            IList<rentmonth> rentmonthlist = new List<rentmonth>();
            IList<calcvaluelog> calcvalueloglist = new List<calcvaluelog>();

            #region 加载待计算数据

            //加载当前正在出租的合同
            var calcrents = DAL.RentManager.GetRentingContracts(calctime, session.AppID, buildid, roomid);
            //根据租赁合同获取房间ID集合(去重)
            var roomidlist = (from g in calcrents group g by g.roomid into p select p.Key).ToArray();
            //加载当前正在出租的房间
            var rentrooms = DAL.RentManager.GetRoomsById(roomidlist);
            //加载房间的收费设定
            var calcconfigs = DAL.FinanceManager.GetRoomCalcConfigs(roomidlist);
            //加载试算表
            var calcvaluelog = FinanceManager.GetCalcValueLogs(roomidlist, year, month);
            //加载收费项目设定
            var financeclasslist = DAL.FinanceManager.GetFinanceClassList(session.AppID);

            #endregion

            #region 计算每个房间的租金

            foreach (long myroomid in roomidlist)
            {
                //取房间的基础数据
                rooms room = rentrooms.FirstOrDefault(c => c.roomid == myroomid);
                if (room == null)
                {
                    //写数据错误日志
                    continue;
                }
                //取合约数据
                rent rent = calcrents.FirstOrDefault(c => c.roomid == myroomid);
                if (rent == null)
                {
                    //写数据错误日志
                    continue;
                }

                #region 逐一计算每个收费项目
                foreach (DAL.financeclass myclass in financeclasslist)
                {
                    //有针对出租屋的计算定义？
                    var mycalcconfig = calcconfigs.FirstOrDefault(c => c.roomid == myroomid && c.classid == myclass.classid);
                    if (mycalcconfig == null)
                    { 
                        //有针对楼房的计算定义?
                        mycalcconfig = calcconfigs.FirstOrDefault(c => c.buildid == room.buildid && c.roomid == 0 && c.classid == myclass.classid);
                    }
                    if (mycalcconfig == null)
                    {
                        //有针对账号的计算定义?
                        mycalcconfig = calcconfigs.FirstOrDefault(c => c.appid == room.appid && c.buildid == 0 && c.roomid == 0 && c.classid == myclass.classid);
                    }
                    if (mycalcconfig == null)
                    {
                        //没有计算定义?使用全局默认设置
                        mycalcconfig = new calcconfig()
                        {
                            configid = 0,
                            appid = room.appid,
                            buildid = room.buildid,
                            roomid = room.roomid,
                            classid = myclass.classid,
                            price = myclass.price,
                            customprice = myclass.customprice,
                            createuserid = session.UserID,
                            STATUS = ConstantDefine.DB_COMM_COL_STATUS_OK
                        };
                    }

                    //有试算表记录
                    var mycalcvaluelog = calcvaluelog.FirstOrDefault(c => c.roomid == myroomid && c.classid == myclass.classid);
                    //没有则新增一条(如果是按量计费项目，代表没有抄表)
                    if (mycalcvaluelog == null)
                    {
                        calcvaluelog lastvaluelog = DAL.FinanceManager.GetLastCalcValueLogs(myroomid, year, month, myclass.classid);
                        DateTime beginday = new DateTime(year, month, 1);
                        mycalcvaluelog = new calcvaluelog() 
                        {
                            appid = room.appid,
                            buildid = room.buildid,
                            roomid = room.roomid,
                            classid = myclass.classid,
                            year = year,
                            month = month,
                            beginday = beginday,
                            endday = beginday.AddMonths(1).AddDays(-1),
                            value_init = lastvaluelog != null ? lastvaluelog.value_end : 0,
                            value_end = lastvaluelog != null ? lastvaluelog.value_end : 0,
                            value_real = 0,
                            price = mycalcconfig.price,
                            customprice = mycalcconfig.customprice,
                            money = 0,
                            loger_id = session.UserID,
                            loger_name = session.UserName,
                            logtime = DateTime.Now,
                            STATUS = ConstantDefine.DB_COMM_COL_STATUS_OK
                        };
                    }

                    //根据类型获取租金计算对象
                    ICalculator calc = CalcFactory.GetCalculator(myclass.calctype);

                    //计算费用
                    decimal money = calc.Calc(new CalcValue(mycalcconfig.price,mycalcconfig.customprice, mycalcvaluelog.value_real.Value));
                    
                    mycalcvaluelog.money = money;
                    //插入更新试算记录表
                    bool isok = false;
                    if (mycalcvaluelog.calclogid == 0) isok = FinanceManager.InsertCalcValueLog(mycalcvaluelog);
                    else isok = FinanceManager.UpdateCalcValueLog(mycalcvaluelog.calclogid,money);
                    
                    if(!isok){
                        //写入出错日志
                    }else // 更新成功则加入到列表，准备统计月度金额
                    {
                        calcvalueloglist.Add(mycalcvaluelog);
                    }
                }

                #endregion
            }

            #endregion

            //按房间统计金额
            var roommonth = (from g in calcvalueloglist group g by g.roomid into p select new {
                roomid = p.Key, 
                money = p.Sum(c => c.money)}).ToList();

            foreach(var roommonthitem in roommonth)
            {
                if (!DAL.FinanceManager.InsertUpdateRoomRentMonth(roommonthitem.roomid,year,month, roommonthitem.money))
                {
                    //写错误日志
                }
            }

            return false;
        }
    }
}
