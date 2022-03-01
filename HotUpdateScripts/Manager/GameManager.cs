//
// GameManager.cs
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
using System.Threading;
using System.Threading.Tasks;
using Game.Data;
using Game.Util;
using Game.View;
using JEngine.Core;
using JEngine.Net;
namespace Game.Mgr
{
    public class GameManager
    {
        public static GameManager Instance
        {
            get
            {
                if (_instance == null) _instance = new GameManager();
                return _instance;
            }
        }
        private static GameManager _instance;

        public JWebSocket Socket;
        public bool Connected => Socket != null && Socket.Connected;
        public bool LoggedIn;

        private readonly JSONObject empty = new JSONObject();
        private readonly JSONObject errMsg = new JSONObject(dic: new System.Collections.Generic.Dictionary<string, string>()
        {
            {"msg", "系统错误，请稍后重试"}
        });

        private string tempUsr;
        private string tempPwd;

        public static void Init(string ip,int port)
        {
            Instance.Socket = new JWebSocket($"ws://{ip}:{port}/socket.io/?EIO=3&transport=websocket");

            Instance.Socket.OnConnect(e =>
            {
                Info.D("成功与服务器建立连接");
            });

            Instance.Socket.OnDisconnect((e) =>
            {
                Loading.Start("正在重连服务器");
            });

            //连接服务器
            Instance.Socket.Connect();

            //断线重连事件
            Instance.Socket.OnReconnect(async () =>
            {
                Loading.Start("正在重新登录");
                if (Instance.LoggedIn)
                {
                    Instance.LoggedIn = false;
                    //重新登录
                    var ret = await Instance.ReqPlayer(Instance.tempUsr, Instance.tempPwd);
                    if (!ret)
                    {
                        LoginPanel.Show();
                    }
                }
                Loading.Finish();
            });
        }

        /// <summary>
        /// 请求获取角色-登录/注册
        /// </summary>
        /// <param name="usr"></param>
        /// <param name="pwd"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<bool> ReqPlayer(string usr, string pwd, bool login = true)
        {
            JSONObject msg = new JSONObject();
            msg.AddField("usr", usr);
            msg.AddField("pwd", pwd);
            msg.AddField("login", login);

            return await SendReq("req_player", msg, resp =>
            {
                //缓存
                tempUsr = usr;
                tempPwd = pwd;

                //同步玩家数据
                var playerData = resp["data"];
                Log.Print(resp);
                Info.D(resp["msg"].str);
                Player p = StringifyHelper.JSONDeSerliaze<Player>(playerData.ToString());
                Player.Instance().id = p.id;

                //更新状态
                LoggedIn = true;
                LoginPanel.Hide();
            }, resp =>
            {
                //显示错误
                Info.D(resp["msg"].str);
            });
        }

        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="usr"></param>
        /// <param name="pwd"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public async void Heartbeat()
        {
            if (Instance.LoggedIn == false)
            {
                return;
            }
            JSONObject msg = new JSONObject();
            Log.PrintWarning(Player.Instance().id);
            msg.AddField("id", Player.Instance().id);

            await SendReq("heartbeat", msg, resp =>
            {
                Log.PrintWarning("心跳处理成功");
            }, resp =>
            {
                //显示错误
                Log.PrintError(resp["msg"].str);
            });
        }

        private Task<bool> SendReq(string name, JSONObject msg,Action<JSONObject> suc,Action<JSONObject> err)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            Socket.EmitToSocketIOServer(name, msg, data =>
            {
                var resp = GetData(data);
                if ((int)resp["code"].n == 200)
                {
                    suc?.Invoke(resp);
                    tcs.SetResult(true);
                }
                else
                {
                    err?.Invoke(resp);
                    tcs.SetResult(false);
                }
            });
            return tcs.Task.TimeoutAfter(TimeSpan.FromSeconds(5),()=>
            {
                err?.Invoke(errMsg);
                tcs.SetResult(false);
            });
        }

        private JSONObject GetData(JSONObject data)
        {
            return data.list[0];
        }
    }
}
