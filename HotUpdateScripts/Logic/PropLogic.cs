//
// PropLogic.cs
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
using Tianti;
using System.Threading.Tasks;
using JEngine.Core;

namespace Game.Logic
{
    public class PropLogic
    {
        public PropLogic()
        {
            m_propData = new PropData();
        }

        /// <summary>
        /// 获取任务数据
        /// </summary>
        /// <param name="cb">回调</param>
        public void GetPropData(Action cb)
        {
            m_propData.GetPropDataFromDB(cb);
        }

        /// <summary>
        /// 增加道具
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="deltaAmount">进度增加量</param>
        public void AddProp(string id, BigInteger deltaAmount)
        {
            var data = m_propData.GetData(id);
            if (null != data)
            {
                data.amount += deltaAmount;
                m_propData.AddOrUpdateData(data);
                if (id != "coin")
                {
                    Tianti.AppLogger.onCollect(id, (int)deltaAmount);
                }
            }
        }

        /// <summary>
        /// 增加道具
        /// </summary>
        /// <param name="award">award</param>
        public void AddProp(Award award)
        {
            AddProp(award.id, award.amount);
        }

        /// <summary>
        /// 使用道具
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="deltaAmount">进度增加量</param>
        /// <param name="cb">回调</param>
        public void UseProp(string id, BigInteger deltaAmount, Action<string,bool> cb)
        {
            var data = m_propData.GetData(id);
            if (null != data)
            {
                if (data.amount >= deltaAmount)
                {
                    if (usePropActions.ContainsKey(id))
                    {
                        data.amount -= deltaAmount;
                        m_propData.AddOrUpdateData(data);
                        cb("使用成功", true);
                        try
                        {
                            usePropActions[id].Invoke(deltaAmount, data);
                        }
                        catch
                        {
                            AddProp(id, deltaAmount);
                        }
                        if (id != "coin")
                        {
                            Tianti.AppLogger.onUse(id, (int)deltaAmount);
                        }
                    }
                    else
                    {
                        cb("该道具无法使用", false);
                    }
                }
                else
                {
                    cb("道具数量不足", false);
                }
            }
        }

        /// <summary>
        /// 使用道具时的对应事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="a"></param>
        public void AddUsePropAction(string id,Action<BigInteger,PropDataItem> a)
        {
            if (!usePropActions.ContainsKey(id))
            {
                usePropActions.Add(id, a);
            }
            else
            {
                usePropActions[id] += a;
            }
        }

        public bool CanUseProp()
        {
            for(int i=0,cnt= propDatas.Count;i<cnt;i++)
            {
                var p = propDatas[i];
                if (PropCfg.instance.GetProp(p.id).canUse && p.amount>0)
                {
                    return true;
                }
            }
            return false;
        }

        public List<PropDataItem> propDatas
        {
            get { return m_propData.propDatas; }
        }
        public Dictionary<string, Action<BigInteger, PropDataItem>> usePropActions = new Dictionary<string, Action<BigInteger, PropDataItem>>();

        public PropData m_propData;
        private static PropLogic s_instance;
        public static PropLogic instance
        {
            get
            {
                if (null == s_instance)
                    s_instance = new PropLogic();
                return s_instance;
            }
        }
    }
}
