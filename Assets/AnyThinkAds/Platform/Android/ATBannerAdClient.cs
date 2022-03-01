using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AnyThinkAds.Common;
using AnyThinkAds.Api;
namespace AnyThinkAds.Android
{
    public class ATBannerAdClient : AndroidJavaProxy, IATBannerAdClient
    {

        private Dictionary<string, AndroidJavaObject> bannerHelperMap = new Dictionary<string, AndroidJavaObject>();

        private  ATBannerAdListener anyThinkListener;

        public ATBannerAdClient() : base("com.anythink.unitybridge.banner.BannerListener")
        {
            
        }


        public void loadBannerAd(string placementId, string mapJson)
        {

            //如果不存在则直接创建对应广告位的helper
            if(!bannerHelperMap.ContainsKey(placementId))
            {
                AndroidJavaObject bannerHelper = new AndroidJavaObject(
                    "com.anythink.unitybridge.banner.BannerHelper", this);
                bannerHelper.Call("initBanner", placementId);
                bannerHelperMap.Add(placementId, bannerHelper);
                Debug.Log("ATBannerAdClient : no exit helper ,create helper ");
            }

            try
            {
                Debug.Log("ATBannerAdClient : loadBannerAd ");
                bannerHelperMap[placementId].Call("loadBannerAd", mapJson);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log ("ATBannerAdClient :  error."+e.Message);
            }


        }

        public string checkAdStatus(string placementId)
        {
            string adStatusJsonString = "";
            Debug.Log("ATBannerAdClient : checkAdStatus....");
            try
            {
                if (bannerHelperMap.ContainsKey(placementId))
                {
                    adStatusJsonString = bannerHelperMap[placementId].Call<string>("checkAdStatus");
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("ATBannerAdClient :  error." + e.Message);
            }

            return adStatusJsonString;
        }

        public string getValidAdCaches(string placementId)
        {
            string validAdCachesString = "";
            Debug.Log("ATBannerAdClient : getValidAdCaches....");
            try
            {
                if (bannerHelperMap.ContainsKey(placementId))
                {
                    validAdCachesString = bannerHelperMap[placementId].Call<string>("getValidAdCaches");
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("ATBannerAdClient :  error." + e.Message);
            }

            return validAdCachesString;
        }


        public void setListener(ATBannerAdListener listener)
        {
            anyThinkListener = listener;
        }


        public void showBannerAd(string placementId, string position, string mapJson)
        {
            Debug.Log("ATBannerAdClient : showBannerAd by position" );
            //todo
            try
            {
                if (bannerHelperMap.ContainsKey(placementId))
                {
                    this.bannerHelperMap[placementId].Call("showBannerAd", position, mapJson);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("ATBannerAdClient :  error." + e.Message);
            }

        }
       

		
        public void showBannerAd(string placementId, ATRect rect, string mapJson)
        {
            Debug.Log("ATBannerAdClient : showBannerAd " );

			try{
                if (bannerHelperMap.ContainsKey(placementId)) {
                    this.bannerHelperMap[placementId].Call ("showBannerAd", rect.x, rect.y, rect.width, rect.height, mapJson);
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log ("ATBannerAdClient :  error."+e.Message);

			}
        }

        public void cleanBannerAd(string placementId)
        {
			
            Debug.Log("ATBannerAdClient : cleanBannerAd" );

			try{
                if (bannerHelperMap.ContainsKey(placementId)) {
                    this.bannerHelperMap[placementId].Call ("cleanBannerAd");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log ("ATBannerAdClient :  error."+e.Message);
			}
        }

        public void hideBannerAd(string placementId) 
        {
            Debug.Log("ATBannerAdClient : hideBannerAd");

            try
            {
                if (bannerHelperMap.ContainsKey(placementId))
                {
                    this.bannerHelperMap[placementId].Call("hideBannerAd");
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("ATBannerAdClient :  error." + e.Message);
            }
        }

        //针对已有的进行展示，没有就调用该方法无效
        public void showBannerAd(string placementId)
        {
            Debug.Log("ATBannerAdClient : showBannerAd ");

            try
            {
                if (bannerHelperMap.ContainsKey(placementId))
                {
                    this.bannerHelperMap[placementId].Call("showBannerAd");
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("ATBannerAdClient :  error." + e.Message);

            }
        }

        public void cleanCache(string placementId)
        {
            
        }

       
        //广告加载成功
        public void onBannerLoaded(string placementId)
        {
            Debug.Log("onBannerLoaded...unity3d.");
            if(anyThinkListener != null){
                anyThinkListener.onAdLoad(placementId);
            } 
        }

        //广告加载失败
        public void onBannerFailed(string placementId,string code, string error)
        {
            Debug.Log("onBannerFailed...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onAdLoadFail(placementId, code, error);
            }
        }

        //广告点击
        public void onBannerClicked(string placementId, string callbackJson)
        {
            Debug.Log("onBannerClicked...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onAdClick(placementId, new ATCallbackInfo(callbackJson));
            }
        }

        //广告展示
        public void onBannerShow(string placementId, string callbackJson)
        {
            Debug.Log("onBannerShow...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onAdImpress(placementId, new ATCallbackInfo(callbackJson));
            }
        }

        //广告关闭
        public void onBannerClose(string placementId, string callbackJson)
        {
            Debug.Log("onBannerClose...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onAdCloseButtonTapped(placementId, new ATCallbackInfo(callbackJson));
            }
        }
        //广告关闭
        public void onBannerAutoRefreshed(string placementId, string callbackJson)
        {
            Debug.Log("onBannerAutoRefreshed...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onAdAutoRefresh(placementId, new ATCallbackInfo(callbackJson));
            }
        }
        //广告自动刷新失败
        public void onBannerAutoRefreshFail(string placementId, string code, string msg)
        {
            Debug.Log("onBannerAutoRefreshFail...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onAdAutoRefreshFail(placementId,code,msg);
            }
        }
       
    }
}
