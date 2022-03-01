using System;
using System.Numerics;
using System.Text;

namespace Game.Util
{
    /// <summary>
    /// 自定义货币类型
    /// </summary>
    public class Unit
    {
        //单位
        private static string[] units = new string[]{ "","k","m","b","t","e15","e18","e21","e24","ee","ff",
            "gg" ,"hh","ii","jj","kk","ll","mm","nn","oo","pp","qq","rr","ss","tt",
            "uu","vv","ww","xx","yy","zz"};

        /// <summary>
        /// 显示字符串
        /// </summary>
        /// <returns></returns>
        public static string GetString(BigInteger i)
        {
            bool neg = false;
            if (i < 0)
            {
                i = 0 - i;
                neg = true;
            }
            if (i < 1000) return neg ? (0 - i).ToString() : i.ToString();

            //之所以这么写是为了低GC，毕竟这个方法常用
            var len = i.ToString().Length - 1;
            /*
             * k 是 4~6位
             * m 是 7~9位
             * len = 6
             * len - 1 = 5
             * 5 - 5%3 = 3
             * 3/3 = 1
             * 
             * len = 8
             * len - 1 = 7
             * 7 - 7%3 = 6
             * 6/3 = 2
             */
            var unitIndex = (int)Math.Floor((len-len%3) / 3d);
            //保留两位小数
            //10k = 10,000
            //100k = 100,000
            //1000k = 1m = 1,000,000
            // a.d1d2 unit
            int a = (len + 1) - unitIndex * 3;
            var str = i.ToString();
            var d1 = str[a];
            var d2 = str[a+1];
            var z = '0';
            if(d1 == d2 && d1==z)
            {
                return new StringBuilder(neg?"-":"").Append(str.Substring(0,a)).Append(units[unitIndex]).ToString();
            }
            else if(d2 == z)
            {
                return new StringBuilder(neg ? "-" : "").Append(str.Substring(0, a)).Append('.').Append(d1).Append(units[unitIndex]).ToString();
            }
            else
            {
                return new StringBuilder(neg ? "-" : "").Append(str.Substring(0, a)).Append('.').Append(d1).Append(d2).Append(units[unitIndex]).ToString();
            }
        }

        public static string GetString(object s)
        {
            return GetString(s.ToString());
        }

        public static string GetString(string s)
        {
            return GetString(BigInteger.Parse(s));
        }

        public static string GetString(int s)
        {
            return GetString(new BigInteger(s));
        }

        public static string GetString(uint s)
        {
            return GetString(new BigInteger(s));
        }

        public static string GetString(long s)
        {
            return GetString(new BigInteger(s));
        }

        public static string GetString(ulong s)
        {
            return GetString(new BigInteger(s));
        }

        public static string GetString(double s)
        {
            return GetString(new BigInteger(s));
        }

        public static string GetString(float s)
        {
            return GetString(new BigInteger(s));
        }
    }
}
