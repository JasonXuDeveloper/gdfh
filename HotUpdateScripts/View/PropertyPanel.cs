//
// PropertyPanel.cs
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
using Vector3 = UnityEngine.Vector3;
using System;

namespace Game.View
{
    public class PropertyPanel : MonoBehaviour
    {
        //初始化用的
        private static GameObject s_taskPanelPrefab;
        private static GameObject panelObj;
        private static GameObject itemPrefab;
        private static List<GameObject> items = new List<GameObject>();

        //新手提示
        private static Text tip;

        //顶部数据
        private static Text coinText;
        private static Text coinAni;
        private static Text incomeText;

        //两个面板
        private static GameObject itemInfo;

        //cdk部分
        private static GameObject cdkPanel;
        private static Button cdkBtn;

        //设置部分
        private static GameObject settingPanel;
        private static Button settingBtn;

        //管理面板
        private static GameObject managePanel;

        //动画时长
        private static float openInfoTime = 0.2f;
        private static float closeInfoTime = 0.1f;

        //选择产业类型部分
        public static PropertyType selectedType;
        public static Transform content;

        //图鉴部分
        private static Button galleryBtn;
        public static GalleryLevelSelectionManager galleryMgr;
        public static GameObject galleryItem;
        public static List<GameObject> galleryCache = new List<GameObject>();
        private static bool galleryInited;
        private static bool onGallery;

        //指定产业部分
        private static Text pageText;
        private static Button leftBtn;
        private static Button rightBtn;
        private static Button descBtn;
        public static Dropdown pageDropdown;
        public static Dropdown typeDropdown;
        public static int curPage = 1;
        public static int totalPages = 1;
        public static bool oneKey = false;
        public static int pageSize;
        public static bool onPage;
        public static int curId;
        public static int curType;
        public static bool desc;
        public static string curProp = "全部产业";

        public static List<Image> imgs;
        public static List<Button> thiefs;
        public static List<Property> activeProp;

        public static bool Active => panelObj.activeSelf;

        public static string[] colors = new string[]
        {
            "#FFFFFF",
            "#F0E4D7",
            "#F5C0C0",
            "#FF7171",
            "#99FEFF",
            "#94DAFF",
            "#94B3FD",
            "#B983FF",
            "#AA14F0",
            "#77E4D4"
        };

        // 显示物品栏界面
        public static void Show()
        {
            if (panelObj != null)
            {
                Player.ResetPropertyUid();
                if (Player.Instance().BuiltProperties.Count >= 800)
                {
                    MessageBox.Show("免费合并产业", "检测到您的产业数量过多，可能会造成游戏卡顿，点击确定可以免费自动将每5个同类型同阶产业合成为下一阶同类型产业（不会合成2阶及以上的产业，可计入进阶任务）")
                        .onComplete += id =>
                         {
                             if (id != MessageBox.EventId.Ok) return;
                             Player.AutoMerge();
                             Player.ResetPropertyUid();
                         };
                }
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
                s_taskPanelPrefab = JResource.LoadRes<GameObject>("PropertyPanel.prefab");
            panelObj = Instantiate(s_taskPanelPrefab);
            panelObj.SetActive(false);
            panelObj.transform.SetParent(GlobalObj.s_canvasTrans, false);
            UpdateIncomeText();
            UpdateCoinText();
            GlobalObj.bgm_btn = panelObj.transform.Find("Bgm").GetComponent<Button>();
            GlobalObj.bgm_btn.onClick.RemoveAllListeners();
            GlobalObj.bgm_btn.onClick.AddListener(() =>
            {
                var au = Tools.FindObjectsOfTypeAll<AudioSource>()[0];
                var ip = au.isPlaying;
                if (ip)
                {
                    au.Pause();
                    GlobalObj.bgm_btn.GetComponentInChildren<Image>().sprite = SpriteManager.instance.GetSprite("speaker_mute");
                }
                else
                {
                    au.Play();
                    GlobalObj.bgm_btn.GetComponentInChildren<Image>().sprite = SpriteManager.instance.GetSprite("speaker_high");
                }
            });


            descBtn.GetComponentInChildren<Image>().sprite = SpriteManager.instance.GetSprite(desc ? "on" : "off");
            descBtn.onClick.RemoveAllListeners();
            descBtn.onClick.AddListener(() =>
            {
                desc = !desc;
                descBtn.GetComponentInChildren<Image>().sprite = SpriteManager.instance.GetSprite(desc ? "on" : "off");

                ShowProperty();
            });

            galleryBtn.onClick.RemoveAllListeners();
            galleryBtn.onClick.AddListener(()=>
            {
                HideGallery();
                SetChooseTypeContent();
            });

            cdkBtn.onClick.RemoveAllListeners();
            cdkBtn.onClick.AddListener(() =>
            {
                cdkPanel.SetActive(true);
                cdkPanel.transform.localScale = UnityEngine.Vector3.zero;
                cdkPanel.transform.DOScale(UnityEngine.Vector3.one, openInfoTime * 1.5f);
            });
            cdkPanel.transform.btn("Panel/Group/Claim").onClick.RemoveAllListeners();
            cdkPanel.transform.btn("Panel/Group/Claim").onClick.AddListener(() =>
            {
                var i = cdkPanel.transform.Find("Panel/input").GetComponent<InputField>();
                cdkPanel.transform.btn("Panel/Group/Claim").interactable = false;
                CDKLogic.ClaimCDK(i.text);
                cdkPanel.transform.btn("Panel/Group/Claim").interactable = true;
            });
            cdkPanel.transform.btn("Panel/Group/Close").onClick.AddListener(() =>
            {
                cdkPanel.transform.DOScale(UnityEngine.Vector3.zero, closeInfoTime);
                cdkPanel.SetActive(false);
            });

            settingBtn.onClick.RemoveAllListeners();
            settingBtn.onClick.AddListener(() =>
            {
                settingPanel.transform.txt("Panel/Text").text = $"游戏名：{Player.Instance().name}\n" +
                $"密码：{Player.Instance().password}\n" +
                $"区服：{Player.Instance().server}区\n" +
                $"退出前请先存档";
                settingPanel.SetActive(true);
                settingPanel.transform.localScale = UnityEngine.Vector3.zero;
                settingPanel.transform.DOScale(UnityEngine.Vector3.one, openInfoTime * 1.5f);
            });
            settingPanel.transform.btn("Panel/Group/Quit").onClick.RemoveAllListeners();
            settingPanel.transform.btn("Panel/Group/Quit").onClick.AddListener(() =>
            {
                MessageBox.Show("退出警告", "请确认已存档，不然无法保证数据不会丢失，退出成功后需重启游戏").onComplete += (id) =>
                {
                    if (id != MessageBox.EventId.Ok)
                    {
                        return;
                    }
                    Loading.Start("正在退出中");
                    JSaver.DeleteAll();
                    Player.LogOut();
                    Application.Quit();
                };
            });
            settingPanel.transform.btn("Panel/Group/Save").onClick.RemoveAllListeners();
            settingPanel.transform.btn("Panel/Group/Save").onClick.AddListener(async () =>
            {
                Loading.Start("存档中");
                await SettingLogic.SaveToCloud();
                Loading.Finish();
            });
            settingPanel.transform.btn("Panel/Group/Close").onClick.AddListener(() =>
            {
                settingPanel.transform.DOScale(UnityEngine.Vector3.zero, closeInfoTime).OnComplete(() =>
                {
                    settingPanel.SetActive(false);
                });
            });


            var g = content.Find("Properties").gameObject;
            g.transform.btn("OneKeyUpgrade").onClick.AddListener(async () =>
            {
                var temp = oneKey;
                oneKey = true;
                if (activeProp != null)
                {
                    changeBtn(false);
                    for (int i = 0; i < activeProp.Count; i++)
                    {
                        var p = activeProp[i];
                        Loading.Start($"开始升级「{p.name}」");
                        await Task.Delay(150);
                        await PropertyLogic.instance.Upgrade(p, () => { });
                        await Task.Delay(100);
                    }
                    changeBtn(true);
                }
                oneKey = temp;
                UpdateIncomeText();
                ShowProperty();
                Loading.Finish();
            });
            g.transform.btn("OneKeyEmploy").onClick.AddListener(async () =>
            {
                var temp = oneKey;
                oneKey = true;
                if (activeProp != null)
                {
                    changeBtn(false);
                    for (int i = 0; i < activeProp.Count; i++)
                    {
                        var p = activeProp[i];
                        if (!Player.Instance().BuiltProperties.Contains(p)) continue;
                        Loading.Start($"开始雇员「{p.name}」");
                        await Task.Delay(150);
                        await PropertyLogic.instance.Employ(p, () => { });
                        await Task.Delay(100);
                    }
                    changeBtn(true);
                }
                oneKey = temp;
                UpdateIncomeText();
                ShowProperty();
                Loading.Finish();
            });
            g.transform.btn("OneKeyUpstage").onClick.AddListener(async () =>
            {
                if (activeProp != null)
                {
                    changeBtn(false);
                    for (int i = 0; i < activeProp.Count; i++)
                    {
                        var p = activeProp[i];
                        Loading.Start($"开始进阶「{p.name}」");
                        await Task.Delay(150);
                        await PropertyLogic.instance.Upstage(p, () => { });
                        await Task.Delay(100);
                        ShowProperty();
                    }
                    changeBtn(true);
                }
                Player.ResetPropertyUid();
                ShowProperty();
                UpdateIncomeText();
                Loading.Finish();
            });
            g.transform.btn("Manage").onClick.AddListener(() =>
            {
                ManagePanel.Show(managePanel);
            });
        }

        public static void UpdateCoinText()
        {
            UpdateCoinText(0);
        }

        public static void UpdateCoinText(BigInteger earn)
        {
            coinText.text = Unit.GetString(Player.Instance().Coin);
            if (earn > 0)
            {
                coinAni.text = $"+{Unit.GetString(earn)}";
                coinAni.DOFade(1, 0.7f).OnComplete(() =>
                {
                    coinAni.DOFade(0, 0.2f);
                });
            }
        }

        public static void UpdateIncomeText()
        {
            var ps = Player.Instance().BuiltProperties;
            BigInteger a = 0;
            for (int i = 0, cnt = ps.Count; i < cnt; i++)
            {
                a += ps[i].Income / (int)ps[i].duration;
            }
            incomeText.text = Unit.GetString(a) + "\n" + $"*{Player.Instance().incomeAddition}%";
        }

        private static void UpdateItems()
        {
            if (Player.Instance().BuiltProperties.Count == 0)
            {
                tip.gameObject.SetActive(true);
            }
            else
            {
                tip.gameObject.SetActive(false);
            }
            try
            {
                HideGallery();
                var go = content.Find("Select").gameObject;
                go.SetActive(false);
                cdkPanel.SetActive(false);
                ShowProperty();
            }
            catch(Exception e)
            {
                Info.D($"请联系作者！！！！{e.Message}");
                Debug.LogError(e);
                Debug.LogError(e.Data["StackTrace"]);
            }
        }

        /// <summary>
        /// 更新选择类型的界面
        /// </summary>
        private static void SetChooseTypeContent()
        {
            //界面
            var go = content.Find("Select").gameObject;
            //显示到最上层
            go.transform.SetAsLastSibling();
            go.SetActive(true);
            go.transform.localScale = UnityEngine.Vector3.zero;
            go.transform.DOScale(UnityEngine.Vector3.one, openInfoTime * 1.5f);

            //按钮
            var btns = go.GetComponentsInChildren<Button>();
            for (int i = 0; i < 3; i++)
            {
                //数据
                var btn = btns[i];
                PropertyType type = (PropertyType)i;
                int has = PropertyLogic.instance.HasAmount(type);
                int total = PropertyLogic.instance.TotalAmount(type);
                BigInteger avgIncome = PropertyLogic.instance.AvgIncome(type);
                Color c = PropertyLogic.instance.unlockAmountColor(has, total);

                //更新UI
                btn.transform.outline().effectColor = c;
                btn.transform.txt("txt").color = c;
                btn.transform.txt("txt").text = $"{type}产业";
                btn.transform.txt("info").text = $"已解锁{has}/{total}种\n平均秒收益{Unit.GetString(avgIncome)}";

                //点击事件
                btn.onClick.AddListener(() =>
                {
                    selectedType = type;
                    ShowGallery();
                    go.transform.DOScale(UnityEngine.Vector3.zero, closeInfoTime);
                    go.SetActive(false);
                });
            }
        }

        /// <summary>
        /// 隐藏滚动部分
        /// </summary>
        private static void HideGallery()
        {
            //界面
            var go = content.Find("Gallery").gameObject;
            go.transform.DOScale(UnityEngine.Vector3.zero, closeInfoTime);
        }

        /// <summary>
        /// 更新卡牌滚动部分
        /// </summary>
        private static async void ShowGallery()
        {
            //界面
            var go = content.Find("Gallery").gameObject;
            //显示到最上层
            go.transform.SetAsLastSibling();
            go.SetActive(true);
            go.transform.localScale = UnityEngine.Vector3.zero;
            go.transform.DOScale(UnityEngine.Vector3.one, openInfoTime * 1.5f);
            onGallery = true;

            //禁用滚动代码
            galleryMgr.enabled = false;


            //获得这个type的产业的总数（不包含重复id的）
            int total = PropertyLogic.instance.TotalAmount(selectedType);

            //更新预制体
            while (galleryCache.Count < total)
            {
                var g = Instantiate(galleryItem, parent: galleryMgr.itemsContainer);
                g.SetActive(true);
                galleryCache.Add(g);
            }
            while (galleryCache.Count > total)
            {
                var g = galleryCache[0];
                Destroy(g);
                galleryCache.RemoveAt(0);
            }

            //生成新的预制体并获得GalleryItem
            List<GalleryLevelView> views = new List<GalleryLevelView>();
            List<PropertyCfgItem> cfgItems = new List<PropertyCfgItem>();
            Info.D("请稍等");
            await Task.Run(() =>
            {
                cfgItems = PropertyLogic.instance.OrderCfgItems(selectedType, desc);
            });
            for(int i = 0; i < total; i++)
            {
                //数据
                int id = cfgItems[i].id;
                int amount = PropertyLogic.instance.TotalHasAmount(id);
                string name = cfgItems[i].name;
                BigInteger avgIncome = PropertyLogic.instance.AvgIncome(id);

                //生成
                var item = galleryCache[i];

                //给view赋值
                var view = item.GetComponent<GalleryLevelView>();
                view.manager = galleryMgr;
                view.levelName = name;

                //更新UI
                var trans = item.transform;
                trans.txt("Total").text = $"共{amount}分部";
                trans.txt("Name").text = amount > 0 ? name : "???";
                trans.txt("Income").text = $"平均秒收益{Unit.GetString(avgIncome)}";

                //加入缓存
                views.Add(view);
            }

            //数据设置
            galleryMgr.items = views;

            //重新初始化滚动代码
            galleryMgr.enabled = true;
            if (galleryInited)
            {
                galleryMgr.Start();
            }
            else
            {
                galleryInited = true;
            }
        }

        /// <summary>
        /// 展示某产业的信息
        /// </summary>
        public static void ShowProperty(bool changeInAmount = true,int itemIndex= -1)
        {
            //界面
            var g = content.Find("Properties").gameObject;
            //显示到最上层
            g.transform.SetAsLastSibling();
            g.SetActive(true);
            if (!onPage)
            {
                g.transform.localScale = UnityEngine.Vector3.zero;
                g.transform.DOScale(UnityEngine.Vector3.one, openInfoTime * 1.5f).OnComplete(HideGallery);
            }

            //筛选产业
            var ps = Player.Instance().BuiltProperties;
            if (!curProp.StartsWith("全部产业"))
            {
                ps = ps.FindAll(p => p.name.StartsWith(curProp.Split('(')[0]));
            }

            //缓存两个值减少call
            Dictionary<int,int> tempD = new Dictionary<int, int>();
            Dictionary<int, BigInteger> tempI = new Dictionary<int, BigInteger>();

            for(int i = 0; i < ps.Count; i++)
            {
                var p = ps[i];
                tempD[p.uid] = p.duration;
                tempI[p.uid] = p.Income;
            }

            //排序
            if (!desc)
            {
                ps = ps.OrderBy(p =>
                {
                    return (int)p.grade + tempI[p.uid] / tempD[p.uid];
                }).ToList();
            }
            else
            {
                ps = ps.OrderByDescending(p =>
                {
                    return (int)p.grade + tempI[p.uid] / tempD[p.uid];
                }).ToList();
            }

            //计算分页
            onPage = true;

            pageSize = 6;
            //分页，8个产业就是8-1+7=14/7=2页
            totalPages = ps.Count % pageSize == 0 ? ps.Count / pageSize : (ps.Count - ps.Count % pageSize + pageSize) / pageSize;
            //判断页数对不对
            if (curPage > totalPages)
            {
                curPage = totalPages;
            }
            if (curPage < 1)
            {
                curPage = 0;
            }

            if (changeInAmount)
            {
                pageText.text = $"{curPage}/{totalPages}页";
                pageDropdown.onValueChanged.RemoveAllListeners();
                pageDropdown.ClearOptions();
                List<string> s = new List<string>();
                for (int i = 1; i <= totalPages + 1; i++)
                {
                    s.Add(i <= totalPages ? $"第{i}页" : "到底啦~");
                }

                //DROPDOWN设置
                pageDropdown.AddOptions(s);
                pageDropdown.value = curPage - 1;
                pageDropdown.onValueChanged.AddListener(OnPageSelect);

                //筛选类型
                typeDropdown.onValueChanged.RemoveAllListeners();
                typeDropdown.ClearOptions();
                s.Clear();
                s.Add($"全部产业({Player.Instance().BuiltProperties.Count})");
                for (int i = 0; i < PropertyCfg.m_cfg.Count; i++)
                {
                    var p = PropertyCfg.m_cfg[i];
                    var a = PropertyLogic.instance.TotalHasAmount(p.id);
                    if (a == 0) continue;
                    s.Add($"{p.name}({a})");
                }
                s.Add("到底啦~");

                //DROPDOWN设置
                typeDropdown.AddOptions(s);
                typeDropdown.value = curType;
                typeDropdown.onValueChanged.AddListener(OnTypeSelect);
                s = null;
            }

            //切割产业
            ps = ps.Skip((curPage - 1) * pageSize).Take(pageSize).ToList();

            //分页点击事件
            leftBtn.gameObject.SetActive(curPage != 1);
            rightBtn.gameObject.SetActive(curPage < totalPages);
            leftBtn.onClick.RemoveAllListeners();
            leftBtn.onClick.AddListener(() =>
            {
                if (curPage > 1)
                {
                    curPage--;
                    ShowProperty();
                }
            });
            rightBtn.onClick.RemoveAllListeners();
            rightBtn.onClick.AddListener(() =>
            {
                if (curPage < totalPages)
                {
                    curPage++;
                    ShowProperty();
                }
            });

            //更新预制体
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

            //UI缓存
            imgs = new List<Image>();
            //当前在展示的index
            activeProp = new List<Property>();
            //小偷图片
            thiefs = new List<Button>();

            //展示数据
            for (int i = 0, cnt = ps.Count; i < cnt; i++)
            {
                int index = i;
                //获取UI
                var go = items[i].transform;
                //获取产业
                var p = ps[i];
                //加入在展示的index列表
                activeProp.Add(p);
                thiefs.Add(go.btn("thief"));

                //if(!changeInAmount && itemIndex != index)
                //{
                //    continue; 
                //}

                //更新颜色
                var color = colors[(((int)(p.grade)) < colors.Count()) ? p.grade : 0].ToColor();
                go.img().color = color;
                go.img("e/fill").color = color;
                go.img("l/fill").color = color;
                go.txt("name").color = color;

                //更新进度条
                go.img("e/fill").fillAmount = PropertyLogic.instance.GetEmployProgress(p);
                go.img("l/fill").fillAmount = PropertyLogic.instance.GetLevelProgress(p);

                //更新文字
                go.txt("name").text = $"{p.grade}阶·{p.name}";
                go.txt("e/txt").text = $"{p.employee}/{PropertyLogic.instance.MaxEmploy(p)}";
                go.txt("l/txt").text = $"{p.level}/{PropertyLogic.instance.MaxLevel(p)}";
                go.txt("income").text = $"{tempD[p.uid]}秒/{Unit.GetString(tempI[p.uid])}金币";

                //按钮点击事件
                go.btn("Btn").onClick.RemoveAllListeners();
                go.btn("Btn").onClick.AddListener(() =>
                {
                    ShowInfo(index);
                });
            }
        }

        private static void OnPageSelect(int index)
        {
            curPage = index + 1;
            if (curPage > totalPages) curPage = totalPages;
            ShowProperty();
        }

        private static void OnTypeSelect(int index)
        {
            curType = index;
            var val = typeDropdown.options[index].text;
            curProp = val;
            ShowProperty();
        }


        private static void updateText(Property prop,bool force)
        {
            UpdateCoinText();
            UpdateIncomeText();
            if (force) return;

            bool rml = PropertyLogic.instance.ReachMaxLevel(prop);
            bool rme = PropertyLogic.instance.ReachMaxEmploy(prop);
            if (!rml)
            {
                if (!rme)
                {
                    itemInfo.transform.Find("Panel/Info").GetComponent<Text>().text = $"升级需要：{Unit.GetString(prop.UpgradeFee)}金币\n" +
                        $"雇员需要：{Unit.GetString(prop.EmployFee)}金币";
                }
                else
                {
                    itemInfo.transform.Find("Panel/Info").GetComponent<Text>().text = $"升级需要：{Unit.GetString(prop.UpgradeFee)}金币\n" +
                        $"员工已满";
                }
            }
            else
            {
                if (!rme)
                {
                    itemInfo.transform.Find("Panel/Info").GetComponent<Text>().text = $"已满级\n" +
                        $"雇员需要：{Unit.GetString(prop.EmployFee)}金币";
                }
                else
                {
                    if (prop.grade < Property.GradeBenefit.Length)
                    {
                        itemInfo.transform.Find("Panel/Info").GetComponent<Text>().text =
                            $"进阶需要：\n" +
                            $"满级满员工的{prop.grade}阶·{prop.name} * 1\n"+
                            $"金币：{Unit.GetString(prop.UpstageFee)}";
                    }
                    else
                    {
                        itemInfo.transform.Find("Panel/Info").GetComponent<Text>().text = $"已满级\n" +
                            $"员工已满\n" +
                            $"等阶已满";
                    }
                }
            }
        }

        private static void update(Property p,bool forceRefresh=false)
        {
            ShowProperty(forceRefresh,activeProp.IndexOf(p));

            //更新面板文字
            updateText(p,forceRefresh);

            //更新面板四个按钮的状态
            bool rml = PropertyLogic.instance.ReachMaxLevel(p);
            bool rme = PropertyLogic.instance.ReachMaxEmploy(p);
            itemInfo.transform.Find("Panel/Group/Upgrade").gameObject.SetActive(!rml);
            itemInfo.transform.Find("Panel/Group/Employ").gameObject.SetActive(!rme);
            itemInfo.transform.Find("Panel/Group/OneKey").gameObject.SetActive(!rml || !rme);
            itemInfo.transform.Find("Panel/Group/Upstage").gameObject.SetActive(rml && rme && p.grade< Property.GradeBenefit.Length);
        }

        private static void changeBtn(bool e)
        {
            var btns = Tools.FindObjectsOfTypeAll<Button>();
            for (int i = 0; i < btns.Count; i++)
            {
                btns[i].interactable = e;
            }
        }

        private static void ShowInfo(int index)
        {
            var prop = activeProp[index];

            update(prop);

            itemInfo.transform.Find("Panel/Name").GetComponent<Text>().text = prop.name;

            var closeBtn = itemInfo.transform.Find("Panel/Group/Close").GetComponent<Button>();

            var btn = itemInfo.transform.Find("Panel/Group/Upgrade").GetComponent<Button>();
            btn.GetComponentInChildren<Text>().text = (oneKey ? "一键" : "") + "升级";
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(async () =>
            {
                changeBtn(false);
                await PropertyLogic.instance.Upgrade(prop, ()=>update(prop));
                changeBtn(true);
            });

            btn = itemInfo.transform.Find("Panel/Group/Employ").GetComponent<Button>();
            btn.GetComponentInChildren<Text>().text = (oneKey ? "一键" : "") + "雇员";
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(async () =>
            {
                changeBtn(false);
                await PropertyLogic.instance.Employ(prop, () => update(prop));
                changeBtn(true);
            });

            btn = itemInfo.transform.Find("Panel/Group/Upstage").GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(async () =>
            {
                changeBtn(false);
                await PropertyLogic.instance.Upstage(prop, ()=>
                {
                    Player.ResetPropertyUid();
                    ShowProperty();
                    CloseInfo();
                });
                changeBtn(true);
            });

            btn = itemInfo.transform.Find("Panel/Group/OneKey").GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(()=>
            {
                oneKey = !oneKey;
                var btn = itemInfo.transform.Find("Panel/Group/Upgrade").GetComponent<Button>();
                btn.GetComponentInChildren<Text>().text = (oneKey ? "一键" : "") + "升级";
                btn = itemInfo.transform.Find("Panel/Group/Employ").GetComponent<Button>();
                btn.GetComponentInChildren<Text>().text = (oneKey ? "一键" : "") + "雇员";
            });

            btn = itemInfo.transform.Find("Panel/Group/Close").GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(CloseInfo);

            itemInfo.transform.DOScale(UnityEngine.Vector3.one, openInfoTime);
        }

        private static void CloseInfo()
        {
            itemInfo.transform.DOScale(UnityEngine.Vector3.zero, closeInfoTime);
        }
    }
}
