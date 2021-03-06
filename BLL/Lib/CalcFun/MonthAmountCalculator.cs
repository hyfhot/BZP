﻿using System;
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
    /// 每月按量计费的计算对象
    /// </summary>
    public class MonthAmountCalculator:ICalculator
    {
        const short CALC_TYPE = ConstantDefine.DB_CLASS_COL_CALCTYPE_MONTHAMOUNT;
        const string CALC_NAME = "每月按量计费";
        const string CALC_DESCRIPTION = "";
        const string CALC_SCHEMA = @"";

        public decimal Calc(CalcValue calcvalue)
        {
            return calcvalue.Amount * calcvalue.Price;
        }

        public bool Verify(string customprice)
        {
            //此功能只适应于高级计费模式
            return true;
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
