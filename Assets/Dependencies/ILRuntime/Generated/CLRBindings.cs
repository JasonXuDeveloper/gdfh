using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {

//will auto register in unity
#if UNITY_5_3_OR_NEWER
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
        static private void RegisterBindingAction()
        {
            ILRuntime.Runtime.CLRBinding.CLRBindingUtils.RegisterBindingAction(Initialize);
        }


        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            System_Type_Binding.Register(app);
            ProtoBuf_PType_Binding.Register(app);
            System_Numerics_BigInteger_Binding.Register(app);
            System_Object_Binding.Register(app);
            System_String_Binding.Register(app);
            System_Math_Binding.Register(app);
            System_Text_StringBuilder_Binding.Register(app);
            UnityEngine_GameObject_Binding.Register(app);
            UnityEngine_UI_Text_Binding.Register(app);
            UnityEngine_Component_Binding.Register(app);
            UnityEngine_Vector3_Binding.Register(app);
            DG_Tweening_ShortcutExtensions_Binding.Register(app);
            DG_Tweening_TweenSettingsExtensions_Binding.Register(app);
            UnityEngine_RectTransform_Binding.Register(app);
            UnityEngine_Vector2_Binding.Register(app);
            Ads_Binding.Register(app);
            UnityEngine_Random_Binding.Register(app);
            System_Net_WebClient_Binding.Register(app);
            System_Net_WebHeaderCollection_Binding.Register(app);
            System_Text_Encoding_Binding.Register(app);
            System_ComponentModel_Component_Binding.Register(app);
            System_Func_2_String_Boolean_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncTaskMethodBuilder_1_Boolean_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncTaskMethodBuilder_1_Int32_Binding.Register(app);
            UnityEngine_ColorUtility_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Transform_Text_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            UnityEngine_Transform_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Transform_Outline_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Transform_Button_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Transform_Image_Binding.Register(app);
            System_DateTime_Binding.Register(app);
            System_TimeZone_Binding.Register(app);
            System_TimeSpan_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Sprite_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncVoidMethodBuilder_Binding.Register(app);
            System_Threading_Tasks_TaskCompletionSource_1_Boolean_Binding.Register(app);
            JSONObject_Binding.Register(app);
            System_Collections_Generic_List_1_JSONObject_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_String_Binding.Register(app);
            JEngine_Core_Log_Binding.Register(app);
            System_Threading_Tasks_Task_1_Boolean_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_Boolean_Binding.Register(app);
            System_Action_1_JSONObject_Binding.Register(app);
            UnityEngine_UI_Image_Binding.Register(app);
            UnityEngine_UI_Slider_Binding.Register(app);
            System_Int32_Binding.Register(app);
            UnityEngine_UI_Button_Binding.Register(app);
            UnityEngine_Events_UnityEventBase_Binding.Register(app);
            UnityEngine_Events_UnityEvent_Binding.Register(app);
            UnityEngine_Debug_Binding.Register(app);
            System_Action_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Int32_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Int32_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_Int32_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            MessageBox_Binding.Register(app);
            UnityEngine_UI_ScrollRect_Binding.Register(app);
            UnityEngine_Events_UnityEvent_1_Vector2_Binding.Register(app);
            UnityEngine_Mathf_Binding.Register(app);
            System_Array_Binding.Register(app);
            UnityEngine_Rect_Binding.Register(app);
            System_Collections_Generic_List_1_GameObject_Binding.Register(app);
            System_Linq_Enumerable_Binding.Register(app);
            JEngine_Core_Tools_Binding.Register(app);
            System_Collections_Generic_List_1_Button_Binding.Register(app);
            UnityEngine_UI_Selectable_Binding.Register(app);
            System_Threading_Tasks_Task_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_Binding.Register(app);
            DG_Tweening_DOTweenModuleUI_Binding.Register(app);
            System_Exception_Binding.Register(app);
            System_Collections_IDictionary_Binding.Register(app);
            UnityEngine_UI_Shadow_Binding.Register(app);
            UnityEngine_UI_Graphic_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Int32_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_BigInteger_Binding.Register(app);
            UnityEngine_UI_Dropdown_Binding.Register(app);
            System_Collections_Generic_List_1_String_Binding.Register(app);
            UnityEngine_Events_UnityEvent_1_Int32_Binding.Register(app);
            System_Collections_Generic_List_1_Image_Binding.Register(app);
            JEngine_AntiCheat_JInt_Binding.Register(app);
            System_Collections_Generic_List_1_UnityEngine_UI_Dropdown_Binding_OptionData_Binding.Register(app);
            UnityEngine_UI_Dropdown_Binding_OptionData_Binding.Register(app);
            UnityEngine_Color_Binding.Register(app);
            System_Collections_Generic_List_1_AudioSource_Binding.Register(app);
            UnityEngine_AudioSource_Binding.Register(app);
            UnityEngine_UI_InputField_Binding.Register(app);
            UnityEngine_Application_Binding.Register(app);
            System_Char_Binding.Register(app);
            UnityEngine_Behaviour_Binding.Register(app);
            GalleryLevelSelectionManager_Binding.Register(app);
            System_Collections_Generic_List_1_GalleryLevelView_Binding.Register(app);
            GalleryLevelView_Binding.Register(app);
            System_Threading_Tasks_Task_1_Int32_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_Int32_Binding.Register(app);
            UnityEngine_ILogger_Binding.Register(app);
            UnityEngine_SystemInfo_Binding.Register(app);
            JEngine_Core_BindableProperty_1_Boolean_Binding.Register(app);
            System_Action_2_Int32_Boolean_Binding.Register(app);
            System_Action_2_Int32_List_1_ILTypeInstance_Binding.Register(app);
            Tianti_AppLogger_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Action_2_BigInteger_ILTypeInstance_Binding.Register(app);
            System_Action_2_String_Boolean_Binding.Register(app);
            System_Action_2_BigInteger_ILTypeInstance_Binding.Register(app);
            System_Threading_Tasks_TaskCompletionSource_1_Int32_Binding.Register(app);
            System_Collections_Specialized_NameValueCollection_Binding.Register(app);
            LitJson_JsonMapper_Binding.Register(app);
            System_Guid_Binding.Register(app);
            InitJEngine_Binding.Register(app);
            JEngine_Core_CryptoHelper_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_List_1_Int32_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncTaskMethodBuilder_Binding.Register(app);
            DG_Tweening_TweenExtensions_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_List_1_ILTypeInstance_Binding.Register(app);
            UnityEngine_PlayerPrefs_Binding.Register(app);
            JEngine_Core_Loom_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            UnityEngine_TextAsset_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding.Register(app);
            System_Text_RegularExpressions_Regex_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int64_BigInteger_Binding.Register(app);
            JEngine_Core_BindableProperty_1_Int32_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int64_Int32_Binding.Register(app);
            System_Nullable_1_Int32_Binding.Register(app);
            System_Nullable_1_Int64_Binding.Register(app);
            System_Nullable_1_Boolean_Binding.Register(app);
            System_Action_2_ILTypeInstance_Object_Binding.Register(app);
            System_Action_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_UIBehaviour_Binding.Register(app);
            JEngine_Net_SocketIOComponent_Binding.Register(app);
            JEngine_Net_JSocketConfig_Binding.Register(app);
            WebSocketSharp_WebSocket_Binding.Register(app);
            System_Action_1_SocketIOEvent_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Object_List_1_ValueTuple_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Object_List_1_ValueTuple_2_String_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Object_List_1_ValueTuple_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_List_1_ValueTuple_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_List_1_Object_Binding.Register(app);
            System_ValueTuple_2_String_ILTypeInstance_Binding.Register(app);
            System_Int16_Binding.Register(app);
            System_Int64_Binding.Register(app);
            System_Decimal_Binding.Register(app);
            System_Double_Binding.Register(app);
            System_Single_Binding.Register(app);
            System_Boolean_Binding.Register(app);
            System_Activator_Binding.Register(app);
            JEngine_Core_ClassData_Binding.Register(app);
            JEngine_Core_ClassBind_Binding.Register(app);
            System_Threading_CancellationTokenSource_Binding.Register(app);
            UnityEngine_MonoBehaviour_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_ILTypeInstance_Binding.Register(app);
            System_NotSupportedException_Binding.Register(app);
            System_Diagnostics_Stopwatch_Binding.Register(app);
            JEngine_Core_GameStats_Binding.Register(app);
            libx_Assets_Binding.Register(app);
            libx_AssetRequest_Binding.Register(app);
            System_Collections_Generic_List_1_Action_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Single_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Func_1_Boolean_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Action_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncTaskMethodBuilder_1_CoroutineAdapter_Binding_Adaptor_Binding.Register(app);
            System_Collections_Generic_List_1_CoroutineAdapter_Binding_Adaptor_Binding.Register(app);
            System_GC_Binding.Register(app);
            System_Func_1_Boolean_Binding.Register(app);
            System_Threading_Tasks_Task_1_CoroutineAdapter_Binding_Adaptor_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_CoroutineAdapter_Binding_Adaptor_Binding.Register(app);
            System_TimeoutException_Binding.Register(app);
            libx_Reference_Binding.Register(app);
            System_Action_2_Boolean_CoroutineAdapter_Binding_Adaptor_Binding.Register(app);
            JEngine_AntiCheat_AntiCheatHelper_Binding.Register(app);
            Version_Binding.Register(app);
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
        }
    }
}
