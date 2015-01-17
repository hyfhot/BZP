using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Lib.CalcFun
{
    /// <summary>
    /// 费用计算类接口
    /// </summary>
    public interface ICalculator
    {
        /// <summary>
        /// 计算类型ID，用于区分不同的计算规则
        /// </summary>
        short CalcTypeID  { get;}

        /// <summary>
        /// 计算类型的通俗名称，便于理解
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 计算类型的说明，解释配置方法和注意事项
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 计算规则的验证框架
        /// </summary>
        string VerifySchema { get; }

        /// <summary>
        /// 计算费用
        /// </summary>
        /// <param name="calcvalue">计算参数，包含用量，单价等信息</param>
        /// <returns></returns>
        decimal Calc(CalcValue calcvalue);

        /// <summary>
        /// 校验规则，高级算法调用，如：阶梯计费
        /// </summary>
        /// <param name="customprice"></param>
        /// <returns></returns>
        bool Verify(string customprice);
    }
}
