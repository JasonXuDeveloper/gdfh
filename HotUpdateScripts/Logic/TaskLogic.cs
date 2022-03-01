//
// TaskLogic.cs
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
using UnityEngine;
using LitJson;
using System;
using Game.Data;
using Game.View;
using System.Numerics;

namespace Game.Logic
{
    public class TaskLogic
    {
        public TaskLogic()
        {
            m_taskData = new TaskData();
        }

        public static Action onAddProgress = () => { };

        /// <summary>
        /// 获取任务数据
        /// </summary>
        /// <param name="cb">回调</param>
        public void GetTaskData(Action cb)
        {
            m_taskData.GetTaskDataFromDB(cb);
        }

        /// <summary>
        /// 更新任务进度
        /// </summary>
        /// <param name="chainId">链id</param>
        /// <param name="subId">任务子id</param>
        /// <param name="deltaProgress">进度增加量</param>
        /// <param name="cb">回调</param>
        public void AddProgress(int chainId, int subId, BigInteger deltaProgress, Action<int, bool> cb)
        {
            var data = m_taskData.GetData(chainId, subId);
            if (null != data)
            {
                data.progress += deltaProgress;
                if (data.progress < 0) data.progress = 0;
                m_taskData.AddOrUpdateData(data);
                var cfg = TaskCfg.instance.GetCfgItem(data.task_chain_id, data.task_sub_id);
                if (null != cfg)
                {
                    var result = data.progress >= cfg.target_amount;
                    onAddProgress?.Invoke();
                    if (result)
                    {
                        //如果面板打开了就更新全部可以领的奖励的文本
                        if (TaskPanel.Active)
                        {
                            TaskPanel.UpdateTips();
                        }

                        if (!m_taskData.hasCompletedTask) m_taskData.hasCompletedTask.Value = data.award_is_get == 0 ? result : false;//更新可绑定数值
                    }
                    cb(0, result);
                }
                else
                    cb(-1, false);
            }
            else
            {
                cb(-1, false);
            }
        }

        /// <summary>
        /// 一键领取所有任务的奖励
        /// </summary>
        /// <param name="cb"></param>
        public void OneKeyGetAward(Action<int, List<Award>> cb)
        {
            if (!m_taskData.hasCompletedTask)
            {
                cb(-1, null);
                return;
            }

            var tmpTaskDatas = new List<TaskDataItem>(m_taskData.taskDatas);

            List<Award> awards = new List<Award>();
            Dictionary<string, int> dict = new Dictionary<string, int>(0);
            for (int i = 0, cnt = tmpTaskDatas.Count; i < cnt; ++i)
            {
                var oneTask = tmpTaskDatas[i];
                var cfg = TaskCfg.instance.GetCfgItem(oneTask.task_chain_id, oneTask.task_sub_id);
                if (oneTask.progress >= cfg.target_amount && 0 == oneTask.award_is_get)
                {
                    oneTask.award_is_get = 1;
                    m_taskData.AddOrUpdateData(oneTask);
                    var award = cfg.award;
                    if (!dict.ContainsKey(award.id))
                    {
                        dict[award.id] = award.amount;
                    }
                    else
                    {
                        dict[award.id] += award.amount;
                    }
                    GoNext(oneTask.task_chain_id, oneTask.task_sub_id, oneTask.progress - cfg.target_amount);
                }
            }
            m_taskData.hasCompletedTask.Value = false;//更新可绑定数值
            if (dict.Count > 0)
            {
                awards = new List<Award>(0);
                foreach(var kvp in dict)
                {
                    var award = new Award
                    {
                        id = kvp.Key,
                        amount = kvp.Value
                    };
                    awards.Add(award);
                    PropLogic.instance.AddProp(award);
                }
                //回调
                cb(0, awards);
            }
            else
            {
                cb(-1, null);
            }
        }

        public void AddProgressToSameTask(string type, BigInteger amount)
        {
            var t = TaskLogic.instance.m_taskData.taskDatas.Find(
                            d => d.award_is_get == 0 && TaskCfg.instance.GetCfgItem(d.task_chain_id, d.task_sub_id).target == type);
            if (t != null)
            {
                TaskLogic.instance.AddProgress(t.task_chain_id, t.task_sub_id, amount, (x, b) => { });
            }
        }

        /// <summary>
        /// 领取任务奖励
        /// </summary>
        /// <param name="chainId">链id</param>
        /// <param name="subId">任务子id</param>
        /// <param name="cb">回调</param>
        public void GetAward(int chainId, int subId, Action<int, List<Award>> cb)
        {
            if (!m_taskData.hasCompletedTask)
            {
                cb(-1, null);
                return;
            }

            var data = m_taskData.GetData(chainId, subId);
            if (null == data)
            {
                cb(-1, null);
                return;
            }
            if (0 == data.award_is_get)
            {
                data.award_is_get = 1;
                m_taskData.AddOrUpdateData(data);
                GoNext(chainId, subId, data.progress - TaskCfg.instance.GetCfgItem(chainId,subId).target_amount);

                m_taskData.hasCompletedTask.Value = HasCompletedTask();//更新可绑定数值

                var cfg = TaskCfg.instance.GetCfgItem(data.task_chain_id, data.task_sub_id);
                //获取奖励
                PropLogic.instance.AddProp(cfg.award);
                cb(0, new List<Award>() { cfg.award });
            }
            else
            {
                cb(-2, null);
            }
        }

        /// <summary>
        /// 获取全部奖励
        /// </summary>
        /// <returns></returns>
        public List<Award> AllAwards()
        {
            var tmpTaskDatas = new List<TaskDataItem>(m_taskData.taskDatas);

            List<Award> totalAward = new List<Award>();
            TaskDataItem oneTask;
            TaskCfgItem cfg;
            for (int i = 0, cnt = tmpTaskDatas.Count; i < cnt; ++i)
            {
                oneTask = tmpTaskDatas[i];
                cfg = TaskCfg.instance.GetCfgItem(oneTask.task_chain_id, oneTask.task_sub_id);
                if (oneTask.progress >= cfg.target_amount && 0 == oneTask.award_is_get)
                {
                    totalAward.Add(cfg.award);
                }
            }
            return totalAward;
        }

        /// <summary>
        /// 获取有没有已经完成的任务
        /// </summary>
        /// <returns></returns>
        public bool HasCompletedTask()
        {
            var tmpTaskDatas = new List<TaskDataItem>(m_taskData.taskDatas);
            for (int i = 0, cnt = tmpTaskDatas.Count; i < cnt; ++i)
            {
                var oneTask = tmpTaskDatas[i];
                var x = TaskCfg.instance.GetCfgItem(oneTask.task_chain_id, oneTask.task_sub_id);
                if (oneTask.progress >= x.target_amount && 0 == oneTask.award_is_get)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 触发下一个任务，并开启支线任务
        /// </summary>
        /// <param name="chainId">链id</param>
        /// <param name="subId">任务子id</param>
        public void GoNext(int chainId, int subId, BigInteger amount)
        {
            var data = m_taskData.GetData(chainId, subId);
            var cfg = TaskCfg.instance.GetCfgItem(data.task_chain_id, data.task_sub_id);
            var nextCfg = TaskCfg.instance.GetCfgItem(data.task_chain_id, data.task_sub_id + 1);

            if (1 == data.award_is_get)
            {
                // 移除掉已领奖的任务
                //m_taskData.RemoveData(chainId, subId);

                // 开启下一个任务
                if (null != nextCfg)
                {
                    if(m_taskData.taskDatas.Find(x=>x.task_sub_id == nextCfg.task_sub_id && x.task_chain_id == nextCfg.task_chain_id) == null)
                    {
                        TaskDataItem dataItem = new TaskDataItem();
                        dataItem.task_chain_id = nextCfg.task_chain_id;
                        dataItem.task_sub_id = nextCfg.task_sub_id;
                        dataItem.progress = amount;
                        dataItem.award_is_get = 0;
                        m_taskData.AddOrUpdateData(dataItem);
                    }
                }

                // 开启支线任务
                if (!string.IsNullOrEmpty(cfg.open_chain))
                {
                    // 开启新分支
                    var chains = cfg.open_chain.Split(',');
                    for (int i = 0, len = chains.Length; i < len; ++i)
                    {
                        var task = chains[i].Split('|');
                        if (m_taskData.taskDatas.Find(x => x.task_sub_id == int.Parse(task[1]) && x.task_chain_id == int.Parse(task[0])) == null)
                        {
                            TaskDataItem subChainDataItem = new TaskDataItem();
                            subChainDataItem.task_chain_id = int.Parse(task[0]);
                            subChainDataItem.task_sub_id = int.Parse(task[1]);
                            subChainDataItem.progress = 0;
                            subChainDataItem.award_is_get = 0;
                            m_taskData.AddOrUpdateData(subChainDataItem);
                        }
                    }
                }
            }
        }

        public List<TaskDataItem> taskIncompleteDatas
        {
            get { return m_taskData.taskDatas.FindAll(x => x.award_is_get == 0); }
        }

        public List<TaskDataItem> taskDatas
        {
            get { return m_taskData.taskDatas; }
        }

        public TaskData m_taskData;
        private static TaskLogic s_instance;
        public static TaskLogic instance
        {
            get
            {
                if (null == s_instance)
                    s_instance = new TaskLogic();
                return s_instance;
            }
        }
    }
}
