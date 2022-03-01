//
// AwardPanel.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using JEngine.Core;
using HotUpdateScripts;
using Game.Data;
using Game.Mgr;

namespace Game.View
{
    public class AwardPanel : MonoBehaviour
    {
        public Text text;
        public Image icon;
        public Button bgBtn;

        private static GameObject s_awardPanelPrefab;

        /// <summary>
        /// 显示奖励界面
        /// </summary>
        public static void Show(List<Award> award)
        {
            if (null == s_awardPanelPrefab)
                s_awardPanelPrefab = JResource.LoadRes<GameObject>("AwardPanel.prefab");
            for(int i = 0, cnt = award.Count; i < cnt; i++)
            {
                var a = award[i];
                var panelObj = Instantiate(s_awardPanelPrefab);
                panelObj.transform.SetParent(GlobalObj.s_canvasTrans, false);
                var panelBhv = panelObj.GetComponent<AwardPanel>();
                panelBhv.Init(a);
            }
        }

        public void Init(Award award)
        {
            var id = award.id;
            var iconPath = PropCfg.instance.GetProp(id).icon;
            icon.sprite = SpriteManager.instance.GetSprite(iconPath);

            text.text = award.amount.ToString();
            bgBtn.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}