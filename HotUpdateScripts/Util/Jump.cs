//
// Jump.cs
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
using System.Threading.Tasks;
using Game.Data;
using Game.Logic;
using Game.View;
using Random = UnityEngine.Random;

namespace Game.Util
{
    public class Jump
    {
        private static int gap = 60;
        private static long lastTime = 0;

        public static void To(string jump_to)
        {
            if(jump_to == "get_cert")
            {
                TaskLogic.instance.AddProgress(1, 1, 1, (x, b) => { });
            }
            if (jump_to == "home")
            {
                TaskPanel.Hide();
                PropertyPanel.Show();
                BagPanel.Hide();
            }
            if (jump_to == "bag")
            {
                TaskPanel.Hide();
                PropertyPanel.Hide();
                BagPanel.Show();
            }
            if (jump_to == "ads")
            {
                if(Time.NowTimeStamp() - lastTime < gap)
                {
                    Info.D("休息一下再看广告吧~");
                    return;
                }
                Ads.instance.PlayAd(() =>
                {
                    Info.D("观看广告完成");
                    lastTime = Time.NowTimeStamp();
                    TaskLogic.instance.AddProgressToSameTask("ads", 1);
                },
                () =>
                {
                    Info.D("腾讯优量汇广告出现错误");
                },
                () =>
                {
                    Info.D("观看广告过于频繁，请稍后再试");
                });
            }
            if(jump_to == "support")
            {
                if (Time.NowTimeStamp() - lastTime < gap)
                {
                    Info.D("休息一下再看广告吧~");
                    return;
                }
                Ads.instance.PlayAd(() =>
                {
                    Info.D("感谢支持！");
                    lastTime = Time.NowTimeStamp();
                    TaskLogic.instance.AddProgressToSameTask("ads", 1);
                    if (Random.Range(0, 100) > 60)
                    {
                        Info.D("偷偷送你一个产业卷~");
                        PropLogic.instance.AddProp("cert", 1);
                    }
                },
                () =>
                {
                    Info.D("腾讯优量汇广告出现错误");
                },
                () =>
                {
                    Info.D("广告还没好哦~您的好意已经传递给作者了~");
                });
            }
            if (jump_to == "thief_ads")
            {
                Ads.instance.PlayAd(() =>
                {
                    lastTime = Time.NowTimeStamp();

                    TaskLogic.instance.AddProgressToSameTask("ads", 1);

                    int range = Random.Range(0, 100);
                    string p = "";
                    if(range > 60)
                    {
                        if(range > 90)
                        {
                            p = "cert_frag";
                        }
                        else
                        {
                            p = "cert";
                        }
                    }
                    else
                    {
                        p = "coin_plus";
                    }
                    int a = p == "cert"? Random.Range(5, 15): Random.Range(3, 10);
                    PropLogic.instance.AddProp(p, a);
                    Info.D($"恭喜获得「{PropCfg.instance.GetProp(p).name}」 * {a}");
                    ThiefLogic.hasAdInfo = false;
                },
                () =>
                {
                    Info.D("很可惜，你来晚了，小偷的祖坟都被刨光了");
                },
                () =>
                {
                    var vs = new string[]
                    {
                        "很抱歉，小偷的私房钱竟然是一个百无一用的砖头",
                        "小偷都会狡兔三窟了，你发现他只是虚晃一枪，并没有所谓私房钱",
                        "你摸到了小偷藏私房钱的地方，发现藏得竟然是SSNI-888无码版，你脸红的将之丢弃"
                    };
                    Info.D(vs[Random.Range(0,vs.Length)]);
                });
            }
        }
    }
}
