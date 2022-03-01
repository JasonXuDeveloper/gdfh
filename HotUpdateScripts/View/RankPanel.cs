//
// RankPanel.cs
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
    public class RankPanel : MonoBehaviour
    {
        private static GameObject s_taskPanelPrefab;
        private static GameObject panelObj;
        [HideInInspector] public GameObject scrollListGO;
        public RecyclingListView scrollList;
        public static Text playerRank;
        public static Dropdown pageDropdown;
        public static Text pageText;
        public static Button left;
        public static Button right;
        public static bool Active => panelObj.activeSelf;
        private static Action onOpen;

        public static int page = 1;
        public static int totalPage = 0;
        public static int size = 30;
        public static bool all = false;

        // 显示界面
        public static void Show()
        {
            if (panelObj != null && !panelObj.activeSelf)
            {
                onOpen?.Invoke();
                panelObj.SetActive(true);
                panelObj.transform.SetAsLastSibling();
            }
            else if(panelObj == null)
            {
                Init();
                Show();
            }
        }

        // 隐藏界面
        public static void Hide()
        {
            if (panelObj != null && panelObj.activeSelf)
            {
                panelObj.SetActive(false);
            }
        }

        // 初始化界面
        public static void Init()
        {
            if (panelObj != null)
            {
                return;
            }

            if (null == s_taskPanelPrefab)
                s_taskPanelPrefab = JResource.LoadRes<GameObject>("RankPanel.prefab");
            panelObj = Instantiate(s_taskPanelPrefab);
            panelObj.SetActive(false);
            panelObj.transform.SetParent(GlobalObj.s_canvasTrans, false);
        }

        private void Start()
        {
            scrollList = scrollListGO.GetComponent<RecyclingListView>();

            // 列表item更新回调
            scrollList.ItemCallback = PopulateItem;

            // 创建列表
            CreateList();

            left.gameObject.SetActive(page != 1);
            right.gameObject.SetActive(page != totalPage);

            left.onClick.RemoveAllListeners();
            left.onClick.AddListener(() =>
            {
                if (page > 1)
                {
                    page--;
                    onOpen?.Invoke();
                    left.gameObject.SetActive(page != 1);
                    right.gameObject.SetActive(page != totalPage);
                }
            });

            right.onClick.RemoveAllListeners();
            right.onClick.AddListener(() =>
            {
                if (page < totalPage)
                {
                    page++;
                    onOpen?.Invoke();
                    left.gameObject.SetActive(page != 1);
                    right.gameObject.SetActive(page != totalPage);
                }
            });

            onOpen = async () =>
            {
                Loading.Start("正在加载排名");
                var ret = await RankLogic.instance.GetRank(page,size,RankPanel.all)
                .TimeoutAfter(TimeSpan.FromSeconds(15), () =>
                {
                    Loading.Finish();
                    Info.D("排行榜加载异常");
                });
                if (ret > 0)
                {
                    RefreshAll();
                    if(ret % size == 0)
                    {
                        totalPage = ret / size;
                    }
                    else
                    {
                        totalPage = (ret - ret % size) / size + 1;
                    }

                    pageDropdown.onValueChanged.RemoveAllListeners();

                    pageDropdown.ClearOptions();

                    List<string> s = new List<string>();
                    for (int i = 1; i <= totalPage+1; i++)
                    {
                        s.Add(i<=totalPage?$"第{i}页":"到底啦~");
                    }

                    pageDropdown.AddOptions(s);
                    pageDropdown.value = page - 1;
                    pageDropdown.onValueChanged.AddListener(OnPageSelect);

                    Loading.Finish();

                    pageText.text = $"{page}/{totalPage}页";
                    playerRank.text = $"玩家排名：第{Ranking.playerRank}名";
                }
            };

            onOpen();
        }

        private void OnPageSelect(int index)
        {
            page = index + 1;
            if (page > totalPage) page = totalPage;
            onOpen?.Invoke();
        }


        /// <summary>
        /// item更新回调
        /// </summary>
        /// <param name="item">复用的item对象</param>
        /// <param name="rowIndex">行号</param>
        private void PopulateItem(RecyclingListViewItem item, int rowIndex)
        {
            var child = item as RankItemUI;
            child.UpdateUI(Ranking.items[rowIndex]);
        }

        /// <summary>
        /// 刷新整个列表
        /// </summary>
        private void RefreshAll()
        {
            scrollList.RowCount = Ranking.items.Count;
            scrollList.Refresh();
        }

        private void CreateList()
        {
            // 设置数据，此时列表会执行更新
            scrollList.RowCount = Ranking.items.Count;
        }
    }
}
