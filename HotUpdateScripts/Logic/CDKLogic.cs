//
// CDKLogic.cs
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
using System.Threading.Tasks;
using Game.Data;
using Game.Util;

namespace Game.Logic
{
    public class CDKLogic
    {
        public static Dictionary<string, List<Award>> cdks = new Dictionary<string, List<Award>>()
        {
            {"rmxy666",
                new List<Award>(){
                    new Award()
                    {
                        id="cert",
                        amount = 10
                    },
                    new Award()
                    {
                        id="coin_plus",
                        amount = 30
                    }
                }
            },
            {"gdfh666",
                new List<Award>(){
                    new Award()
                    {
                        id="cert",
                        amount = 3
                    },
                    new Award()
                    {
                        id="coin_plus",
                        amount = 30
                    }
                }
            },
            {"gdfh888",
                new List<Award>(){
                    new Award()
                    {
                        id="cert",
                        amount = 3000
                    }
                }
            },
            {"bug1",
                new List<Award>(){
                    new Award()
                    {
                        id="cert",
                        amount = 99
                    },
                    new Award()
                    {
                        id="coin_plus",
                        amount = 169
                    }
                }
            },
            {"bug2",
                new List<Award>(){
                    new Award()
                    {
                        id="cert",
                        amount = 99
                    },
                    new Award()
                    {
                        id="coin_plus",
                        amount = 169
                    }
                }
            },
            {"dberr1",
                new List<Award>(){
                    new Award()
                    {
                        id="cert",
                        amount = 199
                    },
                    new Award()
                    {
                        id="coin_plus",
                        amount = 369
                    }
                }
            },
            {"bug3",
                new List<Award>(){
                    new Award()
                    {
                        id="cert",
                        amount = 99
                    },
                    new Award()
                    {
                        id="coin_plus",
                        amount = 169
                    }
                }
            },
        };

        public static async void ClaimCDK(string key)
        {
            if (Player.Instance().ClaimedCdks.Contains(key))
            {
                Info.D("已经领取过啦~");
                return;
            }
            if (cdks.TryGetValue(key, out var aw))
            {
                Player.Instance().ClaimedCdks.Add(key);
                Player.Save();
                for(int i = 0; i < aw.Count; i++)
                {
                    var a = aw[i];
                    PropLogic.instance.AddProp(a.id, a.amount);
                    Info.D($"获得「{PropCfg.instance.GetProp(a.id).name}」 * {a.amount}");
                    await Task.Delay(300);
                }
            }
            else
            {
                Info.D("无效CDK");
            }
        }
    }
}
