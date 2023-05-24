using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Common.Helpers
{
    /// <summary>
    /// 小红书算法类
    /// </summary>
    public class AlgorithmHelper
    {
        /// <summary>
        /// 小红书SearchId算法实现
        /// </summary>
        /// <returns></returns>
        public static string GetSearchId()
        {
            BigInteger t = new BigInteger(DateTime.Now.Ticks);
            BigInteger r = new BigInteger(Math.Ceiling(2147483646 * new Random().NextDouble()));
            t <<= 64;
            t += r;
            string result = ToBase36String(t);
            return result;
        }
        public static string ToBase36String(BigInteger number)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            string result = string.Empty;
            while (number > 0)
            {
                BigInteger remainder = number % 36;
                number /= 36;
                result = chars[(int)remainder] + result;
            }
            return result;
        }
    }
}
