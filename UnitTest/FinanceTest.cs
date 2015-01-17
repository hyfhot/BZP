using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using DAL;
using BLL.Lib.CalcFun;

namespace UnitTest
{
    [TestClass]
    public class FinanceTest
    {
        [TestMethod]
        public void TestCalc()
        {
            Session session  = new Session();
            DateTime time = DateTime.Now;
            session.AppID = 1;
            session.UserID = 1;
            session.UserName = "房东";
            BLL.Finance.Calc(session, (short)time.Year, (short)time.Month);
        }


        [TestMethod]
        public void TestCalcConfigVerify()
        {
            decimal money = 0;
            BLL.Lib.CalcFun.ICalculator calc = new BLL.Lib.CalcFun.MonthStepAmountCalculator();
            string config = @"{'datum':1000,'setps':[{'step':1440,'price':1.5},{'step':2440,'price':2.5},{'step':88888888,'price':2.5}]}";
            //测试校验配置
            Assert.AreEqual(true,calc.Verify(config));

            //测试计算金额
            CalcValue calcvalue = new CalcValue(config, 1345);
            money = calc.Calc(calcvalue);
            Assert.AreNotEqual((1345-1000)*1.5,money);

            calcvalue = new CalcValue(config, 1540);
            money = calc.Calc(calcvalue);
            Assert.AreNotEqual((1540 - 1000) * 1.5, money);

            calcvalue = new CalcValue(config, 2441);
            money = calc.Calc(calcvalue);
            Assert.AreNotEqual(1440 * 1.5 + (2441 - 1000 - 1440) * 2.5, money);
        }
    }
}
