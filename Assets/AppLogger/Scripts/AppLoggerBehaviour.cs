using UnityEngine;
using System.Collections;
using Tianti;
namespace Tianti
{
	public class AppLoggerBehaviour : MonoBehaviour
	{
	#if UNITY_EDITOR
	#elif UNITY_ANDROID

		static bool _pauseStatus = false;// 防止AppLogger.init()接口从Start()中调用

		void Awake () 
		{
			if (!_pauseStatus) {
				AppLogger.onStart ();
			}
		}

		void Start ()
		{
			DontDestroyOnLoad (transform.gameObject);
		}

		void OnApplicationPause(bool pauseStatus)
		{
			if (pauseStatus) {
				AppLogger.onEnd ();
			}
			else if (_pauseStatus != pauseStatus) {
				AppLogger.onStart ();
			}
			_pauseStatus = pauseStatus;
		}
			
		void OnApplicationQuit()
		{
			AppLogger.onExit ();
		}
			
	#endif
	}
}

