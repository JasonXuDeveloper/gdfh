//
// LoginPanel.cs
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
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using JEngine.Core;
using HotUpdateScripts;
using Game.Data;
using Game.Mgr;
using Game.Util;
using Game.Logic;

namespace Game.View
{
    public class LoginPanel : MonoBehaviour
    {
        private static GameObject s_loginPanelPrefab;
        private static GameObject panelObj;

        public static bool Active => panelObj.activeSelf;

        public InputField usr;
        public InputField pwd;

        public Button login;
        public Button reg;

        // 显示界面
        public static void Show()
        {
            if (panelObj != null && !panelObj.activeSelf)
            {
                panelObj.SetActive(true);
            }
            else if (panelObj == null)
            {
                Init();
                Show();
            }
        }

        // 隐藏界面
        public static void Hide()
        {
            if (panelObj != null && panelObj.activeSelf)
            {
                panelObj.SetActive(false);
            }
        }

        // 初始化界面
        public static void Init()
        {
            if (panelObj != null)
            {
                return;
            }

            if (null == s_loginPanelPrefab)
                s_loginPanelPrefab = JResource.LoadRes<GameObject>("LoginPanel.prefab");
            panelObj = Instantiate(s_loginPanelPrefab);
            panelObj.SetActive(false);
            panelObj.transform.SetParent(GlobalObj.s_canvasTrans, false);
        }

        private void Awake()
        {
            async void req(bool login)
            {
                var usr = this.usr.text;
                var pwd = this.pwd.text;
                //校验器
                if (!Validator.Validate(usr, minLength: 3, maxLength: 10) || !Validator.Validate(pwd, minLength: 3, maxLength: 10))
                    return;
                //请求登录的时候禁用按钮
                this.login.interactable = false;
                this.reg.interactable = false;
                //没成功按钮可以继续点
                //if (!await GameManager.Instance.ReqPlayer(usr, pwd, login))
                //{
                //    this.login.interactable = true;
                //    this.reg.interactable = true;
                //}
                //TODO 取消注释上面的，删除下面的
                var b = await SettingLogic.GetFromCloud(usr, pwd);

                GameManager.Instance.LoggedIn = true;
                LoginPanel.Hide();
                if (!b)
                {
                    Player.Instance().id = SystemInfo.deviceUniqueIdentifier;
                    Player.Instance().name = usr;
                    Player.Instance().password = pwd;
                    Player.Save();
                }
            }
            this.login.onClick.AddListener(() => req(true));
            this.reg.onClick.AddListener(() => req(false));
        }

        private void OnDestroy()
        {
            UnityEngine.Debug.unityLogger.logEnabled = true;
        }
    }
}