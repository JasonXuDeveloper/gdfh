//
// TaskData.cs
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
using UnityEngine;
using LitJson;
using System;
using JEngine.Core;
using System.Numerics;
using Game.Logic;

namespace Game.Data
{
    public class TaskData
    {
        public TaskData()
        {
            m_taskDatas = new List<TaskDataItem>();
        }

        public BindableProperty<bool> hasCompletedTask = new BindableProperty<bool>(false);

        /// <summary>
        /// 从数据库读取任务数据
        /// </summary>
        /// <param name="cb"></param>
        public void GetTaskDataFromDB(Action cb)
        {
            // 正规是与服务端通信，从数据库中读取，这里纯客户端进行模拟，直接使用PlayerPrefs从客户端本地读取
            var jsonStr = JSaver.GetString("TASK_DATA", "[{'task_chain_id':1,'task_sub_id':1,'progress':0,'award_is_get':0}]");
            
            var taskList = JsonMapper.ToObject<List<TaskDataItem>>(jsonStr);
            for (int i = 0, cnt = taskList.Count; i < cnt; ++i)
            {
                if (taskList[i].progress < 0) taskList[i].progress = 0;
                AddOrUpdateData(taskList[i]);
            }
            if (m_taskDatas.Find(x => x.task_chain_id == 1 && x.task_sub_id == 1) == null)
            {
                ResetData(()=> { });
                AddOrUpdateData(new TaskDataItem
                {
                    task_chain_id = 1,
                    task_sub_id = 1,
                    progress = 0,
                    award_is_get = 0
                });
            }
            UpdateTaskStatus();
            hasCompletedTask.Value = TaskLogic.instance.HasCompletedTask();//更新可绑定数值
            cb();
        }

        /// <summary>
        /// 检测有没有新的任务可以去做
        /// </summary>
        public void UpdateTaskStatus()
        {
            for (int i = 0, cnt = m_taskDatas.Count; i < cnt; ++i)
            {
                var item = m_taskDatas[i];
                if (item.award_is_get ==0) continue;
                TaskLogic.instance.GoNext(item.task_chain_id, item.task_sub_id, item.progress - TaskCfg.instance.GetCfgItem(item.task_chain_id
                    , item.task_sub_id).target_amount);
            }
        }

        /// <summary>
        /// 添加或更新任务
        /// </summary>
        public void AddOrUpdateData(TaskDataItem itemData)
        {
            bool isUpdate = false;
            for (int i = 0, cnt = m_taskDatas.Count; i < cnt; ++i)
            {
                var item = m_taskDatas[i];
                if (itemData.task_chain_id == item.task_chain_id && itemData.task_sub_id == item.task_sub_id)
                {
                    // 更新数据
                    m_taskDatas[i] = itemData;
                    isUpdate = true;
                    break;
                }
            }
            if (!isUpdate)
                m_taskDatas.Add(itemData);
            // 排序，确保主线在最前面
            m_taskDatas.Sort((a, b) =>
            {
                return a.task_chain_id.CompareTo(b.task_chain_id);
            });
            SaveDataToDB();
        }

        /// <summary>
        /// 获取某个任务数据
        /// </summary>
        /// <param name="chainId">链id</param>
        /// <param name="subId">任务子id</param>
        /// <returns></returns>
        public TaskDataItem GetData(int chainId, int subId)
        {
            for (int i = 0, cnt = m_taskDatas.Count; i < cnt; ++i)
            {
                var item = m_taskDatas[i];
                if (chainId == item.task_chain_id && subId == item.task_sub_id)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 获取某个链当前任务的数据
        /// </summary>
        /// <param name="chainId">链id</param>
        /// <param name="subId">任务子id</param>
        /// <returns></returns>
        public TaskDataItem GetDataFromChain(int chainId)
        {
            for (int i = 0, cnt = m_taskDatas.Count; i < cnt; ++i)
            {
                var item = m_taskDatas[i];
                if (chainId == item.task_chain_id)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 移除任务数据
        /// </summary>
        /// <param name="chainId">链id</param>
        /// <param name="subId">任务子id</param>
        public void RemoveData(int chainId, int subId)
        {
            for (int i = 0, cnt = m_taskDatas.Count; i < cnt; ++i)
            {
                var item = m_taskDatas[i];
                if (chainId == item.task_chain_id && subId == item.task_sub_id)
                {
                    m_taskDatas.Remove(item);
                    SaveDataToDB();
                    return;
                }
            }
        }

        /// <summary>
        /// 写数据到数据库
        /// </summary>
        private void SaveDataToDB()
        {
            //var jsonStr = JsonMapper.ToJson(m_taskDatas);
            JSaver.SaveAsJSON("TASK_DATA", m_taskDatas);
        }

        public void ResetData(Action cb)
        {
            JSaver.DeleteData("TASK_DATA");
            m_taskDatas.Clear();
            GetTaskDataFromDB(cb);
        }

        public List<TaskDataItem> taskDatas
        {
            get { return m_taskDatas; }
        }

        // 任务数据
        private List<TaskDataItem> m_taskDatas;
    }

    /// <summary>
    /// 任务数据
    /// </summary>
    public class TaskDataItem
    {
        // 链id
        public int task_chain_id;
        // 任务子id
        public int task_sub_id;
        // 进度
        public BigInteger progress;
        // 奖励是否被领取了，0：未被领取，1：已被领取
        public short award_is_get;
    }
}
