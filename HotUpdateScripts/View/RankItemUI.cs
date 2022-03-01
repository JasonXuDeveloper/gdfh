//
// RankItemUI.cs
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
using Game.Data;
using Game.Util;
using UnityEngine.UI;

namespace Game.View
{
    public class RankItemUI : RecyclingListViewItem
    {
        // 排名
        public Text rankText;
        // 名字
        public Text nameText;
        // 收入
        public Text incomeText;
        // 高光
        public Image hightlight;

        public bool inited = false;

        /// <summary>
        /// 更新UI
        /// </summary>
        /// <param name="data"></param>
        public void UpdateUI(RankItem data)
        {
            if (!inited)
            {
                rankText = transform.Find("rank/txt").GetComponent<Text>();
                nameText = transform.Find("name").GetComponent<Text>();
                incomeText = transform.Find("income").GetComponent<Text>();
                hightlight = transform.Find("highlight").GetComponent<Image>();
                this.UpdateRect();

                inited = true;
            }

            int rank = Ranking.items.IndexOf(data) + 1 + (RankPanel.page - 1) * RankPanel.size;
            rankText.text = $"{rank}.";
            nameText.text = $"{data.name}";
            incomeText.text = $"秒收益：{Unit.GetString(data.income)}";

            hightlight.gameObject.SetActive(rank == Ranking.playerRank);
        }
    }
}
