using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AnyThinkAds.Common;
using AnyThinkAds.Api;
namespace AnyThinkAds.Android
{
    public class ATInterstitialAdClient : AndroidJavaProxy,IATInterstitialAdClient
    {

        private Dictionary<string, AndroidJavaObject> interstitialHelperMap = new Dictionary<string, AndroidJavaObject>();

		//private  AndroidJavaObject videoHelper;
        private  ATInterstitialAdListener anyThinkListener;

        public ATInterstitialAdClient() : base("com.anythink.unitybridge.interstitial.InterstitialListener")
        {
            
        }


        public void loadInterstitialAd(string placementId, string mapJson)
        {

            //如果不存在则直接创建对应广告位的helper
            if(!interstitialHelperMap.ContainsKey(placementId))
            {
                AndroidJavaObject videoHelper = new AndroidJavaObject(
                    "com.anythink.unitybridge.interstitial.InterstitialHelper", this);
                videoHelper.Call("initInterstitial", placementId);
                interstitialHelperMap.Add(placementId, videoHelper);
                Debug.Log("ATInterstitialAdClient : no exit helper ,create helper ");
            }

            try
            {
                Debug.Log("ATInterstitialAdClient : loadInterstitialAd ");
                interstitialHelperMap[placementId].Call("loadInterstitialAd", mapJson);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("ATInterstitialAdClient :  error."+e.Message);
            }


        }

        public void setListener(ATInterstitialAdListener listener)
        {
            anyThinkListener = listener;
        }

        public bool hasInterstitialAdReady(string placementId)
        {
			bool isready = false;
			Debug.Log ("ATInterstitialAdClient : hasAdReady....");
			try{
                if (interstitialHelperMap.ContainsKey(placementId)) {
                    isready = interstitialHelperMap[placementId].Call<bool> ("isAdReady");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("ATInterstitialAdClient :  error."+e.Message);
			}
			return isready; 
        }

        public string checkAdStatus(string placementId)
        {
            string adStatusJsonString = "";
            Debug.Log("ATInterstitialAdClient : checkAdStatus....");
            try
            {
                if (interstitialHelperMap.ContainsKey(placementId))
                {
                    adStatusJsonString = interstitialHelperMap[placementId].Call<string>("checkAdStatus");
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("ATInterstitialAdClient :  error." + e.Message);
            }

            return adStatusJsonString;
        }


        public string getValidAdCaches(string placementId)
        {
            string validAdCachesString = "";
            Debug.Log("ATNativeAdClient : getValidAdCaches....");
            try
            {
                if (interstitialHelperMap.ContainsKey(placementId))
                {
                    validAdCachesString = interstitialHelperMap[placementId].Call<string>("getValidAdCaches");
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("ATNativeAdClient :  error." + e.Message);
            }

            return validAdCachesString;
        }

        public void showInterstitialAd(string placementId, string jsonmap)
        {
			Debug.Log("ATInterstitialAdClient : showAd " );

			try{
                if (interstitialHelperMap.ContainsKey(placementId)) {
                    this.interstitialHelperMap[placementId].Call ("showInterstitialAd", jsonmap);
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("ATInterstitialAdClient :  error."+e.Message);

			}
        }


        public void cleanCache(string placementId)
        {
			
			Debug.Log("ATInterstitialAdClient : clean" );

			try{
                if (interstitialHelperMap.ContainsKey(placementId)) {
                    this.interstitialHelperMap[placementId].Call ("clean");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("ATInterstitialAdClient :  error."+e.Message);
			}
        }

        public void onApplicationForces(string placementId)
        {
			Debug.Log ("onApplicationForces.... ");
			try{
				if (interstitialHelperMap.ContainsKey(placementId)) {
					this.interstitialHelperMap[placementId].Call ("onResume");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("ATInterstitialAdClient :  error."+e.Message);
			}
        }

        public void onApplicationPasue(string placementId)
        {
			Debug.Log ("onApplicationPasue.... ");
			try{
				if (interstitialHelperMap.ContainsKey(placementId)) {
					this.interstitialHelperMap[placementId].Call ("onPause");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("ATInterstitialAdClient :  error."+e.Message);
			}
        }

        //广告加载成功
        public void onInterstitialAdLoaded(string placementId)
        {
            Debug.Log("onInterstitialAdLoaded...unity3d.");
            if(anyThinkListener != null){
                anyThinkListener.onInterstitialAdLoad(placementId);
            }
        }

        //广告加载失败
        public void onInterstitialAdLoadFail(string placementId,string code, string error)
        {
            Debug.Log("onInterstitialAdFailed...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInterstitialAdLoadFail(placementId, code, error);
            }
        }

        //开始播放
        public void onInterstitialAdVideoStart(string placementId, string callbackJson)
        {
            Debug.Log("onInterstitialAdPlayStart...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInterstitialAdStartPlayingVideo(placementId, new ATCallbackInfo(callbackJson));
            }
        }

        //结束播放
        public void onInterstitialAdVideoEnd(string placementId, string callbackJson)
        {
            Debug.Log("onInterstitialAdPlayEnd...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInterstitialAdEndPlayingVideo(placementId, new ATCallbackInfo(callbackJson));
            }
        }

        //播放失败
        public void onInterstitialAdVideoError(string placementId,string code, string error)
        {
            Debug.Log("onInterstitialAdPlayFailed...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInterstitialAdFailedToPlayVideo(placementId, code, error);
            }
        }
        //广告关闭
        public void onInterstitialAdClose(string placementId, string callbackJson)
        {
            Debug.Log("onInterstitialAdClosed...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInterstitialAdClose(placementId, new ATCallbackInfo(callbackJson));
            }
        }
        //广告点击
        public void onInterstitialAdClicked(string placementId, string callbackJson)
        {
            Debug.Log("onInterstitialAdClicked...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInterstitialAdClick(placementId, new ATCallbackInfo(callbackJson));
            }
        }

        public void onInterstitialAdShow(string placementId, string callbackJson){
            Debug.Log("onInterstitialAdShow...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInterstitialAdShow(placementId, new ATCallbackInfo(callbackJson));
            }
        }
       
    }
}
