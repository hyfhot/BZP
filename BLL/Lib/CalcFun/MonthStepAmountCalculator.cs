using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using COMM;

namespace BLL.Lib.CalcFun
{
    /// <summary>
    /// 阶梯定价
    /// </summary>
    class StepPricePair
    {
        public decimal step = 0;
        public decimal price = 0;
    }

    class StepPrcieConfig
    {
        public decimal datum = 0;
        public StepPricePair[] setps = null;
    }

    /// <summary>
    /// 每月按量阶梯计费计算对象
    /// </summary>
    public class MonthStepAmountCalculator : ICalculator
    {
        const short CALC_TYPE = ConstantDefine.DB_CLASS_COL_CALCTYPE_MONTHSTEPAMOUNT;
        const string CALC_NAME = "每月按量阶梯计费";
        const string CALC_DESCRIPTION = "";
        const string CALC_SCHEMA = @"{
                                      'type': 'object',
                                      'properties': {
                                        'datum': {'type': 'number','required':true},
                                        'setps': {
                                          'type': 'array',
                                          'items': {
                                            'type': 'object',
                                            'properties': {
                                              'step': {'type': 'number','required':true},
                                              'price': {'type': 'number','required':true}
                                            }
                                          }
                                        }
                                      }
                                    }";

        /* 配置文件样例
        {"datum":9.8,"setps":[{"step":1440,"price":1.5},{"step":2440,"price":2.5},{"step":88888888,"price":3.5}]}
         * */

        public decimal Calc(CalcValue calcvalue)
        {
            //判断配置是否正确
            if (!Verify(calcvalue.CustomPrice))
            {
                throw new ConfigException(ErrorCode.CF_CALCCONFIG_CUSTOMPRICE_INVALID, string.Format("自定义收费设置错误!设置【{0}】不符合规范【{1}】", calcvalue.CustomPrice,this.VerifySchema));
            }

            //序列化配置
            StepPrcieConfig configs = JsonConvert.DeserializeObject<StepPrcieConfig>(calcvalue.CustomPrice);
            if (configs.setps.Count() < 2)
            {
                throw new ConfigException(ErrorCode.CF_CALCCONFIG_CUSTOMPRICE_CANTDESERIALIZE, string.Format("自定义收费设置错误,无法序列化!设置【{0}】应符合规范【{1}】", calcvalue.CustomPrice, this.VerifySchema));
            }

            //判断当前所在阶梯，分别得出单价
            decimal curvalue = calcvalue.Amount;
            decimal realvalue = curvalue - configs.datum ;
            decimal stepvalue = 0, totalmeney = 0;
            //数据设置出错？避免出现负数
            if (realvalue <= 0)
            {
                //写数据配置错误日志!
                return 0;
            }

            for (int i = 0; i < configs.setps.Count(); i++)
            {
                if (realvalue > configs.setps[i].step)
                {
                    totalmeney = totalmeney + (configs.setps[i].step - stepvalue) * configs.setps[i].price;
                    stepvalue = configs.setps[i].step;
                }
                else
                {
                    totalmeney = totalmeney + (realvalue - stepvalue) * configs.setps[i].price;
                    break;
                }
            }

            return totalmeney;
        }

        public bool Verify(string customprice)
        {
            JsonSchema schema = JsonSchema.Parse(CALC_SCHEMA);

            JObject obj = JObject.Parse(customprice);

            return obj.IsValid(schema);
        }

        public short CalcTypeID
        {
            get { return CALC_TYPE; }
        }

        public string Name
        {
            get { return CALC_NAME; }
        }

        public string Description
        {
            get { return CALC_DESCRIPTION; }
        }

        public string VerifySchema
        {
            get { return CALC_SCHEMA; }
        }
    }
}
