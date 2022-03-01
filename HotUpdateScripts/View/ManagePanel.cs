//
// ManagePanel.cs
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
using UnityEngine;
using UnityEngine.UI;
using Game.Util;
using DG.Tweening;
using Game.Data;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Game.Logic;

namespace Game.View
{
    public class ManagePanel
    {
        private static GameObject ins;

        //动画时长
        private static float openInfoTime = 0.2f;
        private static float closeInfoTime = 0.1f;

        public static void Show(GameObject go)
        {
            if(ins == null)
            {
                ins = go;
            }
            SetUpUI();
            ins.SetActive(true);
            ins.transform.localScale = Vector3.zero;
            ins.transform.DOScale(UnityEngine.Vector3.one, openInfoTime * 1.5f);
        }

        private static void SetUpUI()
        {
            //数据
            int s = Player.Instance().security;
            bool needAds = s > 4;//招募第6~10保安需要看广告
            var ps = Player.Instance().BuiltProperties;
            BigInteger a = 0;//招募费用
            for (int i = 0, cnt = ps.Count; i < cnt; i++)
            {
                a += ps[i].Income / (int)ps[i].duration;
            }
            for(int i = 0; i < s; i++)
            {
                a *= 3;
            }
            string requirement = needAds ? "观看广告" : $"{Unit.GetString(a)}金币";
            string str = s >= 10 ? "" : $"招募保安需要{requirement}";

            //更新文字
            ins.transform.txt("Panel/Text").text = $"共{s}/10保安\n" +
                $"收益增加{s * 2}%\n" +
                $"每10秒{s * 5}%概率获得1~3产业资格证\n" + str;
            //更新按钮
            ins.transform.btn("Panel/Group/Hire").onClick.RemoveAllListeners();
            ins.transform.btn("Panel/Group/Hire").onClick.AddListener(() =>
            {
                PropertyLogic.instance.HireSecurity();
                SetUpUI();
                PropertyPanel.UpdateIncomeText();
            });
            ins.transform.btn("Panel/Group/Special").onClick.RemoveAllListeners();
            ins.transform.btn("Panel/Group/Special").onClick.AddListener(() =>
            {
                Info.D("暂未开放");
            });
            ins.transform.btn("Panel/Group/Close").onClick.RemoveAllListeners();
            ins.transform.btn("Panel/Group/Close").onClick.AddListener(() =>
            {
                ins.transform.DOScale(UnityEngine.Vector3.zero, closeInfoTime).OnComplete(() =>
                {
                    ins.SetActive(false);
                });
            });
        }
    }
}
