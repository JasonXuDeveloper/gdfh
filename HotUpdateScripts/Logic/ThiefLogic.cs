//
// ThiefLogic.cs
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
using Game.View;
using JEngine.Core;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.UI;
using Game.Util;
using Game.Data;
using System.Numerics;

namespace Game.Logic
{
    public class ThiefLogic
    {
        private float durationMin = 90;
        private float durationMax = 260;
        private float thiefDurationMin = 10;
        private float thiefDurationMax = 25;

        public static bool hasAdInfo;

        private List<Property> thiefs = new List<Property>();

        public BindableProperty<bool> hasThief = new BindableProperty<bool>(false);

        public void InitThief()
        {
            var j = new JAction();
            j.Parallel();
            j
            .Delay(UnityEngine.Random.Range(durationMin, durationMax))
            .Do(async () =>
            {
                //来小偷
                int tempPage = PropertyPanel.curPage;
                //小偷产业范围
                Property prop = PropertyPanel.activeProp[Random.Range(0, PropertyPanel.activeProp.Count)];
                Debug.Log($"thief: {prop.uid}");
                if (HasThief(prop) || hasAdInfo)//如果已经有了，就跳过，或者可以看广告
                {
                    j.Execute(true);
                    return;
                }

                hasThief.Value = true;
                thiefs.Add(prop);

                //进小偷
                int tIndex = PropertyPanel.activeProp.IndexOf(prop);
                var thief = PropertyPanel.thiefs[tIndex];
                thief.gameObject.SetActive(true);
                var rect = thief.GetComponent<RectTransform>();
                //小偷持续时间
                float thiefExistTime = Random.Range(thiefDurationMin, thiefDurationMax);
                var anim = thief.GetComponent<Image>().DOFade(0, thiefExistTime).OnComplete(() =>
                {
                    thiefExistTime = 0;
                    thief.gameObject.SetActive(false);
                    thief.GetComponent<Image>().DOFade(1, 0.1f);
                });
                bool caught = false;
                thief.onClick.RemoveAllListeners();
                thief.onClick.AddListener(() =>
                {
                    caught = true;
                    anim.Kill(true);
                    thief.gameObject.SetActive(false);
                    CaughtThief(prop);
                });

                //保安
                if(Player.Instance().security > 0)
                {
                    int c = Random.Range(0, 100);
                    //70%抓到
                    if (c < 70)
                    {
                        caught = true;
                        anim.Kill(true);
                        thief.gameObject.SetActive(false);
                        CaughtThief(prop);
                    }
                    //5%抓到但牺牲
                    else if(c < 75)
                    {
                        Player.Instance().security--;
                        Player.Save();
                        await SettingLogic.SaveToCloud();
                        Info.D("很遗憾，小偷与保安殊死搏斗，保安去世了");
                        await Task.Delay(30);
                        caught = true;
                        anim.Kill(true);
                        thief.gameObject.SetActive(false);
                        CaughtThief(prop);
                    }
                }

                void move()
                {
                    if (rect != null)
                    {
                        var dis = Random.Range(-250, 250);
                        rect.DOAnchorPosX(dis, 0.7f).OnComplete(() =>
                        {
                            if (rect != null)
                            {
                                rect.DOAnchorPosX(-dis, 0.4f).OnComplete(() =>
                                {
                                    if (thiefExistTime > 0)
                                    {
                                        move();
                                    }
                                });
                            }
                        });
                    }
                }
                move();
                while (thiefExistTime > 0 && !caught)
                {
                    //如果换页
                    if (PropertyPanel.curPage != tempPage)
                    {
                        anim.Kill();
                        PropertyPanel.thiefs[tIndex].gameObject.SetActive(false);
                        break;
                    }

                    await Task.Delay(50);
                    thiefExistTime -= 0.05f;
                }

                //没抓到，钱被偷
                if (!caught)
                {
                    //10%保安没抓到
                    if (Player.Instance().security > 0)
                    {
                        Info.D("很抱歉，小偷趁保安打盹，趁虚而入");
                    }
                    UncaughtThief(prop);
                }

                thiefs.Remove(prop);
                if (thiefs.Count == 0)
                {
                    hasThief.Value = false;
                }
                //再次执行
                j.Execute(true);
            })
            .Execute(true);
        }


        public async void UncaughtThief(Property prop)
        {
            Info.D($"很抱歉，「{prop.name}」进了小偷");
            await Task.Delay(300);
            var pn = prop.Income + prop.Income * (BigInteger)Random.Range(0.5f, 3f);
            Info.D($"损失了「{Unit.GetString(pn)}」金币");
            PropLogic.instance.AddProp("coin", -pn);
            await Task.Delay(300);
            //3%概率偷产业
            if (Random.Range(0, 100) < 3 && prop.grade > 0)
            {
                var name = prop.name;
                prop.grade--;
                prop.level = PropertyLogic.instance.MaxLevel(prop) - 100;
                prop.employee = PropertyLogic.instance.MaxEmploy(prop) - 100;
                Player.Save();
                await SettingLogic.SaveToCloud();
                Info.D($"一个不幸的消息：小偷偷使您的「{name}」产业降低了1阶");
                if (PropertyPanel.Active)
                {
                    PropertyPanel.Show();
                }
            }
        }


        public async void CaughtThief(Property prop)
        {
            TaskLogic.instance.AddProgressToSameTask("catch_thief", 1);
            Info.D($"恭喜抓到了「{prop.name}」的小偷");
            await Task.Delay(300);
            var reward = prop.Income + prop.Income * (BigInteger)Random.Range(5f, 100f);
            Info.D($"恭喜获得「{Unit.GetString(reward)}」金币");
            PropLogic.instance.AddProp("coin", reward);
            await Task.Delay(200);
            //显示广告，30%几率
            if (Random.Range(0, 100) <= 30)
            {
                hasAdInfo = true;
                MessageBox.Show("小偷の私房钱", "在抓小偷的过程中，你发现了小偷的私房钱，是否通过观看广告寻找？（可获得随机奖励）", "前往", "放弃").onComplete += id =>
                {
                    if (id != MessageBox.EventId.Ok)
                    {
                        hasAdInfo = false;
                        return;
                    }
                    //跳转界面
                    Jump.To("thief_ads");
                };
            }
            //直接获得奖励，40%几率
            else if (Random.Range(0, 100) <= 40)
            {
                var p = Random.Range(0, 100) > 80 ? "cert" : "coin_plus";
                int a = Random.Range(2,5);
                PropLogic.instance.AddProp(p, a);
                Info.D($"从小偷身上捞到了「{PropCfg.instance.GetProp(p).name}」 * {a}");
            }
        }

        public bool HasThief(Property index)
        {
            return thiefs.Contains(index);
        }

        private static ThiefLogic s_instance;
        public static ThiefLogic instance
        {
            get
            {
                if (null == s_instance)
                    s_instance = new ThiefLogic();
                return s_instance;
            }
        }
    }
}