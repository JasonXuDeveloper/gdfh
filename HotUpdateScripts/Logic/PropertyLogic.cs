//
// PropertyLogic.cs
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
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Data;
using Game.Util;
using Game.View;
using UnityEngine;
using System.Numerics;
using Time = Game.Util.Time;

namespace Game.Logic
{
    public class PropertyLogic
    {
        public long lastUpgradeTime;
        public long lastBuildTime;
        public long lastEmployTime;
        public int upgradeAmount;
        public int employAmount;
        public int buildAmount;

        public int MaxLevel(Property p)
        {
            return (p.grade + 1) * 100;
        }
        public int MaxEmploy(Property p)
        {
            return (p.grade + 1) * 100;
        }

        public bool ReachMaxEmploy(Property p)
        {
            return p.employee >= (p.grade + 1) * 100; ;
        }

        public bool ReachMaxLevel(Property p)
        {
            return p.level >= (p.grade + 1) * 100; ;
        }

        public float GetLevelProgress(Property p)
        {
            return (float)p.level / MaxLevel(p);
        }

        public float GetEmployProgress(Property p)
        {
            return (float)p.employee / MaxEmploy(p);
        }

        public Property GetByUid(int uid)
        {
            return Player.Instance().BuiltProperties.Find(p => p.uid == uid);
        }

        /// <summary>
        /// 某产业平均秒收益
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BigInteger AvgIncome(int id)
        {
            var all = Player.Instance().BuiltProperties.FindAll(p => p.id == id);
            return CalAvg(all);
        }

        /// <summary>
        /// 某类型产业平均秒收益
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BigInteger AvgIncome(PropertyType type)
        {
            var all = Player.Instance().BuiltProperties.FindAll(p => p.type == type);
            return CalAvg(all);
        }

        private BigInteger CalAvg(List<Property> ps)
        {
            BigInteger ret = 0;
            foreach(var p in ps)
            {
                ret += (BigInteger)(p.Income / (BigInteger)p.duration);
            }
            return ret;
        }

        /// <summary>
        /// 以玩家某产业平均收益排序配置产业
        /// </summary>
        /// <returns></returns>
        public List<PropertyCfgItem> OrderCfgItems(bool desc)
        {
            if (desc)
            {
                return PropertyCfg.m_cfg.OrderByDescending(i =>
                CalAvg(Player.Instance().BuiltProperties.FindAll(p => p.id == i.id))).ToList();
            }

            return PropertyCfg.m_cfg.OrderBy(i =>
            CalAvg(Player.Instance().BuiltProperties.FindAll(p => p.id == i.id))).ToList();
        }
        /// <summary>
        /// 以玩家某产业平均收益排序配置产业
        /// </summary>
        /// <returns></returns>
        public List<PropertyCfgItem> OrderCfgItems(PropertyType type, bool desc)
        {
            if (desc)
            {
                return PropertyCfg.m_cfg.FindAll(x => x.type == type).OrderByDescending(i =>
                  CalAvg(Player.Instance().BuiltProperties.FindAll(p => p.id == i.id))).ToList();
            }
            return PropertyCfg.m_cfg.FindAll(x => x.type == type).OrderBy(i =>
              CalAvg(Player.Instance().BuiltProperties.FindAll(p => p.id == i.id))).ToList();
        }
    

        /// <summary>
        /// 玩家拥有某类型产业的总数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int HasAmount(PropertyType type)
        {
            int has = 0;
            List<int> tempInt = new List<int>();
            var all = Player.Instance().BuiltProperties.FindAll(p => p.type == type);
            for (int x = 0; x < all.Count; x++)
            {
                if (!tempInt.Contains(all[x].id))
                {
                    has++;
                    tempInt.Add(all[x].id);
                }
            }
            tempInt.Clear();
            tempInt = null;
            return has;
        }

        /// <summary>
        /// 玩家拥有某产业的总数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int TotalHasAmount(int id)
        {
            var dic = Player.Instance().PropAmount;
            return dic.ContainsKey(id) ? dic[id] : 0;
        }

        public int TotalAmount(PropertyType type)
        {
            int c = 0;
            for(int i=0;i< PropertyCfg.m_cfg.Count; i++)
            {
                if(PropertyCfg.m_cfg[i].type == type)
                {
                    c++;
                }
            }
            return c;
        }

        private string[] unlockColor = new string[] { "#DB0000", "#00DBDB", "#DB00DB" };
        public Color unlockAmountColor(int cur,int max)
        {
            int index = -1;
            if (cur >= max)
            {

                index = 2;
            }
            else
            {
                int step = max / 3;
                if (cur <= step)
                {
                    index = 0;
                }
                else
                {
                    if (cur > step *2)
                    {
                        index = 2;
                    }
                    else
                    {
                        index = 1;
                    }
                }
            }
            return unlockColor[index].ToColor();
        }

        public void HireSecurity()
        {
            int s = Player.Instance().security;
            bool needAds = s > 4;//招募第5~10个保安需要看广告
            var ps = Player.Instance().BuiltProperties;
            BigInteger a = 0;//招募费用
            for (int i = 0, cnt = ps.Count; i < cnt; i++)
            {
                a += ps[i].Income / (int)ps[i].duration;
            }
            for (int i = 0; i < s; i++)
            {
                a *= 3;
            }

            if (s >= 10)
            {
                Info.D("保安已满");
                return;
            }
            if (needAds)
            {
                Ads.instance.PlayAd(() =>
                {
                    TaskLogic.instance.AddProgressToSameTask("ads", 1);
                    Player.Instance().security++;
                    Player.Save();
                    Info.D("招募成功");
                }, () =>
                {
                    Info.D("广告播放失败");
                }, () =>
                {
                    Info.D("暂时没找到合适的保安");
                });
            }
            else
            {
                PropLogic.instance.UseProp("coin", a, (str, b) =>
                {
                    if (b)
                    {
                        Player.Instance().security++;
                        Player.Save();
                        Info.D("招募成功");
                    }
                    else
                    {
                        Info.D("招募失败，金币不足");
                    }
                });
            }
        }

        public async Task Upgrade(Property prop,Action cb)
        {
            await Task.Delay(1);
            if (Time.NowTimeStamp() - lastUpgradeTime < 3)
            {
                if(upgradeAmount > 15)
                {
                    Info.D("操作过于频繁");
                    return;
                }
                else
                {
                    upgradeAmount++;
                }
            }
            else
            {
                lastUpgradeTime = Time.NowTimeStamp();
                upgradeAmount = 0;
            }

            if (prop.level >= (prop.grade + 1) * 100)
            {
                prop.level = 100 * (prop.grade + 1);
                Player.Save();
                Info.D("已满级");
                return;
            }

            bool hasUpdate = false;
            int counts = 0;
            Info.D("升级中");

            int remainCount = PropertyPanel.oneKey ? MaxLevel(prop) - prop.level : 1;

            for (int i = 0; i < remainCount; i++)
            {
                if (ReachMaxLevel(prop))
                {
                    break;
                }

                bool f = false;//用来判断回调好了没，这里懒得用tcs了
                PropLogic.instance.UseProp("coin", prop.UpgradeFee, (x, b) =>
                {
                    f = true;
                    if (b)
                    {
                        hasUpdate = true;
                        prop.level++;
                        counts++;
                    }
                    else
                    {
                        Info.D("升级失败，金币不足");
                    }
                });
                while (!f)
                {
                    await Task.Delay(1);
                }
            }
            if (hasUpdate)
            {
                Info.D("升级成功");
                TaskLogic.instance.AddProgressToSameTask("upgrade_property", counts);
                cb?.Invoke();
            }
            Player.Save();
            await Task.Delay(1);
        }

        public async Task Employ(Property prop, Action cb)
        {
            await Task.Delay(1);
            if (Time.NowTimeStamp() - lastEmployTime < 3)
            {
                if (employAmount > 15)
                {
                    Info.D("操作过于频繁");
                    return;
                }
                else
                {
                    employAmount++;
                }
            }
            else
            {
                lastEmployTime = Time.NowTimeStamp();
                employAmount = 0;
            }

            if (prop.employee >= (prop.grade + 1) * 100)
            {
                prop.employee = 100 * (prop.grade + 1);
                Player.Save();
                Info.D("员工已满");
                return;
            }
            
            bool hasUpdate = false;
            int counts = 0;
            Info.D("雇员中");
            int remainCount = PropertyPanel.oneKey ? MaxEmploy(prop) - prop.employee : 1;
            for (int i = 0; i < remainCount; i++)
            {
                if (ReachMaxEmploy(prop))
                {
                    break;
                }

                bool f = false;//用来判断回调好了没，这里懒得用tcs了
                PropLogic.instance.UseProp("coin", prop.EmployFee, (x, b) =>
                {
                    f = true;
                    if (b)
                    {
                        hasUpdate = true;
                        prop.employee++;
                        counts++;
                    }
                    else
                    {
                        Info.D("雇员失败，金币不足");
                    }
                });
                while (!f)
                {
                    await Task.Delay(1);
                }
            }

            if (hasUpdate)
            {
                Info.D("雇员成功");
                TaskLogic.instance.AddProgressToSameTask("employ_property", counts);
                cb?.Invoke();
            }
            Player.Save();
            await Task.Delay(1);
        }

        public async Task Upstage(Property prop, Action cb)
        {
            await Task.Delay(1);
            if (prop.grade >= Property.GradeBenefit.Length)
            {
                prop.grade = Property.GradeBenefit.Length - 1;
                prop.level = MaxLevel(prop);
                prop.employee = MaxEmploy(prop);
                Player.Save();
                Info.D("等阶已满");
                return;
            }
            else if (prop.grade >= Property.GradeBenefit.Length - 1)
            {
                Info.D("等阶已满");
                return;
            }

            //两个同等阶满员工满等级产业
            int ml = MaxLevel(prop);
            int me = MaxEmploy(prop);
            if (prop.employee < me)
            {
                Info.D("员工未满");
                return;
            }
            if (prop.level < ml)
            {
                Info.D("等级未满");
                return;
            }
            var res = Player.Instance().BuiltProperties.Find(p => p.uid!=prop.uid&& p.id == prop.id &&
            p.level >= ml && p.employee >= me && p.grade == prop.grade);
            if (res == null)
            {
                Info.D("需要2个同阶满级满员工的相同产业");
                //Debug.Log(prop.uid);
                return;
            }

            bool hasUpdate = false;
            int counts = 0;
            Info.D("进阶中");
            bool f = false;
            PropLogic.instance.UseProp("coin", prop.UpstageFee, (x, b) =>
            {
                f = true;
                if (b)
                {
                    hasUpdate = true;
                    prop.grade++;
                    counts++;
                    Player.Instance().BuiltProperties.RemoveAt(Player.Instance().BuiltProperties.IndexOf(res));
                }
                else
                {
                    Info.D("进阶失败，金币不足");
                }
            });
            while (!f)
            {
                await Task.Delay(1);
            }

            if (hasUpdate)
            {
                Info.D("进阶成功");
                TaskLogic.instance.AddProgressToSameTask("upstage_property", 1);
                cb?.Invoke();
            }
            await Task.Delay(1);
            Player.Save();
        }

        private static PropertyLogic s_instance;
        public static PropertyLogic instance
        {
            get
            {
                if (null == s_instance)
                    s_instance = new PropertyLogic();
                return s_instance;
            }
        }
    }
}
