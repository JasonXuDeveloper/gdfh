//
// SettingLogic.cs
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
using System.Collections.Specialized;
using System.Threading.Tasks;
using Game.Data;
using Game.Util;
using Game.View;
using JEngine.Core;
using UnityEngine;

namespace Game.Logic
{
    public class SettingLogic
    {
        public static string apiUrl => RankLogic.instance.apiUrl;

        public static Task<bool> SaveToCloud()
        {
            Player.CleanName();
            string playerData = PlayerPrefs.GetString("PLAYER_DATA");
            string taskData = PlayerPrefs.GetString("TASK_DATA");
            string propData = PlayerPrefs.GetString("PROP_DATA");

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            //开源不包括服务端，直接return了
            tcs.SetResult(true);
            return tcs.Task;

            string url = $"http://{apiUrl}/Storage/Set";
            var data = new NameValueCollection();
            data["player"] = playerData;
            data["task"] = taskData;
            data["prop"] = propData;
            data["name"] = Player.Instance().name;
            data["password"] = Player.Instance().password;
            data["server"] = Player.Instance().server.ToString();
            data["id"] = Player.Instance().id;
            var result = "";
            Task.Run(() =>
            {
                result = Web.Post(url, data);
                Debug.LogError(result);
                Loom.QueueOnMainThread((o) =>
                {
                    if (result == "suc")
                    {
                        Info.D("存档成功");
                    }
                    else
                    {
                        Info.D("存档失败");
                    }
                }, null);
                tcs.SetResult(result == "suc");
            });
            return tcs.Task;
        }

        public static Task<bool> GetFromCloud(string usr,string pwd, bool force=true)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            //开源了不带服务端代码，这块直接return
            tcs.SetResult(false);
            return tcs.Task;


            string url = $"http://{apiUrl}/Storage/Get";
            var data = new NameValueCollection();
            data["name"] = usr;
            data["password"] = pwd;
            var result = "";
            Task.Run(() =>
            {
                Loom.QueueOnMainThread(o =>
                {
                    Loading.Start("正在请求数据");
                }, null);
                result = Web.Post(url, data);

                Loom.QueueOnMainThread(o =>
                {
                    if (result != "[]")
                    {
                        try
                        {
                            var encryptKey = InitJEngine.Instance.key;
                            result = CryptoHelper.DecryptStr(result, encryptKey);
                            Debug.LogError(result);
                            Loading.Start("正在加载玩家数据");

                            var dt = LitJson.JsonMapper.ToObject<CloudDataStruct>(result);
                            var p = dt.player;
                            var pl = LitJson.JsonMapper.ToObject<Player>(p.ToString());

                            Player.LogOut();
                            Player.Save(pl);
                            Player.CleanName();
                            Player.CleanData();
                            JSaver.SaveAsString("TASK_DATA", dt.task);
                            JSaver.SaveAsString("PROP_DATA", dt.prop);
                            Loading.Finish();

                            tcs.SetResult(true);
                        }
                        catch
                        {
                            tcs.SetResult(false);
                        }
                    }
                    else
                    {
                        if (!force)
                        {
                            if (!Player.Instance().name.IsNullOrEmpty())
                            {
                                tcs.SetResult(false);
                                return;
                            }
                        }
                        Player.LogOut();
                        Player.Save(new Player());
                        JSaver.SaveAsString("PROP_DATA", "[]");
                        JSaver.SaveAsString("TASK_DATA", "[{'task_chain_id':1,'task_sub_id':1,'progress':0,'award_is_get':0}]");
                        tcs.SetResult(false);
                    }
                }, null);
            });
            return tcs.Task;
        }

        public class CloudDataStruct
        {
            public string player;
            public string task;
            public string prop;
        }
    }
}
