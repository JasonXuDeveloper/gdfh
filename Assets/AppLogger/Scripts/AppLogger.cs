using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Tianti
{
	public enum DEBUG_LOG_MODE {
		OFF = 0, // 不输出调试信息
		LOGCAT,  // 用于联机调试 android下输出到logcat ios下输出到console
		FILE,    // 输出到 文件（applogger.log）, 用于脱机调试
		ALL      // 输出到 logcat/console 和 文件（applogger.log）
	}

	public enum DEBUG_LOG_LEVEL {
		VERBOSE = 0, // 输出所有级别调试信息
		INFO,        // 输出INFO级别以上调试信息
		WARN,        // 输出WARN级别以上调试信息
		ERROR        // 输出ERROR级别以上调试信息
	}

	public class AppLogger
	{
		public static string STAGE_NULL    = "@null";     // 空，没有关卡/页面
		public static string STAGE_MAX     = "@max";      // 最大关卡
		public static string STAGE_LAST    = "@last";     // 最后关卡，缺省
		public static string STAGE_CURRENT = "@current";  // 当前关卡或页面

		public static bool   bInited = false;

#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR

		#if UNITY_ANDROID
		private static AndroidJavaClass tt_cls = null;
		#endif

		/// //////////////////////////////////////////
		/// sdk初始化接口,在游戏启动脚本中添加
		/// //////////////////////////////////////////
		public static void init (string appKey, string appChannel = null)
		{
			if (bInited) {
				Debug.Log ("AppLogger.init不可以被多次调用，请检查代码");
				return;
			}
			bInited = true;

	#if UNITY_IPHONE
			logger_init (appKey, appChannel);
  	#elif UNITY_ANDROID
			AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject obj_context = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			tt_cls = new AndroidJavaClass ("com.tianti.AppLogger");
			tt_cls.CallStatic ("init", obj_context, appKey, appChannel);

			GameObject gameObj = new GameObject();
			gameObj.AddComponent<AppLoggerBehaviour>();
			gameObj.name = "AppLoggerBehaviour";
  	#endif
		}

		/// ///////////////////////////////////////////////////////////
		/// 设置接口，调用顺序上可以任意，不依赖与AppLogger.init(...)
		/// ///////////////////////////////////////////////////////////

		//调试模式
		public static void setDebugLog (DEBUG_LOG_MODE mode, DEBUG_LOG_LEVEL level) { logger_setDebugLog((int)mode,(int)level); }

		//设置session间隔 单位秒，android下默认30秒，iOS下默认0秒
		public static void setSessionInterval (int v) { logger_setSessionInterval(v); }

		//是否发送统计数据，不调用此接口时默认发送状态。
    	//可用作过滤非登录启动：
    	//AppLogger.init调用前后，可调用AppLogger.setOnline(false),此时数据不发送；
    	//等登录成功后，调用AppLogger.setOnline(true),即可发送统计数据
    	public static void setOnline (bool bOnline) { logger_setOnline(bOnline); }

		//开启崩溃统计
		public static void enableCrashReport () 
		{
			CrashHandler.Init ();
			logger_enableCrashReport();
		}
		//开启在线参数功能，不调用此接口，无法更新统计后台配置的在线参数列表
		public static void enableOnlineConfig () { logger_enableOnlineConfig(); }

		/// //////////////////////////////////////////
		/// 接口名称: getOnlineConfig
		/// 功   能: 根据在线参数后台配置的参数名称，来获取对应的参数值
		/// (必须在AppLogger.init(...)后使用)        
		/// //////////////////////////////////////////
		public static string getOnlineConfig(string param, string def="") {
    #if UNITY_IPHONE
			int nLen = 0;
			string blank = "";
			string value = logger_getParamValuePTR(param, ref nLen, null);
			if (nLen < 0) return def;
			else if (nLen == 0) return blank;
			return value ?? def;
    #elif UNITY_ANDROID
			string value = "";
			value = tt_cls.CallStatic<string> ("getOnlineConfig", param, def);
			return value;
    #else 
			return null;
    #endif
		}

		//android下自动调用，无需手动调用
    #if UNITY_ANDROID
		public static void onStart () { logger_onStart (); }
		public static void onEnd   () { logger_onEnd ();   }
		public static void onExit  () { logger_onExit ();  }
    #endif

		/// //////////////////////////////////////////
		/// 事件接口
		/// //////////////////////////////////////////
		public static void onSubStart (string stage) { logger_onSubStart (stage); }
		public static void onSubEnd   (string stage) { logger_onSubEnd (stage); }
		public static void onPassFail (bool bPass)   { logger_onPassFail (bPass); }
		public static void onEvent    (string name, string stage = "@last") { logger_onEvent (name, stage); }
		public static void onBalance  (string name, string item, int value, string stage = "@last") { logger_onBalance(name, item, value, stage); }
		public static void onLevelUp  (string name, string oldLevel, string newLevel, string stage = "@last") { logger_onLevelUp(name, oldLevel, newLevel, stage); }
		public static void onBuy      (string service, string item, int count, float value, string stage = "@last") { logger_onBuy (service, item, count, value, stage); }
		public static void onUse      (string item, int count, string stage = "@last") { logger_onUse (item, count, stage); }
		public static void onExchange (string item, int count, string stage = "@last") { logger_onExchange (item, count, stage); }
		public static void onCollect  (string item, int count, string stage = "@last") { logger_onCollect (item, count, stage); }
		public static void onReward   (string item, int count, string stage = "@last") { logger_onReward (item, count, stage); }
		public static void onShare    (string service, string item, string stage = "@last") { logger_onShare (service, item, stage); }
		public static void onError    (string type, string msg) { logger_onError (type, msg); }
		public static void clearStatus() { logger_clearStatus (); }
		public static void setStatus  (string key, int value, bool bAutoConvert = true) { logger_setStatus(key, value, bAutoConvert); }
		public static void setUser    (string level, int age = -1, string gender = null, string userId = null, string userService = null) { logger_setUser (level, age, gender, userId, userService); }

		/// //////////////////////////////////////////
		/// 声明模块导出函数
		/// //////////////////////////////////////////
  #if UNITY_IPHONE
		[DllImport ("__Internal")] private static extern void   logger_init (string appKey, string appChannel);
		[DllImport ("__Internal")] private static extern void   logger_setDebugLog(int mode, int level);
		[DllImport ("__Internal")] private static extern void   logger_setSessionInterval (int v);
        [DllImport ("__Internal")] private static extern void   logger_setOnline (bool bOnline);
		[DllImport ("__Internal")] private static extern void   logger_enableCrashReport ();
		[DllImport ("__Internal")] private static extern void   logger_enableOnlineConfig();
		[DllImport ("__Internal")] private static extern string logger_getParamValuePTR(string param, ref int len, string defValue);
		[DllImport ("__Internal")] private static extern int    logger_onSubStart (string stage);
		[DllImport ("__Internal")] private static extern void   logger_onSubEnd (string stage);
		[DllImport ("__Internal")] private static extern void   logger_onPassFail (bool bPass);
		[DllImport ("__Internal")] private static extern void   logger_onEvent (string name, string stage);
        [DllImport ("__Internal")] private static extern void   logger_onBalance (string name, string item, int value, string stage);
        [DllImport ("__Internal")] private static extern void   logger_onLevelUp (string name, string oldLevel, string newLevel, string stage);
		[DllImport ("__Internal")] private static extern void   logger_onBuy (string service, string item, int count, float value, string stage);
		[DllImport ("__Internal")] private static extern void   logger_onUse (string item, int count, string stage);
		[DllImport ("__Internal")] private static extern void   logger_onExchange (string item, int count, string stage);
		[DllImport ("__Internal")] private static extern void   logger_onCollect (string item, int count, string stage);
		[DllImport ("__Internal")] private static extern void   logger_onReward (string item, int count, string stage);
		[DllImport ("__Internal")] private static extern void   logger_onShare (string service, string item, string stage);
		[DllImport ("__Internal")] private static extern void   logger_onError (string type, string msg);
		[DllImport ("__Internal")] private static extern void   logger_clearStatus();
		[DllImport ("__Internal")] private static extern void   logger_setStatus(string key, int value, bool bAutoConvert);
		[DllImport ("__Internal")] private static extern void   logger_setUser (string level, int age, string gender, string userId, string userService);
  #elif UNITY_ANDROID
		[DllImport ("logger")] private static extern void logger_setDebugLog(int mode, int level);
		[DllImport ("logger")] private static extern void logger_setSessionInterval (int v);
        [DllImport ("logger")] private static extern void logger_setOnline (bool bOnline);
		[DllImport ("logger")] private static extern void logger_enableCrashReport ();
		[DllImport ("logger")] private static extern void logger_enableOnlineConfig();
		[DllImport ("logger")] private static extern void logger_onStart ();
		[DllImport ("logger")] private static extern void logger_onEnd ();
		[DllImport ("logger")] private static extern void logger_onExit ();
		[DllImport ("logger")] private static extern int  logger_onSubStart (string stage);
		[DllImport ("logger")] private static extern void logger_onSubEnd (string stage);
		[DllImport ("logger")] private static extern void logger_onPassFail (bool bPass);
		[DllImport ("logger")] private static extern void logger_onEvent (string name, string stage);
        [DllImport ("logger")] private static extern void logger_onBalance (string name, string item, int value, string stage);
        [DllImport ("logger")] private static extern void logger_onLevelUp (string name, string oldLevel, string newLevel, string stage);
		[DllImport ("logger")] private static extern void logger_onBuy (string service, string item, int count, float value, string stage);
		[DllImport ("logger")] private static extern void logger_onUse (string item, int count, string stage);
		[DllImport ("logger")] private static extern void logger_onExchange (string item, int count, string stage);
		[DllImport ("logger")] private static extern void logger_onCollect (string item, int count, string stage);
		[DllImport ("logger")] private static extern void logger_onReward (string item, int count, string stage);
		[DllImport ("logger")] private static extern void logger_onShare (string service, string item, string stage);
		[DllImport ("logger")] private static extern void logger_onError (string type, string msg);
		[DllImport ("logger")] private static extern void logger_clearStatus();
		[DllImport ("logger")] private static extern void logger_setStatus(string key, int value, bool bAutoConvert);
		[DllImport ("logger")] private static extern void logger_setUser (string level, int age, string gender, string userId, string userService);
  #endif
#else
	// 其他平台不支持
	public static void   init (string appKey, string appChannel = null){}
	public static void   setDebugLog (DEBUG_LOG_MODE mode, DEBUG_LOG_LEVEL level) {}
	public static void   setSessionInterval (int v) {}
	public static void   setOnline (bool bOnline) {}
	public static void   enableCrashReport () {}
	public static void   enableOnlineConfig () {}
	public static string getOnlineConfig(string param) {return null;}
	public static void   onSubStart (string stage) {}
	public static void   onSubEnd   (string stage) {}
	public static void   onPassFail (bool bPass)   {}
	public static void   onEvent    (string name,    string stage = "@last") {}
    public static void   onBalance  (string name, string item, int value, string stage = "@last") {}
    public static void   onLevelUp  (string name, string oldLevel, string newLevel, string stage = "@last") {}
	public static void   onBuy      (string service, string item, int count, float value, string stage = "@last") {}
	public static void   onUse      (string item, int count, string stage = "@last") {}
	public static void   onExchange (string item, int count, string stage = "@last") {}
	public static void   onCollect  (string item, int count, string stage = "@last") {}
	public static void   onReward   (string item, int count, string stage = "@last") {}
	public static void   onShare    (string service, string item, string stage = "@last") {}
	public static void   onError    (string type, string msg) {}
	public static void   clearStatus() {}
	public static void   setStatus  (string key, int value, bool bAutoConvert = true) {}
	public static void   setUser    (string level, int age = -1, string gender = null, string userId = null, string userService = null) {}
#endif
	}
}