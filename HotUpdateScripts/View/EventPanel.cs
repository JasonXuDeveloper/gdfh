//
// EventPanel.cs
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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using HotUpdateScripts;
using Game.Logic;
using Game.Data;
using JEngine.Core;
using Game.Util;
using System.Numerics;
using System.Text;
using System;

namespace Game.View
{
    public class EventPanel : MonoBehaviour
    {
        private static GameObject s_taskPanelPrefab;
        public static GameObject panelObj;
        public static bool Active => panelObj.activeSelf;

        // 显示任务界面
        public static void Show()
        {
            if (panelObj != null && !panelObj.activeSelf)
            {
                panelObj.SetActive(true);
            }
            else if(panelObj == null)
            {
                Init();
                Show();
            }
        }

        // 隐藏任务界面
        public static void Hide()
        {
            if (panelObj != null && panelObj.activeSelf)
            {
                panelObj.SetActive(false);
            }
        }

        // 初始化任务界面
        public static void Init()
        {
            if (panelObj != null)
            {
                return;
            }

            if (null == s_taskPanelPrefab)
                s_taskPanelPrefab = JResource.LoadRes<GameObject>("EventPanel.prefab");
            panelObj = Instantiate(s_taskPanelPrefab);
            panelObj.SetActive(false);
            panelObj.transform.SetParent(GlobalObj.s_canvasTrans, false);
        }

        public Button rankBtn;
        public Button rankAllBtn;
        public Button dailyRewardBtn;
        public Button supportBtn;

        private void Start()
        {
            rankBtn.onClick.AddListener(() =>
            {
                RankPanel.all = false;
                RankPanel.Show();
            });
            rankAllBtn.onClick.AddListener(() =>
            {
                RankPanel.all = true;
                RankPanel.Show();
            });
            dailyRewardBtn.onClick.AddListener(() =>
            {
                Info.D("帅气的作者还在思考每日奖励的内容~");
            });
            supportBtn.onClick.AddListener(() =>
            {
                Jump.To("support");
            });
        }
    }
}
