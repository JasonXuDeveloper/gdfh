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
    unsafe class GalleryLevelView_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::GalleryLevelView);

            field = type.GetField("manager", flag);
            app.RegisterCLRFieldGetter(field, get_manager_0);
            app.RegisterCLRFieldSetter(field, set_manager_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_manager_0, AssignFromStack_manager_0);
            field = type.GetField("levelName", flag);
            app.RegisterCLRFieldGetter(field, get_levelName_1);
            app.RegisterCLRFieldSetter(field, set_levelName_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_levelName_1, AssignFromStack_levelName_1);


        }



        static object get_manager_0(ref object o)
        {
            return ((global::GalleryLevelView)o).manager;
        }

        static StackObject* CopyToStack_manager_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::GalleryLevelView)o).manager;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_manager_0(ref object o, object v)
        {
            ((global::GalleryLevelView)o).manager = (global::GalleryLevelSelectionManager)v;
        }

        static StackObject* AssignFromStack_manager_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::GalleryLevelSelectionManager @manager = (global::GalleryLevelSelectionManager)typeof(global::GalleryLevelSelectionManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::GalleryLevelView)o).manager = @manager;
            return ptr_of_this_method;
        }

        static object get_levelName_1(ref object o)
        {
            return ((global::GalleryLevelView)o).levelName;
        }

        static StackObject* CopyToStack_levelName_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::GalleryLevelView)o).levelName;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_levelName_1(ref object o, object v)
        {
            ((global::GalleryLevelView)o).levelName = (System.String)v;
        }

        static StackObject* AssignFromStack_levelName_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @levelName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::GalleryLevelView)o).levelName = @levelName;
            return ptr_of_this_method;
        }



    }
}
