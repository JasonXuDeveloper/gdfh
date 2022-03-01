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
    unsafe class Version_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::Version);

            field = type.GetField("versionName", flag);
            app.RegisterCLRFieldGetter(field, get_versionName_0);
            app.RegisterCLRFieldSetter(field, set_versionName_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_versionName_0, AssignFromStack_versionName_0);
            field = type.GetField("forceUpdate", flag);
            app.RegisterCLRFieldGetter(field, get_forceUpdate_1);
            app.RegisterCLRFieldSetter(field, set_forceUpdate_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_forceUpdate_1, AssignFromStack_forceUpdate_1);
            field = type.GetField("content", flag);
            app.RegisterCLRFieldGetter(field, get_content_2);
            app.RegisterCLRFieldSetter(field, set_content_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_content_2, AssignFromStack_content_2);


        }



        static object get_versionName_0(ref object o)
        {
            return ((global::Version)o).versionName;
        }

        static StackObject* CopyToStack_versionName_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::Version)o).versionName;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_versionName_0(ref object o, object v)
        {
            ((global::Version)o).versionName = (System.String)v;
        }

        static StackObject* AssignFromStack_versionName_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @versionName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::Version)o).versionName = @versionName;
            return ptr_of_this_method;
        }

        static object get_forceUpdate_1(ref object o)
        {
            return ((global::Version)o).forceUpdate;
        }

        static StackObject* CopyToStack_forceUpdate_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::Version)o).forceUpdate;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_forceUpdate_1(ref object o, object v)
        {
            ((global::Version)o).forceUpdate = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_forceUpdate_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @forceUpdate = ptr_of_this_method->Value == 1;
            ((global::Version)o).forceUpdate = @forceUpdate;
            return ptr_of_this_method;
        }

        static object get_content_2(ref object o)
        {
            return ((global::Version)o).content;
        }

        static StackObject* CopyToStack_content_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::Version)o).content;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_content_2(ref object o, object v)
        {
            ((global::Version)o).content = (System.String)v;
        }

        static StackObject* AssignFromStack_content_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @content = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::Version)o).content = @content;
            return ptr_of_this_method;
        }



    }
}
