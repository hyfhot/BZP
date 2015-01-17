using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Lib.CalcFun
{
    /// <summary>
    /// 费用计算对象工厂
    /// </summary>
    public class CalcFactory
    {
        public static IList<ICalculator> Calculators = new List<ICalculator>();

        /// <summary>
        /// 注册接口
        /// </summary>
        /// <param name="calc"></param>
        /// <returns></returns>
        public static int RegisterCalculator(ICalculator calc)
        {
            for (int i = 0; i < Calculators.Count; i++)
            {
                var calcitem = Calculators[i];
                if (calcitem.CalcTypeID == calc.CalcTypeID)
                {
                    return i;
                }
            }
            Calculators.Add(calc);

            return Calculators.IndexOf(calc);
        }

        /// <summary>
        /// 根据接口类型获取费用计算对象
        /// </summary>
        /// <param name="calctype"></param>
        /// <returns></returns>
        public static ICalculator GetCalculator(short calctype)
        {
            for (int i = 0; i < Calculators.Count; i++)
            {
                var calcitem = Calculators[i];
                if (calcitem.CalcTypeID == calctype)
                {
                    return calcitem;
                }
            }

            return null;
        }

        /// <summary>
        /// 静态构造函数,用于注册接口
        /// </summary>
        static CalcFactory()
        {
            CalcFactory.RegisterCalculator(new MonthStaticCalculator());
            CalcFactory.RegisterCalculator(new MonthAmountCalculator());
            CalcFactory.RegisterCalculator(new MonthStepAmountCalculator());
            CalcFactory.RegisterCalculator(new ManualCalculator());
        }
    }
}
