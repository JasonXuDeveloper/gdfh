//
// Info.cs
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
using UnityEngine;
using DG.Tweening;
using HotUpdateScripts;
using DG.Tweening.Core;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Game.Util
{
    public class Info
    {
        private static float openTime = 0.4f;
        private static float closeTime = 0.35f;
        private static float waitTime = 1.25f;

        public static void D(string text)
        {
            var go = GlobalObj.info.PoolObject;
            var rect = go.transform.GetComponent<RectTransform>();
            go.GetComponentInChildren<Text>().text = text;
            go.gameObject.SetActive(true);
            go.transform.DOScale(Vector3.one, openTime)
            .OnComplete(() =>
            {
                rect.DOLocalMoveY(rect.anchoredPosition.y + 350, waitTime)
                .OnComplete(() =>
                {
                    go.transform.DOScale(Vector3.zero, closeTime)
                    .OnComplete(() =>
                    {
                        go.gameObject.SetActive(false);
                        rect.DOLocalMoveY(rect.anchoredPosition.y - 350, 0.1f);
                    });
                });
            });
        }
    }
}
