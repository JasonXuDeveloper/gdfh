using UnityEngine;
using System;
using System.Collections.Generic;

namespace Tianti
{
	public struct crash_struct {
		public int count;
		public string type,reason,stack;
		public crash_struct(string type,string reason,string stack) {
			this.type = type;
			this.reason = reason;
			this.stack = stack;
			this.count = 0;
		}
	}

	public class CrashHandler {
		public static void Init() {
			RegisterHandler ();
		}

		static private List<crash_struct> crash = new List<crash_struct>();
		static private object listLock = new object();
		static private int popIndex = 0;

		private static void RegisterHandler() {

			AppDomain.CurrentDomain.UnhandledException += OnUncaughtHandler;

			#if (UNITY_4_9 || UNITY_4_8 || UNITY_4_7 || UNITY_4_6 || UNITY_4_5 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0 || UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0 || UNITY_2_6_1 || UNITY_2_6)
			Application.RegisterLogCallback(OnLogHandler);
			#else
			Application.logMessageReceived += OnLogHandler;
			#endif
		}

		private static void OnLogHandler (string condition, string stackTrace, LogType type) {
			if (type == LogType.Exception || type == LogType.Assert) {
				string sType;
				if (type == LogType.Exception) {
					sType = "Exception";
				}
				else sType = "Assert";
				AddCrash (sType, condition, stackTrace);
			}
		}


		private static void OnUncaughtHandler (object sender, System.UnhandledExceptionEventArgs e) {
			Exception ec = (Exception)e.ExceptionObject;
			AddCrash("UnHandled", ec.Message, ec.StackTrace);
		}

		private static void AddCrash (string type, string reason, string stack) {
			lock (listLock) {
				bool bFound = false;
				for (int i=0; i<crash.Count; i++) {
					crash_struct cs = crash [i];
					if (cs.reason == reason && cs.stack == stack) {
						cs.count++;
						crash [i] = cs;
						bFound = true;
						break;
					}
				}
				// 同个错误只发送一次
				if (!bFound) {
					crash.Add(new crash_struct (type, reason, stack));
					Tianti.AppLogger.onError (type, string.Format("{0}\n{1}\nBinary Image:\n", reason,stack));
				}
			}
		}

		public static crash_struct popCrash () {
			crash_struct c = new crash_struct();
			lock(listLock)
			{
				if (crash != null && crash.Count > popIndex) {
					c = crash [popIndex];
					popIndex++;
				}
			}
			return c;
		}
	}
}