//
// Time.cs
//
// Author:
//       jason <jasonxudeveloper@gmail.com>
//
// Copyright (c) 2021 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
namespace Game.Util
{
    public static class Time
    {
        public static bool IsGreaterDay(this DateTime dt)
        {
            var now = DateTime.Now;
            return now > dt && now.Day != dt.Day;
        }

        public static bool IsGreaterDay(this long timeStamp)
        {
            var dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            dt = dt.AddSeconds(timeStamp);
            return IsGreaterDay(dt);
        }

        public static string TimeToTomorrow()
        {
            var now = DateTime.Now;
            DateTime ret = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(1);
            return $"{(ret - now).Hours:00}:{(ret - now).Minutes:00}:{(ret - now).Seconds:00}";
        }

        public static long NowTimeStamp()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (long)(DateTime.Now - startTime).TotalSeconds;
        }

        public static DateTime NowTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 判断是否在环状数据的指定区间内
        /// </summary>
        /// <param name="ringSize">环的大小</param>
        /// <param name="first">起始位置</param>
        /// <param name="last">终止位置</param>
        /// <param name="num">需要判断的数据</param>
        /// <returns>是否在区间内</returns>
        public static bool InTime(double ringSize, double first, double last, double num)
        {
            first %= ringSize;
            last %= ringSize;
            double distance = (last + ringSize - first) % ringSize;
            return (num + ringSize - first) % ringSize <= distance;
        }
    }
}
