//
// Extensions.cs
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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Util
{
    public static class Extensions
    {
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout, Action callback = null)
        {
            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                }
                else
                {
                    callback?.Invoke();
                    JEngine.Core.Log.PrintError($"{nameof(TimeoutAfter)}: The operation has timed out after {timeout.Seconds} seconds");
                }
                return await task;  // Very important in order to propagate exceptions
            }
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static Color ToColor(this string hex)
        {
            ColorUtility.TryParseHtmlString(hex, out var c);
            return c;
        }

        private static Dictionary<Transform, Text> t_cache = new Dictionary<Transform, Text>();
        public static Text txt(this Transform x)
        {
            if (t_cache.TryGetValue(x, out var ret))
            {
                return ret;
            }
            ret = x.GetComponent<Text>();
            if (ret == null)
            {
                ret = x.gameObject.AddComponent<Text>();
            }
            t_cache[x] = ret;
            return ret;
        }
        public static Text txt(this Transform x,string name)
        {
            return x.Find(name).txt();
        }

        private static Dictionary<Transform, Outline> o_cache = new Dictionary<Transform, Outline>();
        public static Outline outline(this Transform x)
        {
            if (o_cache.TryGetValue(x, out var ret))
            {
                return ret;
            }
            ret = x.GetComponent<Outline>();
            if (ret == null)
            {
                ret = x.gameObject.AddComponent<Outline>();
            }
            o_cache[x] = ret;
            return ret;
        }
        public static Outline outline(this Transform x, string name)
        {
            return x.Find(name).outline();
        }

        private static Dictionary<Transform, Button> b_cache = new Dictionary<Transform, Button>();
        public static Button btn(this Transform x)
        {
            if (b_cache.TryGetValue(x, out var ret))
            {
                return ret;
            }
            ret = x.GetComponent<Button>();
            if (ret == null)
            {
                ret = x.gameObject.AddComponent<Button>();
            }
            b_cache[x] = ret;
            return ret;
        }
        public static Button btn(this Transform x, string name)
        {
            return x.Find(name).btn();
        }


        private static Dictionary<Transform, Image> i_cache = new Dictionary<Transform, Image>();
        public static Image img(this Transform x)
        {
            if (i_cache.TryGetValue(x, out var ret))
            {
                return ret;
            }
            ret = x.GetComponent<Image>();
            if (ret == null)
            {
                ret = x.gameObject.AddComponent<Image>();
            }
            i_cache[x] = ret;
            return ret;
        }
        public static Image img(this Transform x, string name)
        {
            return x.Find(name).img();
        }
    }
}
