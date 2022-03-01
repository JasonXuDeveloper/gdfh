//
// TaskItemUI.cs
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
using UnityEngine.UI;
using UnityEngine;
using System;
using Game.Data;
using Game.Logic;
using Game.Mgr;
using LitJson;
using System.Linq;
using Game.Util;

namespace Game.View
{
    public class TaskItemUI : RecyclingListViewItem
    {
        // 描述
        public Text desText;
        // 进度
        public Text progressText;
        // 前往按钮
        public Button goAheadBtn;
        // 领奖按钮
        public Button getAwardBtn;
        // 进度条
        public Slider progressSlider;
        // 任务图标
        public Image icon;
        // 任务类型标记，主线/支线 
        public Image taskType;
        // 奖励图标
        public Image awardImage;
        // 奖励总数
        public Text awardText;

        public Action updateListCb;

        public bool inited = false;

        /// <summary>
        /// 更新UI
        /// </summary>
        /// <param name="data"></param>
        public void UpdateUI(TaskDataItem data)
        {
            if (!inited)
            {
                desText = transform.Find("DescText").GetComponent<Text>();
                progressText = transform.Find("Slider/ProgressText").GetComponent<Text>();
                goAheadBtn = transform.Find("GoAheadBtn").GetComponent<Button>();
                getAwardBtn = transform.Find("GetAwardBtn").GetComponent<Button>();
                progressSlider = transform.Find("Slider").GetComponent<Slider>();
                icon = transform.Find("iconframe/icon").GetComponent<Image>();
                taskType = transform.Find("iconframe/taskType").GetComponent<Image>();
                awardImage = transform.Find("award").GetComponent<Image>();
                awardText = transform.Find("award/NumText").GetComponent<Text>();
                this.UpdateRect();

                inited = true;
            }

            var cfg = TaskCfg.instance.GetCfgItem(data.task_chain_id, data.task_sub_id);
            if (null != cfg)
            {
                desText.text = cfg.desc;
                // 图标
                icon.sprite = SpriteManager.instance.GetSprite(cfg.icon);
                // 主线/支线标记
                var taskTypeSpriteName = 1 == cfg.task_chain_id ? "zhu" : "zhi";
                taskType.sprite = SpriteManager.instance.GetSprite(taskTypeSpriteName);
                //进度
                progressText.text = Unit.GetString(data.progress) + "/" + Unit.GetString(cfg.target_amount);
                progressSlider.value = Calc.GetProgress(data.progress,cfg.target_amount);
                //奖励
                var award = cfg.award;
                var iconPath = PropCfg.instance.GetProp(award.id).icon;
                awardImage.sprite = SpriteManager.instance.GetSprite(iconPath);
                awardText.text = award.amount.ToString();

                goAheadBtn.onClick.RemoveAllListeners();
                goAheadBtn.onClick.AddListener(() =>
                {
                    TipsPanel.Show(data.task_chain_id, data.task_sub_id, () =>
                    {
                        UpdateUI(data);
                    });
                });

                getAwardBtn.onClick.RemoveAllListeners();
                getAwardBtn.onClick.AddListener(() =>
                {
                    TaskLogic.instance.GetAward(data.task_chain_id, data.task_sub_id, (errorCode, awards) =>
                    {
                        Debug.Log("errorCode： " + errorCode);
                        if (0 == errorCode)
                        {
                            //展示弹窗
                            AwardPanel.Show(awards);
                            //更新UI
                            updateListCb();
                        }
                    });
                });

                goAheadBtn.gameObject.SetActive(data.progress < cfg.target_amount);
                getAwardBtn.gameObject.SetActive(data.progress >= cfg.target_amount && 0 == data.award_is_get);
            }
        }
    }

}
