//
// PropertyData.cs
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
using System.Numerics;
using JEngine.Core;
using LitJson;
using UnityEngine;

namespace Game.Data
{
    public class PropertyCfg
    {
        //道具表，id:数据
        public static List<PropertyCfgItem> m_cfg;

        /// <summary>
        /// 读取配置
        /// </summary>
        public void LoadCfg(string cfgName = "property_cfg.json")
        {
            m_cfg = new List<PropertyCfgItem>();
            var txt = JResource.LoadRes<TextAsset>(cfgName).text;
            m_cfg = JsonMapper.ToObject<List<PropertyCfgItem>>(txt);
        }


        private static PropertyCfg s_instance;
        public static PropertyCfg instance
        {
            get
            {
                if (null == s_instance)
                    s_instance = new PropertyCfg();
                return s_instance;
            }
        }

        public static PropertyCfgItem Random()
        {
            int chance = UnityEngine.Random.Range(0, 100);
            var avaliable = m_cfg.FindAll(x => x.chance > chance);
            while(avaliable.Count == 0)
            {
                chance = UnityEngine.Random.Range(0, 100);
                avaliable = m_cfg.FindAll(x => x.chance > chance);
            }
            return avaliable[UnityEngine.Random.Range(0, avaliable.Count)];
        }

        public static PropertyCfgItem GetProperty(int id)
        {
            return m_cfg.Find(x => x.id == id);
        }

        public static PropertyCfgItem GetProperty(string name)
        {
            return m_cfg.Find(x => x.name == name);
        }
    }

    public class PropertyCfgItem
    {
        public int id;
        public string name;//名字
        public PropertyType type;//类型
        public BigInteger baseIncome;//基础收益
        public int duration;//收益时长
        public int chance;//抽到的概率，百分比
    }
}