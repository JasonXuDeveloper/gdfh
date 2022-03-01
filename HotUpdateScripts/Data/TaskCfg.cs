//
// TaskCfg.cs
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
using JEngine.Core;
using System.Numerics;
using System.Linq;

namespace Game.Data
{
    public class TaskCfg
    {
        /// <summary>
        /// 读取配置
        /// </summary>
        public void LoadCfg(string cfgName = "task_cfg.json",string descName = "task_desc.json")
        {
            m_cfg = new Dictionary<int, Dictionary<int, TaskCfgItem>>();
            m_desc = new Dictionary<string, string>();
            var txt = JResource.LoadRes<TextAsset>(cfgName).text;
            var desc = JResource.LoadRes<TextAsset>(descName).text;
            var jd = new JSONObject(txt);
            var ds = new JSONObject(desc);
            for (int i = 0, cnt = ds.Count; i < cnt; ++i)
            {
                var d = ds[i];
                m_desc.Add(d["task_target"].str, d["task_fulltext"].str);
            }
           for (int i = 0, cnt = jd.Count; i < cnt; ++i)
            {
                var itemJd = jd[i];
                //Debug.Log(itemJd["task_chain_id"]+"."+itemJd["task_sub_id"]);
                //任务奖励转换
                var json = itemJd["award"].str.Split(':');
                itemJd["award"] = new JSONObject(JsonMapper.ToJson(new Award
                {
                    id = json[0],
                    amount = int.Parse(json[1].ToString())
                }));
                TaskCfgItem cfgItem = JsonMapper.ToObject<TaskCfgItem>(itemJd.ToString());
                var result = m_desc.TryGetValue(cfgItem.task_target, out var s);
                cfgItem.target = cfgItem.task_target;
                cfgItem.task_target = result ? s : cfgItem.task_target;

                if (!m_cfg.ContainsKey(cfgItem.task_chain_id))
                {
                    m_cfg[cfgItem.task_chain_id] = new Dictionary<int, TaskCfgItem>();
                }
                m_cfg[cfgItem.task_chain_id].Add(cfgItem.task_sub_id, cfgItem);
            }
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <param name="chainId">链id</param>
        /// <param name="taskSubId">任务子id</param>
        /// <returns></returns>
        public TaskCfgItem GetCfgItem(int chainId, int taskSubId)
        {
            if (m_cfg.ContainsKey(chainId) && m_cfg[chainId].ContainsKey(taskSubId))
                return m_cfg[chainId][taskSubId];
            return null;
        }

        // 任务配置，(链id : 子任务id : TaskCfgItem)
        private Dictionary<int, Dictionary<int, TaskCfgItem>> m_cfg;
        // 任务详情，（desc，详细说明）
        private Dictionary<string, string> m_desc;

        private static TaskCfg s_instance;
        public static TaskCfg instance
        {
            get
            {
                if (null == s_instance)
                    s_instance = new TaskCfg();
                return s_instance;
            }
        }
    }

    /// <summary>
    /// 任务配置结构
    /// </summary>
    public class TaskCfgItem
    {
        public int task_chain_id;
        public int task_sub_id;
        public string icon;
        public string desc;
        public string task_target;
        public string target;
        public BigInteger target_amount;
        public Award award;
        public string open_chain;
        public string jump_to;
    }

    /// <summary>
    /// 奖励结构
    /// </summary>
    public class Award
    {
        //id
        public string id;
        //数量
        public int amount;
    }
}
