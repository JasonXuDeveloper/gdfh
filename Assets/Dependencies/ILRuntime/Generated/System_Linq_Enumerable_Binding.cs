using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class System_Linq_Enumerable_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            MethodBase method;
            Type[] args;
            Type type = typeof(System.Linq.Enumerable);
            Dictionary<string, List<MethodInfo>> genericMethods = new Dictionary<string, List<MethodInfo>>();
            List<MethodInfo> lst = null;                    
            foreach(var m in type.GetMethods())
            {
                if(m.IsGenericMethodDefinition)
                {
                    if (!genericMethods.TryGetValue(m.Name, out lst))
                    {
                        lst = new List<MethodInfo>();
                        genericMethods[m.Name] = lst;
                    }
                    lst.Add(m);
                }
            }
            args = new Type[]{typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance)};
            if (genericMethods.TryGetValue("Count", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Int32), typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Count_0);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance), typeof(System.Int32)};
            if (genericMethods.TryGetValue("OrderByDescending", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Linq.IOrderedEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>), typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>), typeof(System.Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, OrderByDescending_1);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance)};
            if (genericMethods.TryGetValue("ToList", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>), typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ToList_2);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance), typeof(System.Numerics.BigInteger)};
            if (genericMethods.TryGetValue("OrderBy", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Linq.IOrderedEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>), typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>), typeof(System.Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Numerics.BigInteger>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, OrderBy_3);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance), typeof(System.Numerics.BigInteger)};
            if (genericMethods.TryGetValue("OrderByDescending", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Linq.IOrderedEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>), typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>), typeof(System.Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Numerics.BigInteger>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, OrderByDescending_4);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance)};
            if (genericMethods.TryGetValue("Skip", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>), typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>), typeof(System.Int32)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Skip_5);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance)};
            if (genericMethods.TryGetValue("Take", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>), typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>), typeof(System.Int32)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Take_6);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(UnityEngine.Color)};
            if (genericMethods.TryGetValue("Count", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Int32), typeof(System.Collections.Generic.IEnumerable<UnityEngine.Color>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Count_7);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.Object)};
            if (genericMethods.TryGetValue("Contains", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Boolean), typeof(System.Collections.Generic.IEnumerable<System.Object>), typeof(System.Object)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Contains_8);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.Object), typeof(System.String)};
            if (genericMethods.TryGetValue("Select", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.IEnumerable<System.String>), typeof(System.Collections.Generic.IEnumerable<System.Object>), typeof(System.Func<System.Object, System.String>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Select_9);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.Collections.Generic.KeyValuePair<System.String, ILRuntime.Runtime.Intepreter.ILTypeInstance>)};
            if (genericMethods.TryGetValue("ElementAt", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.KeyValuePair<System.String, ILRuntime.Runtime.Intepreter.ILTypeInstance>), typeof(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String, ILRuntime.Runtime.Intepreter.ILTypeInstance>>), typeof(System.Int32)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ElementAt_10);

                        break;
                    }
                }
            }


        }


        static StackObject* Count_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance> @source = (System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.Count<ILRuntime.Runtime.Intepreter.ILTypeInstance>(@source);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* OrderByDescending_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32> @keySelector = (System.Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)typeof(System.Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance> @source = (System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.OrderByDescending<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>(@source, @keySelector);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ToList_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance> @source = (System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.ToList<ILRuntime.Runtime.Intepreter.ILTypeInstance>(@source);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* OrderBy_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Numerics.BigInteger> @keySelector = (System.Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Numerics.BigInteger>)typeof(System.Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Numerics.BigInteger>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance> @source = (System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.OrderBy<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Numerics.BigInteger>(@source, @keySelector);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* OrderByDescending_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Numerics.BigInteger> @keySelector = (System.Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Numerics.BigInteger>)typeof(System.Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Numerics.BigInteger>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance> @source = (System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.OrderByDescending<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Numerics.BigInteger>(@source, @keySelector);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Skip_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @count = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance> @source = (System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.Skip<ILRuntime.Runtime.Intepreter.ILTypeInstance>(@source, @count);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Take_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @count = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance> @source = (System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.Take<ILRuntime.Runtime.Intepreter.ILTypeInstance>(@source, @count);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Count_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<UnityEngine.Color> @source = (System.Collections.Generic.IEnumerable<UnityEngine.Color>)typeof(System.Collections.Generic.IEnumerable<UnityEngine.Color>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.Count<UnityEngine.Color>(@source);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* Contains_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object @value = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.IEnumerable<System.Object> @source = (System.Collections.Generic.IEnumerable<System.Object>)typeof(System.Collections.Generic.IEnumerable<System.Object>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.Contains<System.Object>(@source, @value);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* Select_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Func<System.Object, System.String> @selector = (System.Func<System.Object, System.String>)typeof(System.Func<System.Object, System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.IEnumerable<System.Object> @source = (System.Collections.Generic.IEnumerable<System.Object>)typeof(System.Collections.Generic.IEnumerable<System.Object>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.Select<System.Object, System.String>(@source, @selector);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ElementAt_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String, ILRuntime.Runtime.Intepreter.ILTypeInstance>> @source = (System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String, ILRuntime.Runtime.Intepreter.ILTypeInstance>>)typeof(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String, ILRuntime.Runtime.Intepreter.ILTypeInstance>>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.ElementAt<System.Collections.Generic.KeyValuePair<System.String, ILRuntime.Runtime.Intepreter.ILTypeInstance>>(@source, @index);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
