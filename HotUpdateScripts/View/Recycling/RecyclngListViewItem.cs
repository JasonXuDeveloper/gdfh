//
// RecyclngListViewItem.cs
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

namespace Game.View
{
    /// <summary>
    /// 列表item，你自己写的列表item需要继承该类
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class RecyclingListViewItem : MonoBehaviour
    {

        private RecyclingListView parentList;

        /// <summary>
        /// 循环列表
        /// </summary>
        public RecyclingListView ParentList
        {
            get => parentList;
        }

        private int currentRow;
        /// <summary>
        /// 行号
        /// </summary>
        public int CurrentRow
        {
            get => currentRow;
        }

        private RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = gameObject.GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }

        public void UpdateRect()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
        }

        public void UpdatePos(Vector2 pos)
        {
            rectTransform.anchoredPosition = pos;
        }

        private void Awake()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
        }

        /// <summary>
        /// item更新事件响应函数
        /// </summary>
        public virtual void NotifyCurrentAssignment(RecyclingListView v, int row)
        {
            parentList = v;
            currentRow = row;
        }


    }
}
