//
// Property.cs
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
using System.Numerics;
using JEngine.AntiCheat;
using JEngine.Core;
using LitJson;

namespace Game.Data
{
    public class Property
    {
        public JInt id;
        public JInt level = 1;//等级
        public JInt grade = 0;//等阶
        public JInt employee = 1;//员工数量

        public BindableProperty<int> propProgress = new BindableProperty<int>(0);
        [JsonIgnore] public int uid = -1;

        [JsonIgnore]
        private PropertyCfgItem item
        {
            get
            {
                if (_item == null)
                {
                    _item = PropertyCfg.GetProperty(id);
                }
                return _item;
            }
        }

        [JsonIgnore] private PropertyCfgItem _item;
        [JsonIgnore] public string name => item.name;
        [JsonIgnore] public BigInteger baseIncome => item.baseIncome;
        //收益时长 = 基础 * (等级/10 + 1) / （1 + 员工数量 / 5）
        //每10级提升1倍时长
        [JsonIgnore]
        public int duration
        {
            get
            {
                //id level grade employee
                //long key = id*1000000000 + level * 1000000 + grade * 1000 + employee;
                //if (durationCache.TryGetValue(key,out var val))
                //{
                //    return val;
                //}
                int val = item.duration;
                val -= val * (employee % 100 == 0 && employee / 100 != grade ? 100 : employee % 100) / 250;//员工最多减少40%基础消耗
                val += val * (level % 100 == 0 ? 100 : level % 100) / 500;//升级最多增加20%
                if (val < 1)
                {
                    val = 1;
                }
                //durationCache.Add(key, val);
                return val;
            }
        }
        [JsonIgnore] public PropertyType type => item.type;

        [JsonIgnore]
        public BigInteger Income
        // 等阶表[等阶] * 等级 * 基础收益 * (1 + 员工 / 20) = 总收益
        {
            get
            {
                BigInteger val = item.baseIncome;
                float t = (GradeBenefit[grade < GradeBenefit.Length ? grade : GradeBenefit.Length - 1] * (level + level / 4)
                    * (1 + (employee * 0.05f)));
                val *= (BigInteger)t;
                return val;
            }
        }

        //目前最高8阶
        [JsonIgnore]
        public static int[] GradeBenefit = new int[] {
            1,2,4,8,16,32,64,256,512,1024
        };

        [JsonIgnore]
        public BigInteger EmployFee
        {
            get
            {
                var ret = (BigInteger)((employee * 0.8f) * (int)item.baseIncome / 4 * (int)item.baseIncome / (int)item.duration) * GradeBenefit[grade];
                switch (type)
                {
                    case PropertyType.餐厅:
                        ret += item.duration * item.baseIncome;
                        break;
                }
                if (grade > 1)
                {
                    ret *= GradeBenefit[grade - 1] / 2;
                }
                return ret;
            }
        }
        [JsonIgnore]
        public BigInteger UpgradeFee
        {
            get
            {
                var ret = (int)level * item.baseIncome / 4 * item.baseIncome / (int)item.duration * GradeBenefit[grade];
                switch (type)
                {
                    case PropertyType.餐厅:
                        ret += item.duration * item.baseIncome /2;
                        break;
                    case PropertyType.汽车:
                        ret -= ret / 8;
                        break;
                    case PropertyType.计算机科技:
                        ret -= ret / 4;
                        break;
                }
                if (grade > 1)
                {
                    ret *= GradeBenefit[grade - 1] / 2;
                }
                return ret;
            }
        }
        [JsonIgnore]
        public BigInteger UpstageFee
        {
            get
            {
                BigInteger baseIn = item.baseIncome;

                for (int i = 0; i < grade; i++)
                {
                    switch (type)
                    {
                        case PropertyType.餐厅:
                            baseIn *= (int)grade * 2 * item.baseIncome / item.duration;
                            break;
                        case PropertyType.汽车:
                            baseIn *= (int)grade * 4 / 2;
                            break;
                        case PropertyType.计算机科技:
                            baseIn *= (int)grade * 4 / 3;
                            break;
                    }
                }
                return baseIn * item.duration * GradeBenefit[grade];
            }
        }
    }

    public enum PropertyType
    {
        餐厅,
        汽车,
        计算机科技
    }
}
