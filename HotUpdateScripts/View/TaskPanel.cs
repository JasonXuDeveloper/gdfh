//
// TaskPanel.cs
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
    public class TaskPanel : MonoBehaviour
    {
        private static GameObject s_taskPanelPrefab;
        private static GameObject panelObj;
        [HideInInspector] public GameObject scrollListGO;
        public RecyclingListView scrollList;
        public Button oneKeyGetAwardBtn;
        private static Text tips = null;

        public static bool Active => panelObj.activeSelf;

        // 显示任务界面
        public static void Show()
        {
            if (panelObj != null && !panelObj.activeSelf)
            {
                panelObj.SetActive(true);
                refresh?.Invoke();
                UpdateTips();
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
                s_taskPanelPrefab = JResource.LoadRes<GameObject>("TaskPanel.prefab");
            panelObj = Instantiate(s_taskPanelPrefab);
            panelObj.SetActive(false);
            panelObj.transform.SetParent(GlobalObj.s_canvasTrans, false);
        }

        public static Action refresh;

        private void Start()
        { 
            scrollList = scrollListGO.GetComponent<RecyclingListView>();
            refresh = RefreshAll;
            // 列表item更新回调
            scrollList.ItemCallback = PopulateItem;

            // 创建列表
            CreateList();

            oneKeyGetAwardBtn.onClick.AddListener(() =>
            {
                TaskLogic.instance.OneKeyGetAward((errorCode, awards) =>
                {
                    if (0 == errorCode)
                    {
                        AwardPanel.Show(awards);
                    }
                    RefreshAll(true);
                });
            });

            TaskLogic.onAddProgress += RefreshAll;
        }

        /// <summary>
        /// 更新全部可领取的奖励
        /// </summary>
        public static void UpdateTips()
        {
            if(TaskLogic.instance.taskIncompleteDatas.Count == 0)
            {
                tips.text = "太厉害了，任务全做完了";
                return;
            }

            //更新可领取奖励的文本
            var allAwards = TaskLogic.instance.AllAwards();
            Dictionary<string, int> dict = new Dictionary<string, int>(0);
            for(int i =0, cnt = allAwards.Count; i < cnt; i++)
            {
                var a = allAwards[i];
                if(dict.ContainsKey(a.id))
                {
                    dict[a.id] += a.amount;
                }
                else
                {
                    dict[a.id] = a.amount;
                }
            }
            var allAwardsStr = new StringBuilder();
            foreach (var kvp in dict)
            {
                allAwardsStr.Append(PropCfg.instance.GetProp(kvp.Key).name).Append('*').Append(Unit.GetString(kvp.Value)).Append(',');
            }
            if (allAwardsStr.Length > 0)
                allAwardsStr.Remove(allAwardsStr.Length - 1, 1);
            tips.text = new StringBuilder("总共可以领取: ").Append(allAwardsStr).ToString();
        }

        /// <summary>
        /// item更新回调
        /// </summary>
        /// <param name="item">复用的item对象</param>
        /// <param name="rowIndex">行号</param>
        private void PopulateItem(RecyclingListViewItem item, int rowIndex)
        {
            var child = item as TaskItemUI;
            child.UpdateUI(TaskLogic.instance.taskIncompleteDatas[rowIndex]);
            child.updateListCb = () =>
            {
                RefreshAll(true);
            };
        }

        private void RefreshAll()
        {
            RefreshAll(false);
        }

        /// <summary>
        /// 刷新整个列表
        /// </summary>
        private void RefreshAll(bool force=false)
        {
            if (!Active && !force) return;
            scrollList.RowCount = TaskLogic.instance.taskIncompleteDatas.Count;
            scrollList.Refresh();
            UpdateTips();
        }

        private void CreateList()
        {
            // 设置数据，此时列表会执行更新
            scrollList.RowCount = TaskLogic.instance.taskIncompleteDatas.Count;
        }
    }

}
