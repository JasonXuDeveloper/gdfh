//
// PropData.cs
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
using System.Collections.Generic;
using JEngine.Core;
using LitJson;
using UnityEngine;

namespace Game.Data
{
    public class PropCfg
    {
        //道具表，id:数据
        private static Dictionary<string, PropCfgItem> m_cfg;
        //权重
        public static Dictionary<string, int> sort = new Dictionary<string, int>();

        /// <summary>
        /// 读取配置
        /// </summary>
        public void LoadCfg(string cfgName = "prop_cfg.json")
        {
            m_cfg = new Dictionary<string, PropCfgItem>();
            var txt = JResource.LoadRes<TextAsset>(cfgName).text;
            var jd = new JSONObject(txt);
            for (int i = 0, cnt = jd.Count; i < cnt; ++i)
            {
                var itemJd = jd[i];

                PropCfgItem cfgItem = JsonMapper.ToObject<PropCfgItem>(itemJd.ToString());
                m_cfg.Add(cfgItem.id, cfgItem);
                sort.Add(cfgItem.id, cfgItem.sort);
            }
        }


        private static PropCfg s_instance;
        public static PropCfg instance
        {
            get
            {
                if (null == s_instance)
                    s_instance = new PropCfg();
                return s_instance;
            }
        }

        /// <summary>
        /// 获取道具信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PropCfgItem GetProp(string id)
        {
            if(m_cfg.ContainsKey(id))
                return m_cfg[id];
            return PropCfgItem.None;
        }
    }

    public class PropCfgItem
    {
        //道具id
        public string id;
        //道具名字
        public string name;
        //道具图片
        public string icon;
        //道具描述
        public string desc;
        //权重
        public int sort;
        //可用
        public bool canUse;

        public static PropCfgItem None
        {
            get
            {
                if(none == null)
                {
                    none = new PropCfgItem
                    {
                        id = "unknown",
                        name = "未知",
                        icon = "unknown",
                        desc = "未知道具"
                    };
                }
                return none;
            }
        }
        private static PropCfgItem none;
    }
}
