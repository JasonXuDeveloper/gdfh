//
// BagPanel.cs
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
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using HotUpdateScripts;
using Game.Logic;
using Game.Data;
using JEngine.Core;
using Game.Util;
using System.Numerics;
using System.Text;
using Game.Mgr;
using DG.Tweening;
using System.Threading.Tasks;

namespace Game.View
{
    public class BagPanel : MonoBehaviour
    {
        private static GameObject s_taskPanelPrefab;
        private static GameObject panelObj;
        private static GameObject itemPrefab;
        private static List<GameObject> items = new List<GameObject>();
        private static GameObject itemInfo;
        private static float openInfoTime = 0.2f;
        private static float closeInfoTime = 0.1f;

        public static bool Active => panelObj.activeSelf;

        // 显示物品栏界面
        public static void Show()
        {
            if (panelObj != null && !panelObj.activeSelf)
            {
                UpdateItems();
                itemInfo.transform.localScale = UnityEngine.Vector3.zero;
                panelObj.SetActive(true);
            }
            else if (panelObj == null)
            {
                Init();
                Show();
            }
        }

        // 隐藏物品栏界面
        public static void Hide()
        {
            if (panelObj != null && panelObj.activeSelf)
            {
                panelObj.SetActive(false);
            }
        }

        // 初始化任务界面
        public static void Init()
        {
            if (panelObj != null)
            {
                return;
            }

            if (null == s_taskPanelPrefab)
                s_taskPanelPrefab = JResource.LoadRes<GameObject>("BagPanel.prefab");
            panelObj = Instantiate(s_taskPanelPrefab);
            panelObj.SetActive(false);
            panelObj.transform.SetParent(GlobalObj.s_canvasTrans, false);
        }

        private static void UpdateItems()
        {
            var ps = PropLogic.instance.m_propData.propDatas.FindAll(x => x.id != "coin" && x.amount > 0);
            while (items.Count < ps.Count())
            {
                var go = Instantiate(itemPrefab, parent: itemPrefab.transform.parent);
                go.SetActive(true);
                items.Add(go);
            }
            while (items.Count > ps.Count())
            {
                var go = items[0];
                Destroy(go);
                items.RemoveAt(0);
            }
            ps = ps.OrderByDescending(x => PropCfg.instance.GetProp(x.id).sort).ToList();
            for (int i = 0, cnt = ps.Count; i < cnt; i++)
            {
                var go = items[i].transform;
                var p = ps[i];
                var pData = PropCfg.instance.GetProp(p.id);

                go.Find("iconframe/icon").GetComponent<Image>().sprite = SpriteManager.instance.GetSprite(pData.icon);
                go.Find("DescText").GetComponent<Text>().text = pData.name;
                go.Find("NumText").GetComponent<Text>().text = $"x{p.amount}";
                go.Find("Btn/Alarm").gameObject.SetActive(pData.canUse);
                var btn = go.Find("Btn").GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    ShowInfo(pData);
                });
            }
        }

        private static void ShowInfo(PropCfgItem prop)
        {
            itemInfo.transform.Find("Panel/Name").GetComponent<Text>().text = prop.name;
            itemInfo.transform.Find("Panel/Info").GetComponent<Text>().text = prop.desc;
            itemInfo.transform.Find("Panel/Group/Use/Alarm").gameObject.SetActive(prop.canUse);
            var btn = itemInfo.transform.Find("Panel/Group/Use").GetComponent<Button>();
            btn.gameObject.SetActive(prop.canUse);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                changeBtn(false);
                PropLogic.instance.UseProp(prop.id, 1, (x, b) =>
                {
                    if (b)
                    {
                        UpdateItems();
                        var p = PropLogic.instance.m_propData.propDatas.Find(x => x.id == prop.id);
                        if (p.amount <= 0)
                        {
                            CloseInfo();
                        }
                    }
                    changeBtn(true);
                });
            });

            BigInteger amount = PropLogic.instance.m_propData.GetData(prop.id).amount;
            btn = itemInfo.transform.btn("Panel/Group/MultiUse");
            btn.gameObject.SetActive(prop.canUse && amount >= 10);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(async() =>
            {
                changeBtn(false);
                for (int i = 0; i < 10; i++)
                {
                    PropLogic.instance.UseProp(prop.id, 1, (x, b) =>
                    {
                        if (b)
                        {
                            UpdateItems();
                            var p = PropLogic.instance.m_propData.propDatas.Find(x => x.id == prop.id);
                            if (p.amount <= 0)
                            {
                                CloseInfo();
                            }
                        }
                    });
                    await Task.Delay(85);
                }
                changeBtn(true);
            });

            btn = itemInfo.transform.btn("Panel/Group/MultiUse2");
            btn.gameObject.SetActive(prop.canUse && amount >= 100);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(async () =>
            {
                changeBtn(false);
                for(int i = 0; i < 100; i++)
                {
                    PropLogic.instance.UseProp(prop.id, 1, (x, b) =>
                    {
                        if (b)
                        {
                            UpdateItems();
                            var p = PropLogic.instance.m_propData.propDatas.Find(x => x.id == prop.id);
                            if (p.amount <= 0)
                            {
                                CloseInfo();
                            }
                        }
                    });
                    await Task.Delay(85);
                }
                changeBtn(true);
            });


            btn = itemInfo.transform.Find("Panel/Group/Close").GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(CloseInfo);

            itemInfo.transform.DOScale(UnityEngine.Vector3.one, openInfoTime);
        }

        private static void changeBtn(bool e)
        {
            var btns = Tools.FindObjectsOfTypeAll<Button>();
            for (int i = 0; i < btns.Count; i++)
            {
                btns[i].interactable = e;
            }
        }

        private static void CloseInfo()
        {
            itemInfo.transform.DOScale(UnityEngine.Vector3.zero, closeInfoTime);
        }
    }
}
