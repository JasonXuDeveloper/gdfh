using UnityEngine;
using AnyThinkAds.Api;
using AnyThinkAds.Common;

using System.Collections;
using System.Collections.Generic;

namespace AnyThinkAds
{
    public class ATAdsClientFactory
    {
        public static IATBannerAdClient BuildBannerAdClient()
        {
            #if UNITY_EDITOR
            // Testing UNITY_EDITOR first because the editor also responds to the currently
            // selected platform.
            #elif UNITY_ANDROID
                return new AnyThinkAds.Android.ATBannerAdClient();
            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
                return new AnyThinkAds.iOS.ATBannerAdClient();
            #else
                
            #endif
            return new UnityBannerClient();
        }

        public static IATInterstitialAdClient BuildInterstitialAdClient()
        {
            #if UNITY_EDITOR
            // Testing UNITY_EDITOR first because the editor also responds to the currently
            // selected platform.
            #elif UNITY_ANDROID
                return new AnyThinkAds.Android.ATInterstitialAdClient();
            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
                return new AnyThinkAds.iOS.ATInterstitialAdClient();
            #else

            #endif
            return new UnityInterstitialClient();
        }

        public static IATNativeAdClient BuildNativeAdClient()
        {
           #if UNITY_EDITOR
            // Testing UNITY_EDITOR first because the editor also responds to the currently
            // selected platform.
            #elif UNITY_ANDROID
                return new AnyThinkAds.Android.ATNativeAdClient();
            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
                return new AnyThinkAds.iOS.ATNativeAdClient();
            #else

            #endif
            return new UnityNativeAdClient();
        }

        public static IATNativeBannerAdClient BuildNativeBannerAdClient()
        {
           #if UNITY_EDITOR
            // Testing UNITY_EDITOR first because the editor also responds to the currently
            // selected platform.
            #elif UNITY_ANDROID
                return new AnyThinkAds.Android.ATNativeBannerAdClient();
            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
                return new AnyThinkAds.iOS.ATNativeBannerAdClient();
            #else

            #endif
            return new UnityNativeBannerAdClient();
        }

        public static IATRewardedVideoAdClient BuildRewardedVideoAdClient()
        {
            #if UNITY_EDITOR
            // Testing UNITY_EDITOR first because the editor also responds to the currently
            // selected platform.

            #elif UNITY_ANDROID
                return new AnyThinkAds.Android.ATRewardedVideoAdClient();
            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
                return new AnyThinkAds.iOS.ATRewardedVideoAdClient();            
            #else
                            
            #endif
            return new UnityRewardedVideoAdClient();
        }

        public static IATSDKAPIClient BuildSDKAPIClient()
        {
            Debug.Log("BuildSDKAPIClient");
            #if UNITY_EDITOR
                Debug.Log("Unity Editor");
                        // Testing UNITY_EDITOR first because the editor also responds to the currently
                        // selected platform.

            #elif UNITY_ANDROID
                return new AnyThinkAds.Android.ATSDKAPIClient();
            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
                 Debug.Log("Unity:ATAdsClientFactory::Build iOS Client");
                return new AnyThinkAds.iOS.ATSDKAPIClient();         
            #else

            #endif
            return new UnitySDKAPIClient();
        }

        public static IATDownloadClient BuildDownloadClient()
        {
            Debug.Log("BuildDownloadClient");
            #if UNITY_EDITOR
                Debug.Log("Unity Editor");
                        // Testing UNITY_EDITOR first because the editor also responds to the currently
                        // selected platform.

            #elif UNITY_ANDROID
                return new AnyThinkAds.Android.ATDownloadClient();
               
            #else

            #endif
                return new UnityDownloadClient();
        }

    }

    class UnitySDKAPIClient:IATSDKAPIClient
    {
        public void initSDK(string appId, string appkey){}
        public void initSDK(string appId, string appkey, ATSDKInitListener listener){ }
        public void getUserLocation(ATGetUserLocationListener listener){ }
        public void setGDPRLevel(int level){ }
        public void showGDPRAuth(){ }
        public void addNetworkGDPRInfo(int networkType, string mapJson){ }
        public void setChannel(string channel){ }
        public void setSubChannel(string subchannel){ }
        public void initCustomMap(string cutomMap){ }
        public void setCustomDataForPlacementID(string customData, string placementID){ }
        public void setLogDebug(bool isDebug){ }
        public int getGDPRLevel(){ return ATSDKAPI.PERSONALIZED; }
        public bool isEUTraffic() { return false; }
        public void deniedUploadDeviceInfo(string deniedInfo) { }
        public void setExcludeBundleIdArray(string bundleIds) { }
        public void setExcludeAdSourceIdArrayForPlacementID(string placementID, string adsourceIds) { }
        public void setSDKArea(int area) { }
        public void getArea(ATGetAreaListener listener) { }
        public void setWXStatus(bool install) { }
        public void setLocation(double longitude, double latitude) { }

    }

    class UnityBannerClient:IATBannerAdClient
    {
       ATBannerAdListener listener;
       public void loadBannerAd(string unitId, string mapJson){
            if(listener != null)
            {
                listener.onAdLoadFail(unitId, "-1", "Must run on Android or IOS platform!");
            }
       }
     
       public void setListener(ATBannerAdListener listener)
       {
            this.listener = listener;
       }

       public string checkAdStatus(string unitId) { return ""; }
       
       public void showBannerAd(string unitId, string position){ }

       public void showBannerAd(string unitId, string position, string mapJson){ }
       
       public void showBannerAd(string unitId, ATRect rect){ }

       public void showBannerAd(string unitId, ATRect rect, string mapJson){ }

       public  void cleanBannerAd(string unitId){ }
      
       public void hideBannerAd(string unitId){ }
    
       public void showBannerAd(string unitId){ }
      
       public void cleanCache(string unitId){}

        public string getValidAdCaches(string unitId) { return ""; }
    }

    class UnityInterstitialClient : IATInterstitialAdClient
    {
       ATInterstitialAdListener listener;
       public void loadInterstitialAd(string unitId, string mapJson){
            if (listener != null)
            {
               listener.onInterstitialAdLoadFail(unitId, "-1", "Must run on Android or IOS platform!");
            }
       }
       
       public void setListener(ATInterstitialAdListener listener){
            this.listener = listener;
       }

       public bool hasInterstitialAdReady(string unitId) { return false; }

        public string checkAdStatus(string unitId) { return ""; }

        public void showInterstitialAd(string unitId, string mapJson){}
        
       public void cleanCache(string unitId){}
        public string getValidAdCaches(string unitId) { return ""; }

    }

    class UnityNativeAdClient : IATNativeAdClient
    {
        ATNativeAdListener listener;
       public void loadNativeAd(string unitId, string mapJson){
            if(listener != null)
            {
                listener.onAdLoadFail(unitId, "-1", "Must run on Android or IOS platform!");
            }
       }

       public bool hasAdReady(string unitId) { return false; }

       public string checkAdStatus(string unitId) { return ""; }

       public string getValidAdCaches(string unitId) { return ""; }


        public void setListener(ATNativeAdListener listener){
            this.listener = listener;
       }
        
       public void renderAdToScene(string unitId, ATNativeAdView anyThinkNativeAdView){}

       public void renderAdToScene(string unitId, ATNativeAdView anyThinkNativeAdView, string mapJson){}

       public void cleanAdView(string unitId, ATNativeAdView anyThinkNativeAdView){}
       
       public void onApplicationForces(string unitId, ATNativeAdView anyThinkNativeAdView){}
        
       public void onApplicationPasue(string unitId, ATNativeAdView anyThinkNativeAdView){}
        
       public void cleanCache(string unitId){}
        
       public void setLocalExtra(string unitid, string mapJson){}
    }

    class UnityNativeBannerAdClient : IATNativeBannerAdClient
    {
        ATNativeBannerAdListener listener;
       public void loadAd(string unitId, string mapJson){
            if(listener != null)
            {
                 listener.onAdLoadFail(unitId, "-1", "Must run on Android or IOS platform!");
            }
       }

       public bool adReady(string unitId) { return false; }
        
       public void setListener(ATNativeBannerAdListener listener){
            this.listener = listener;
       }
       
       public void showAd(string unitId, ATRect rect, Dictionary<string, string> pairs){}
        
       public void removeAd(string unitId){}
    }

    class UnityRewardedVideoAdClient : IATRewardedVideoAdClient
    {
        ATRewardedVideoListener listener;
        public void loadVideoAd(string unitId, string mapJson){
            if (listener != null)
            {
                listener.onRewardedVideoAdLoadFail(unitId, "-1", "Must run on Android or IOS platform!");
            }
       }

        public void setListener(ATRewardedVideoListener listener){
            this.listener = listener;
       }

        public bool hasAdReady(string unitId) { return false; }

        public string checkAdStatus(string unitId) { return ""; }

        public string getValidAdCaches(string unitId) { return ""; }

        public void showAd(string unitId, string mapJson){}

    }


    class UnityDownloadClient : IATDownloadClient
    {
        public void setListener(ATDownloadAdListener listener)
        {
            Debug.Log("Must run on Android platform");
        }
    }
}