//
// SpriteManager.cs
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
using JEngine.Core;

namespace Game.Mgr
{
    public class SpriteManager
    {
        /// <summary>
        /// 获取图片精灵
        /// </summary>
        public Sprite GetSprite(string name)
        {
            if (m_sprites.ContainsKey(name))
                return m_sprites[name];
            var sprite = JResource.LoadRes<Sprite>("Sprites/" + name + ".png");
            m_sprites.Add(name, sprite);
            return sprite;
        }


        private Dictionary<string, Sprite> m_sprites = new Dictionary<string, Sprite>();
        private static SpriteManager s_instance;
        public static SpriteManager instance
        {
            get
            {
                if (null == s_instance)
                    s_instance = new SpriteManager();
                return s_instance;
            }
        }

    }
}
