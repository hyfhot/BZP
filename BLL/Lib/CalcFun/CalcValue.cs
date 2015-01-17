using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Lib.CalcFun
{
    public class CalcValue
    {
        /// <summary>
        /// 当前用量，固定收取费用和手动收取方式的设置为1
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 单价，固定收取费用和手动收取方式的设置为每月需要收取的费用,阶梯计费设置为0
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 复杂单价，如：阶梯计费的分段方式
        /// 例子：[{"step":1440,"price":1.5},{"step":2440,"price":2.5},{"step":88888888,"price":2.5}]
        /// </summary>
        public string CustomPrice { get; set; }

        public CalcValue(decimal price)
        {
            SetValue(1,price,"");
        }

        public CalcValue(decimal price,decimal amount)
        {
            SetValue(amount, price, "");
        }

        private void SetValue(decimal amount, decimal price, string customprice)
        {
            Amount = amount;
            Price = price;
            CustomPrice = customprice;
        }

        public CalcValue(string customprice, decimal amount)
        {
            SetValue(amount, 0, customprice);
        }

        public CalcValue(decimal price, string customprice,decimal amount)
        {
            SetValue(amount, price, customprice);
        }
    }
}
