using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Helper
{
    public class String
    {
        /// <summary>
        /// 获取指定长度的随机字符串
        /// </summary>
        /// <param name="len"></param>
        public static string GetRndString(int len)
        {
            string result = "";
            string str = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";//62个字符
            Random r = new Random();

            //生成一个8位长的随机字符，具体长度可以自己更改
            for (int i = 0; i < len; i++)
            {
                int m = r.Next(0, 62);//这里下界是0，随机数可以取到，上界应该是62，因为随机数取不到上界，也就是最大61，符合我们的题意
                string s = str.Substring(m, 1);
                result += s;
            }
            return result;
        }
    }
}
