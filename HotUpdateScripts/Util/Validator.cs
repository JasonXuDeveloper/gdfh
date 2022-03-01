//
// Validator.cs
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
    public class Validator
    {
        public static bool Validate<T>(T val, Func<T, bool> condition)
        {
            return condition(val);
        }

        public static bool Validate(string val, int minLength = -1, int maxLength = -1, bool nullable = false,
            string nullableStr = "字符串不可为空", string minLengthStr = "字符串长度至少{0}位", string maxLengthStr = "字符串长度最多{0}位")
        {
            Func<string, bool> cond = (str) =>
             {
                 if (str == null && !nullable)
                 {
                     Info.D(nullableStr);
                     return false;
                 }
                 var l = str.Length;
                 if (l < minLength)
                 {
                     Info.D(string.Format(minLengthStr, minLength));
                     return false;
                 }
                 if (l > maxLength && maxLength >= 0)
                 {
                     Info.D(string.Format(maxLengthStr, maxLength));
                     return false;
                 }
                 return true;
             };
            return Validate(val, cond);
        }
    }
}
