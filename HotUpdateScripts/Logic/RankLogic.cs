//
// RankLogic.cs
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
using System.Threading.Tasks;
using Game.Data;
using UnityEngine;
using System.Net;
using System.Collections.Specialized;
using JEngine.Core;
using System.Text;
using Game.Util;
using LitJson;
using System.Linq;
using System.Numerics;

namespace Game.Logic
{
    public class RankLogic
    {
        public string apiUrl = HotUpdateScripts.GlobalObj.IsDebug ? "127.0.0.1:9999" : "127.0.0.1:9966";

        public Task<int> GetRank(int page,int size=50,bool all = false)
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            //开源版无服务端，直接返回0
            tcs.SetResult(0);
            return tcs.Task;

            string url = $"http://{apiUrl}/Rank/Get";
            var data = new NameValueCollection();
            data["id"] = Player.Instance().id;
            data["page"] = page.ToString();
            data["count"] = size.ToString();
            if (!all)
            {
                data["server"] = Player.Instance().server.ToString();
            }
            var result = "";
            Task.Run(() =>
            {
                result = Web.Post(url, data);
                Debug.Log(result);
                JSONObject j = new JSONObject(result);
                List<RankItem> rs = JsonMapper.ToObject<List<RankItem>>(j["data"].ToString());
                Ranking.items = rs;
                Ranking.playerRank = (int)j["rank"].n;
                Debug.Log($"玩家排名：{Ranking.playerRank}");
                tcs.SetResult((int)j["total"].n);
            });
            return tcs.Task;
        }

        public async void UpdateRank()
        {
            if (string.IsNullOrEmpty(Player.Instance().name))
            {
                Player.Instance().name = $"玩家{System.Guid.NewGuid().ToString("N").Substring(0, 5)}";
                Player.Save();
            }
            //开源版不用服务端
            return;
            string url = $"http://{apiUrl}/Rank/Set";
            var data = new NameValueCollection();
            data["name"] = Player.Instance().name;
            data["id"] = Player.Instance().id;
            data["server"] = Player.Instance().server.ToString();
            data["income"] = CryptoHelper.EncryptStr(Player.Instance().TotalAvgIncome.ToString(), InitJEngine.Instance.key);
            var result = "";
            await Task.Run(() =>
            {
                result = Web.Post(url, data);
            });
            Debug.Log($"添加排名结果({Player.Instance().name})：{result}");
            if (result.StartsWith("suc"))
            {
                var server = int.Parse(result.Split(':')[1]);
                if (HotUpdateScripts.GlobalObj.IsDebug)
                {
                    server = 0;
                }
                Player.Instance().server = server;
                Player.Save();
            }
            await SettingLogic.SaveToCloud();
        }

        private static RankLogic s_instance;
        public static RankLogic instance
        {
            get
            {
                if (null == s_instance)
                    s_instance = new RankLogic();
                return s_instance;
            }
        }
    }
}
