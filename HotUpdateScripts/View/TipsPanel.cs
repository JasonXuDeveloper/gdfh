//
// TipsPanel.cs
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
using UnityEngine;
using UnityEngine.UI;
using System;
using HotUpdateScripts;
using Game.Logic;
using Game.Data;
using JEngine.Core;
using libx;
using Game.Util;

namespace Game.View
{
    public class TipsPanel
    {
        // 显示任务界面
        public static void Show(int chainId, int subId, Action cb)
        {
            var task = TaskCfg.instance.GetCfgItem(chainId, subId);
            MessageBox.Show(task.desc, task.task_target,"前往").onComplete += id =>
            {
                if (id != MessageBox.EventId.Ok)
                    return;
                //跳转界面
                Jump.To(task.jump_to);
                cb();
            };
        }
    }

}
