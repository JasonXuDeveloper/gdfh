//
// Player.cs
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
using System.Numerics;
using Game.Logic;
using LitJson;
using JEngine.Core;
using UnityEngine;
using JEngine.AntiCheat;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Game.Data
{
    public class Player
    {
        [JsonIgnore] public BigInteger Coin => PropLogic.instance.m_propData.GetData("coin").amount;
        public List<Property> CanBuildProperties = new List<Property>();
        public List<Property> BuiltProperties = new List<Property>();

        public List<string> ClaimedCdks = new List<string>();

        public string name;
        public string id;
        public string password;
        public int server = 1;

        //保安玩法
        public JInt security;

        //收益百分比加成（整数，需要除以100）
        [JsonIgnore]public JInt incomeAddition
        {
            get
            {
                JInt x = 100;
                x += security * 2;
                return x;
            }
        }

        [JsonIgnore]
        public BigInteger TotalAvgIncome
        {
            get
            {
                var ps = BuiltProperties;
                BigInteger a = 0;
                for (int i = 0, cnt = ps.Count; i < cnt; i++)
                {
                    a += ps[i].Income / (int)ps[i].duration;
                }
                return a;
            }
        }

        [JsonIgnore] public Dictionary<int, int> PropAmount = new Dictionary<int, int>();

        public static Player Instance()
        {
            if (instance != null) return instance;
            if (JSaver.HasData("PLAYER_DATA"))
            {
                var encryptKey = InitJEngine.Instance.key;
                var result = PlayerPrefs.GetString("PLAYER_DATA");
                result = CryptoHelper.DecryptStr(result, encryptKey);
                //Debug.Log(result);
                instance = StringifyHelper.JSONDeSerliaze<Player>(result);
            }
            else
            {
                instance = new Player();
                Save(instance);
            }
            if (HotUpdateScripts.GlobalObj.IsDebug)
            {
                instance.server = 0;
            }

            return instance;
        }

        public static void CleanData()
        {
            int index = 0;
            foreach (var prop in instance.BuiltProperties)
            {
                if (prop.level > (prop.grade + 1) * 100)
                {
                    prop.level = 100 * (prop.grade + 1);
                }
                if (prop.employee > (prop.grade + 1) * 100)
                {
                    prop.employee = 100 * (prop.grade + 1);
                }
                if (prop.grade >= Property.GradeBenefit.Length)
                {
                    prop.grade = Property.GradeBenefit.Length - 1;
                    prop.employee = 1;
                    prop.level = 1;
                }
                prop.uid = index;
                index++;
            }
            foreach (var p in instance.CanBuildProperties)
            {
                Player.Instance().BuiltProperties.Add(new Property
                {
                    id = p.id,
                    uid = Player.Instance().BuiltProperties.Count
                });
            }
            instance.CanBuildProperties = new List<Property>();
            Player.Save();
        }

        public static void CleanName()
        {
            string input = Player.Instance().name;
            string pattern = @"[u4e00-u9fa5]";
            for (int i = 0; i < input.Length - 4; i++)
            {
                if (input[i] == 'u')
                {
                    try
                    {
                        var s = input.Substring(i, 5);
                        if (Regex.IsMatch(s, pattern))
                        {
                            var r = Regex.Unescape(@"\" + s);
                            Player.Instance().name = input.Replace(s, r);
                        }
                    }
                    catch { }
                }
            }
            Player.Save();
        }

        public static async void AutoMerge()
        {
            var bps = instance.BuiltProperties;
            var cfgs = PropertyCfg.m_cfg;
            int cnt = cfgs.Count;
            for (int i = 0; i < cnt; i++)
            {
                for(int grade = 0; grade < 2; grade++)
                {
                    var c = bps.FindAll(p => p.id == cfgs[i].id && p.grade == grade);
                    while (c.Count >= 5)
                    {
                        c[0].grade++;
                        for (int x = 1; x < 5; x++)
                        {
                            instance.BuiltProperties.Remove(c[x]);
                        }
                        Save();
                        Util.Info.D($"免费自动合成了「{c[0].grade}阶{c[0].name}」");
                        bps = instance.BuiltProperties;
                        c = bps.FindAll(p => p.id == cfgs[i].id && p.grade == grade);
                        TaskLogic.instance.AddProgressToSameTask("upstage_property", 1);
                        await Task.Delay(1);
                    }
                }
            }
            View.PropertyPanel.UpdateIncomeText();
            View.PropertyPanel.ShowProperty();
        }

        public static void LogOut()
        {
            instance = null;
        }

        public static void ResetPropertyUid()
        {
            instance.PropAmount = new Dictionary<int, int>();
            for (int i=0;i< instance.BuiltProperties.Count; i++)
            {
                int index = i;
                instance.BuiltProperties[index].uid = index;
                //Debug.Log($"{ instance.BuiltProperties[index].name}:{index}");
                if (instance.PropAmount.ContainsKey(instance.BuiltProperties[index].id))
                {
                    instance.PropAmount[instance.BuiltProperties[index].id]++;
                }
                else
                {
                    instance.PropAmount[instance.BuiltProperties[index].id] = 1;
                }
            }
        }

        private static Player instance;

        public static void Save(Player p = null)
        {
            if (p == null) p = Instance();
            JSaver.SaveAsJSON("PLAYER_DATA", p);
            instance = p;
        }
    }
}