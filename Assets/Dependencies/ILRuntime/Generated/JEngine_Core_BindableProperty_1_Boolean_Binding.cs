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
    unsafe class JEngine_Core_BindableProperty_1_Boolean_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(JEngine.Core.BindableProperty<System.Boolean>);
            args = new Type[]{typeof(JEngine.Core.BindableProperty<System.Boolean>)};
            method = type.GetMethod("op_Implicit", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_Implicit_0);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_Value", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_Value_1);
            args = new Type[]{};
            method = type.GetMethod("get_Value", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Value_2);

            field = type.GetField("OnChange", flag);
            app.RegisterCLRFieldGetter(field, get_OnChange_0);
            app.RegisterCLRFieldSetter(field, set_OnChange_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnChange_0, AssignFromStack_OnChange_0);

            args = new Type[]{typeof(System.Boolean)};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }


        static StackObject* op_Implicit_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            JEngine.Core.BindableProperty<System.Boolean> @t = (JEngine.Core.BindableProperty<System.Boolean>)typeof(JEngine.Core.BindableProperty<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = (System.Boolean)t;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* set_Value_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            JEngine.Core.BindableProperty<System.Boolean> instance_of_this_method = (JEngine.Core.BindableProperty<System.Boolean>)typeof(JEngine.Core.BindableProperty<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Value = value;

            return __ret;
        }

        static StackObject* get_Value_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            JEngine.Core.BindableProperty<System.Boolean> instance_of_this_method = (JEngine.Core.BindableProperty<System.Boolean>)typeof(JEngine.Core.BindableProperty<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.Value;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }


        static object get_OnChange_0(ref object o)
        {
            return ((JEngine.Core.BindableProperty<System.Boolean>)o).OnChange;
        }

        static StackObject* CopyToStack_OnChange_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((JEngine.Core.BindableProperty<System.Boolean>)o).OnChange;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnChange_0(ref object o, object v)
        {
            ((JEngine.Core.BindableProperty<System.Boolean>)o).OnChange = (JEngine.Core.BindableProperty<System.Boolean>.onChange)v;
        }

        static StackObject* AssignFromStack_OnChange_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            JEngine.Core.BindableProperty<System.Boolean>.onChange @OnChange = (JEngine.Core.BindableProperty<System.Boolean>.onChange)typeof(JEngine.Core.BindableProperty<System.Boolean>.onChange).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            ((JEngine.Core.BindableProperty<System.Boolean>)o).OnChange = @OnChange;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @val = ptr_of_this_method->Value == 1;


            var result_of_this_method = new JEngine.Core.BindableProperty<System.Boolean>(@val);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
