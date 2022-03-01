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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using JEngine.Core;
using Game.Logic;
using System.Numerics;

namespace Game.Data
{
    public class PropData
    {
        public PropData()
        {
            m_propDatas = new List<PropDataItem>();
        }

        public BindableProperty<bool> canUseProp = new BindableProperty<bool>(false);

        /// <summary>
        /// 从数据库读取任务数据
        /// </summary>
        /// <param name="cb"></param>
        public void GetPropDataFromDB(Action cb)
        {
            // 正规是与服务端通信，从数据库中读取，这里纯客户端进行模拟，直接使用PlayerPrefs从客户端本地读取
            var jsonStr = JSaver.GetString("PROP_DATA", "[]");
            Log.Print(jsonStr);
            var propList = JsonMapper.ToObject<List<PropDataItem>>(jsonStr);
            for (int i = 0, cnt = propList.Count; i < cnt; ++i)
            {
                if(propList[i].id == "prop")
                {
                    propList[i].id = "cert";
                }
                AddOrUpdateData(propList[i]);
            }
            canUseProp.Value = PropLogic.instance.CanUseProp();
            cb();
        }

        /// <summary>
        /// 添加或更新道具
        /// </summary>
        public void AddOrUpdateData(PropDataItem itemData)
        {
            bool isUpdate = false;
            for (int i = 0, cnt = m_propDatas.Count; i < cnt; ++i)
            {
                var item = m_propDatas[i];
                if (itemData.id == item.id)
                {
                    // 更新数据
                    m_propDatas[i] = itemData;
                    isUpdate = true;
                    break;
                }
            }
            if (!isUpdate)
                m_propDatas.Add(itemData);
            // 排序，确保权重大的在最前面
            m_propDatas.Sort((a, b) =>
            {
                return PropCfg.sort[b.id].CompareTo(PropCfg.sort[a.id]);
            });
            canUseProp.Value = PropLogic.instance.CanUseProp();
            SaveDataToDB();
        }

        /// <summary>
        /// 获取某个道具
        /// </summary>
        /// <param name="chainId">链id</param>
        /// <param name="subId">任务子id</param>
        /// <returns></returns>
        public PropDataItem GetData(string id)
        {
            for (int i = 0, cnt = m_propDatas.Count; i < cnt; ++i)
            {
                var item = m_propDatas[i];
                if (item.id == id)
                    return item;
            }
            var p = PropCfg.instance.GetProp(id);
            if (p != PropCfgItem.None)
            {
                var dt = new PropDataItem { id = p.id, amount = 0 };
                AddOrUpdateData(dt);
                return dt;
            }
            return null;
        }

        /// <summary>
        /// 移除任务数据
        /// </summary>
        /// <param name="chainId">链id</param>
        /// <param name="subId">任务子id</param>
        public void RemoveData(string id)
        {
            for (int i = 0, cnt = m_propDatas.Count; i < cnt; ++i)
            {
                var item = m_propDatas[i];
                if (item.id == id)
                {
                    m_propDatas.Remove(item);
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
            var jsonStr = JsonMapper.ToJson(m_propDatas);
            JSaver.SaveAsString("PROP_DATA", jsonStr);
        }

        public void ResetData(Action cb)
        {
            JSaver.DeleteData("PROP_DATA");
            m_propDatas.Clear();
            GetPropDataFromDB(cb);
        }

        public List<PropDataItem> propDatas
        {
            get { return m_propDatas; }
        }

        // 任务数据
        private List<PropDataItem> m_propDatas;
    }

    /// <summary>
    /// 任务数据
    /// </summary>
    public class PropDataItem
    {
        // 道具id
        public string id;
        // 道具数量
        public BigInteger amount;
    }
}
