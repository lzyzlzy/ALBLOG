using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility.Extension
{
    public static class DoubleExtension
    {
        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToRound(this double value)
        {
            return Math.Round(value);
        }

        /// <summary>
        /// 取n位小数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToRound(this double value,int n)
        {
            return Math.Round(value, n);
        }
    }
}
