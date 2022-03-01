using UnityEngine;

using AnyThinkAds.Common;
using AnyThinkAds.Api;
namespace AnyThinkAds.Android
{
    public class ATDownloadClient : AndroidJavaProxy,IATDownloadClient
    {

        private AndroidJavaObject downloadHelper;


        private  ATDownloadAdListener anyThinkListener;

        public ATDownloadClient() : base("com.anythink.unitybridge.download.DownloadListener")
        {
            
        }

        public void setListener(ATDownloadAdListener listener)
        {
            Debug.Log("ATDownloadClient : setListener");
            anyThinkListener = listener;

            if (downloadHelper == null)
            {
                downloadHelper = new AndroidJavaObject(
                    "com.anythink.unitybridge.download.DownloadHelper", this);
            }

        }

        
        public void onDownloadStart(string placementId, string callbackJson, long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.Log("onDownloadStart...unity3d.");
            if(anyThinkListener != null){
                anyThinkListener.onDownloadStart(placementId, new ATCallbackInfo(callbackJson), totalBytes, currBytes, fileName, appName);
            }
        }

        
        public void onDownloadUpdate(string placementId, string callbackJson, long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.Log("onDownloadUpdate...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onDownloadUpdate(placementId, new ATCallbackInfo(callbackJson), totalBytes, currBytes, fileName, appName);
            }
        }

        
        public void onDownloadPause(string placementId, string callbackJson, long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.Log("onDownloadPause...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onDownloadPause(placementId, new ATCallbackInfo(callbackJson), totalBytes, currBytes, fileName, appName);
            }
        }

       
        public void onDownloadFinish(string placementId, string callbackJson, long totalBytes, string fileName, string appName)
        {
            Debug.Log("onDownloadFinish...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onDownloadFinish(placementId, new ATCallbackInfo(callbackJson), totalBytes, fileName, appName);
            }
        }

       
        public void onDownloadFail(string placementId, string callbackJson, long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.Log("onDownloadFail...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onDownloadFail(placementId, new ATCallbackInfo(callbackJson), totalBytes, currBytes, fileName, appName);
            }
        }
       

        public void onInstalled(string placementId, string callbackJson, string fileName, string appName)
        {
            Debug.Log("onInstalled...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInstalled(placementId, new ATCallbackInfo(callbackJson), fileName, appName);
            }
        }
     
    }
}
