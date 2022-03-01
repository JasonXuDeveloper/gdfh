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
    unsafe class GalleryLevelSelectionManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::GalleryLevelSelectionManager);
            args = new Type[]{};
            method = type.GetMethod("Start", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Start_0);

            field = type.GetField("itemsContainer", flag);
            app.RegisterCLRFieldGetter(field, get_itemsContainer_0);
            app.RegisterCLRFieldSetter(field, set_itemsContainer_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_itemsContainer_0, AssignFromStack_itemsContainer_0);
            field = type.GetField("items", flag);
            app.RegisterCLRFieldGetter(field, get_items_1);
            app.RegisterCLRFieldSetter(field, set_items_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_items_1, AssignFromStack_items_1);


        }


        static StackObject* Start_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::GalleryLevelSelectionManager instance_of_this_method = (global::GalleryLevelSelectionManager)typeof(global::GalleryLevelSelectionManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Start();

            return __ret;
        }


        static object get_itemsContainer_0(ref object o)
        {
            return ((global::GalleryLevelSelectionManager)o).itemsContainer;
        }

        static StackObject* CopyToStack_itemsContainer_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::GalleryLevelSelectionManager)o).itemsContainer;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_itemsContainer_0(ref object o, object v)
        {
            ((global::GalleryLevelSelectionManager)o).itemsContainer = (UnityEngine.Transform)v;
        }

        static StackObject* AssignFromStack_itemsContainer_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Transform @itemsContainer = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::GalleryLevelSelectionManager)o).itemsContainer = @itemsContainer;
            return ptr_of_this_method;
        }

        static object get_items_1(ref object o)
        {
            return ((global::GalleryLevelSelectionManager)o).items;
        }

        static StackObject* CopyToStack_items_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::GalleryLevelSelectionManager)o).items;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_items_1(ref object o, object v)
        {
            ((global::GalleryLevelSelectionManager)o).items = (System.Collections.Generic.List<global::GalleryLevelView>)v;
        }

        static StackObject* AssignFromStack_items_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<global::GalleryLevelView> @items = (System.Collections.Generic.List<global::GalleryLevelView>)typeof(System.Collections.Generic.List<global::GalleryLevelView>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::GalleryLevelSelectionManager)o).items = @items;
            return ptr_of_this_method;
        }



    }
}
