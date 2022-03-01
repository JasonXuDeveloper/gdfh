//
// Program.cs
//
// Author:
//       JasonXuDeveloper（傑） <jasonxudeveloper@gmail.com>
//
// Copyright (c) 2020 JEngine
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
using JEngine.Core;
using JEngine.AntiCheat;
using JEngine.Examples;
using JEngine.Net;
using JEngine.UI;
using UnityEngine;
using UnityEngine.UI;
using Game.Data;
using Game.Logic;
using Game.View;
using Game.Util;
using LitJson;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Game.Mgr;
using System.Numerics;
using Time = Game.Util.Time;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using Application = UnityEngine.Application;
using Text = UnityEngine.UI.Text;
using System;
using Random = UnityEngine.Random;

namespace HotUpdateScripts
{
    public class Program
    {
        public static Program ins;


        public void RunGame()
        {
            ins = this;
            if (!Application.isEditor)
            {
                UnityEngine.Debug.unityLogger.logEnabled = false;
            }

            Application.targetFrameRate = 60;
            bool isNewPlayer = false;

            //内存修改
            AntiCheatHelper.OnMemoryCheatDetected(() =>
            {
                Loading.Start("检测到在修改内存进行作弊");
            });
            var j = new JAction();
            //对比版本
            j.Do(() =>
            {
                var v = JResource.LoadRes<Version>("Version.asset");
                if (Application.version != v.versionName)
                {
                    var t = v.forceUpdate ? "" : "，建议更新！";
                    MessageBox.Show("新版更新", $"检测到有新版本(v{v.versionName}){t}", "确定", "关闭");
                    if (v.forceUpdate)
                    {
                        GlobalObj.update_panel_text.text = v.content;
                        GlobalObj.update_panel.SetActive(true);
                        j.Cancel();
                    }
                }
            })
            //隐藏footer
            .Do(() =>
            {
                GlobalObj.footer.SetActive(false);
            })
            //显示登录界面
            .Do(async () =>
            {
                if (Player.Instance() == null)
                {
                    Loading.Finish();
                    LoginPanel.Show();
                    isNewPlayer = true;
                    return;
                }
                //TODO 强联网的话不需要if，直接显示登录界面
                if (Player.Instance().name.IsNullOrEmpty() || Player.Instance().password.IsNullOrEmpty())
                {
                    Loading.Finish();
                    LoginPanel.Show();
                    isNewPlayer = true;
                }
                else
                {
                    Loading.Start("正在同步数据");
                    Debug.Log("syncing");
                    await SettingLogic.GetFromCloud(Player.Instance().name, Player.Instance().password, false);
                    GameManager.Instance.LoggedIn = true;
                    Debug.Log("done");
                    Player.CleanName();
                }
            })
            //等待登录
            .Until(() => GameManager.Instance.LoggedIn)
            //显示footer
            .Do(() =>
            {
                GlobalObj.footer.SetActive(true);
            })
            //加载配置
            .Do(() =>
            {
                Loading.Start("正在加载游戏配置");
                //加载道具配置
                PropCfg.instance.LoadCfg();
                //获取道具数据
                PropLogic.instance.GetPropData(() =>
                {
                    BagPanel.Init();
                });

                //加载任务配置
                TaskCfg.instance.LoadCfg();
                //获取任务数据
                TaskLogic.instance.GetTaskData(() =>
                {
                    TaskPanel.Init();
                });

                //加载产业配置
                PropertyCfg.instance.LoadCfg();
                Player.ResetPropertyUid();
            })
            //更新排行榜
            .Do(() =>
            {
                new JAction().RepeatWhen(() =>
                {
                    RankLogic.instance.UpdateRank();
                }, () => Application.isPlaying && GameManager.Instance.LoggedIn, 60)
                .Execute(true);
            })
            //内测专属
            .Do(() =>
            {
                if (isNewPlayer && GlobalObj.IsDebug)
                {
                    PropLogic.instance.AddProp("coin_plus", 999);
                    PropLogic.instance.AddProp("cert", 999);
                }
                if (GlobalObj.IsDebug)
                {
                    GlobalObj.upper_canvas.Find("Debug").gameObject.SetActive(true);
                }
            })
            //注册道具使用事件
            .Do(() =>
            {
                //金币
                PropLogic.instance.AddUsePropAction("coin", ((amount, d) =>
                {
                }));

                //产业资格证
                PropLogic.instance.AddUsePropAction("cert", async (amount, prop) =>
                {

                    if (Time.NowTimeStamp() - PropertyLogic.instance.lastBuildTime < 3)
                    {
                        if (PropertyLogic.instance.buildAmount > 15)
                        {
                            Info.D("操作过于频繁");
                            PropLogic.instance.AddProp("cert", amount);
                            return;
                        }
                        else
                        {
                            PropertyLogic.instance.buildAmount++;
                        }
                    }
                    else
                    {
                        PropertyLogic.instance.lastBuildTime = Time.NowTimeStamp();
                        PropertyLogic.instance.buildAmount = 0;
                    }

                    for (int i = 0; i < amount; i++)
                    {
                        //抽取产业
                        try
                        {
                            var p = PropertyCfg.Random();

                            Info.D($"恭喜获得产业「{p.name}」的经营许可");
                            Player.Instance().BuiltProperties.Add(new Property
                            {
                                id = p.id,
                                uid = Player.Instance().BuiltProperties.Count
                            });
                            TaskLogic.instance.AddProgressToSameTask("unlock_property", 1);
                            TaskLogic.instance.AddProgressToSameTask("build_property", 1);
                            TaskLogic.instance.AddProgressToSameTask($"build_type_{(int)p.type}", 1);
                            Player.Save();
                            Tianti.AppLogger.onEvent($"获得「{p.name}」");
                            int index = i;
                            if (index != amount - 1)
                            {
                                await Task.Delay(75);
                            }
                        }
                        catch(Exception e)
                        {
                            Debug.LogError(e.Message);
                            Debug.LogError(e.Data["StackTrace"]);
                        }
                    }
                });

                //金币增加
                PropLogic.instance.AddUsePropAction("coin_plus", (amount, prop) =>
                {
                    for (int i = 0; i < amount; i++)
                    {
                        //没产业就固定给1000~2000
                        if (Player.Instance().BuiltProperties.Count == 0)
                        {
                            var give = Random.Range(1000, 2000);
                            PropLogic.instance.AddProp("coin", give);
                            Info.D($"恭喜获得「金币」*{Unit.GetString(give)}");
                        }
                        //随机一个玩家的产业的收益
                        else
                        {
                            var ps = Player.Instance().BuiltProperties;
                            var p = ps[Random.Range(0, ps.Count)];
                            var give = Random.Range(3, 10) * p.Income;
                            if (Random.Range(0, 100) < 8)
                            {
                                give = 0 - give;
                            }
                            PropLogic.instance.AddProp("coin", give);
                            Info.D($"恭喜获得{p.name}的一次收益：「金币」*{Unit.GetString(give)}");
                        }
                        PropertyPanel.UpdateCoinText();
                        Player.Save();
                    }
                });
            })
            //让动画播放0.1秒
            .Delay(0.1f)
            //UI处理
            .Do(() =>
            {
                Loading.Start("正在初始化UI");
                //初始化产业园
                PropertyPanel.Show();
                //对任务完成状态进行绑定
                GlobalObj.task_alarm.CreateJUI()
                    .Bind(TaskLogic.instance.m_taskData.hasCompletedTask)
                    .onMessage<bool>((x, b) =>
                    {
                        //如果面板打开了就更新全部可以领的奖励的文本
                        if (TaskPanel.Active)
                        {
                            TaskPanel.UpdateTips();
                        }

                        //看看是不是要弹出提醒图标
                        if (b)
                        {
                            x.Show();
                        }
                        else
                        {
                            x.Hide();
                        }
                    })
                    .Activate();
                //对背包道具可使用状态进行绑定
                GlobalObj.bag_alarm.CreateJUI()
                    .Bind(PropLogic.instance.m_propData.canUseProp)
                    .onMessage<bool>((x, b) =>
                    {
                        //看看是不是要弹出提醒图标
                        if (b)
                        {
                            x.Show();
                        }
                        else
                        {
                            x.Hide();
                        }
                    })
                    .Activate();
                //对产业园小偷状态进行绑定
                GlobalObj.property_alarm.CreateJUI()
                    .Bind(ThiefLogic.instance.hasThief)
                    .onMessage<bool>((x, b) =>
                    {
                        //看看是不是要弹出提醒图标
                        if (b)
                        {
                            x.Show();
                        }
                        else
                        {
                            x.Hide();
                        }
                    })
                    .Activate();
            })
            //UI动画播放
            .Delay(0.1f)
            //点击事件处理
            .Do(() =>
            {
                //产业园按钮事件
                GlobalObj.property_alarm.GetComponentInParent<Button>().onClick.AddListener(() =>
                {
                    PropertyPanel.Show();
                    BagPanel.Hide();
                    TaskPanel.Hide();
                    EventPanel.Hide();
                    RankPanel.Hide();
                });
                //任务中心按钮事件
                GlobalObj.task_alarm.GetComponentInParent<Button>().onClick.AddListener(() =>
                {
                    TaskPanel.Show();
                    PropertyPanel.Hide();
                    BagPanel.Hide();
                    EventPanel.Hide();
                    RankPanel.Hide();
                });
                //物品栏按钮事件
                GlobalObj.bag_alarm.GetComponentInParent<Button>().onClick.AddListener(() =>
                {
                    BagPanel.Show();
                    PropertyPanel.Hide();
                    TaskPanel.Hide();
                    EventPanel.Hide();
                    RankPanel.Hide();
                });
                //活动大厅
                GlobalObj.event_alarm.GetComponentInParent<Button>().onClick.AddListener(() =>
                {
                    BagPanel.Hide();
                    PropertyPanel.Hide();
                    TaskPanel.Hide();
                    RankPanel.Hide();
                    EventPanel.Show();
                });
            })
            //关闭加载界面
            .Do(() =>
            {
                Loading.Finish();
            })
            //加钱 加产业卷
            .Do(() =>
            {
                new JAction().RepeatWhen(() =>
                {
                    var s = Player.Instance().security;
                    if (s > 0)
                    {
                        int c = Random.Range(0, 101);
                        if(c <= s * 5)
                        {
                            int a = Random.Range(1, 4);
                            PropLogic.instance.AddProp("cert", a);
                            Info.D($"保安给您带来了「产业资格证」*「{a}」");
                        }
                    }
                }, () => Application.isPlaying && GameManager.Instance.LoggedIn, 10)
                .Execute(true);
                new JAction().RepeatWhen(() =>
                {
                    var bps = Player.Instance().BuiltProperties;
                    bool updated;
                    BigInteger total = 0;
                    for (int i = 0, cnt = bps.Count; i < cnt; i++)
                    {
                        var prop = bps[i];
                        var d = prop.duration;
                        var prog = prop.propProgress.Value;
                        if (prop.propProgress < 0)
                        {
                            prop.propProgress.Value = 0;
                            continue;
                        }
                        if (prog % d == 0 && prog != 0)
                        {
                            prop.propProgress.Value = 0;
                            total += prop.Income;
                        }
                        else if (prog > d)
                        {
                            prop.propProgress.Value -= d;
                            total += prop.Income;
                        }
                        else
                        {
                            prop.propProgress.Value++;
                        }
                    }
                    updated = total > 0;
                    if (updated)
                    {
                        //百分比加成
                        total *= (int)Player.Instance().incomeAddition / 100;
                        TaskLogic.instance.AddProgressToSameTask("earn", total);
                        PropLogic.instance.AddProp("coin", total);
                        Player.Save();
                        PropertyPanel.UpdateCoinText(total);
                    }
                }, () => Application.isPlaying && GameManager.Instance.LoggedIn, 1)
                .Execute(true);
            })
            //提前准备广告
            .Do(() =>
            {
                if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                {
                    Ads.instance.LoadVideo();
                }
            })
            //激活小偷
            .Do(() =>
            {
                new JAction()
                .Do(() =>
                {
                    ThiefLogic.instance.InitThief();
                })
                .Delay(10)
                .Do(() =>
                {
                    ThiefLogic.instance.InitThief();
                })
                .Delay(10)
                .Do(() =>
                {
                    ThiefLogic.instance.InitThief();
                })
                .Execute(true);
            })
            //编辑器下干的一些事情
            .Do(async () =>
            {
                Loading.Finish();
                if (!Application.isEditor) return;
                //PropLogic.instance.AddProp("cert", 10);
                //PropLogic.instance.AddProp("coin", 1000000000000000);
            })
            .Execute(true);

            //夜间确认真人
            new JAction()
                .RepeatWhen(() =>
                {
                    //判断是不是夜间
                    if (Time.InTime(24, 1, 7, Time.NowTime().Hour))
                    {
                        MessageBox.Show("验证真人", "请点击确定关闭本窗口，以确认是真人玩家");
                    }
                }, () => Application.isPlaying && GameManager.Instance.LoggedIn, 3600)
                .Execute(true);
        }
    }

    public class GlobalObj
    {
        public static bool IsDebug = false;
        public static Transform s_canvasTrans;
        public static Transform upper_canvas;
        public static GameObject footer;
        public static GameObject task_alarm;
        public static GameObject bag_alarm;
        public static GameObject property_alarm;
        public static GameObject event_alarm;
        public static GameObject loading_panel;
        public static Text loading_panel_text;
        public static GameObject update_panel;
        public static Text update_panel_text;
        public static JGameObjectPool info;
        public static Button bgm_btn;
    }
}