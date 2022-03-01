using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

using AnyThinkAds.Common;
using AnyThinkAds.ThirdParty.LitJson;


namespace AnyThinkAds.Api
{
    public class ATNativeAdLoadingExtra
    {
        public static readonly string kATNativeAdLoadingExtraNativeAdSizeStruct = "native_ad_size_struct";
        public static readonly string kATNativeAdLoadingExtraNativeAdSize = "native_ad_size";
        public static readonly string kATNativeAdSizeUsesPixelFlagKey = "uses_pixel";
    }

    public class ATNativeAd
    {

        private static readonly ATNativeAd instance = new ATNativeAd();
        private IATNativeAdClient client;

        public ATNativeAd(){
            client = GetATNativeAdClient();
        }

        public static ATNativeAd Instance
        {
            get
            {
                return instance;
            }
        }


        public void loadNativeAd(string placementId, Dictionary<String,object> pairs){
            if (pairs != null && pairs.ContainsKey(ATNativeAdLoadingExtra.kATNativeAdLoadingExtraNativeAdSizeStruct))
            {
                ATSize size = (ATSize)(pairs[ATNativeAdLoadingExtra.kATNativeAdLoadingExtraNativeAdSizeStruct]);
                pairs.Add(ATNativeAdLoadingExtra.kATNativeAdLoadingExtraNativeAdSize, size.width + "x" + size.height);
                pairs.Add(ATNativeAdLoadingExtra.kATNativeAdSizeUsesPixelFlagKey, size.usesPixel);
            }
            client.loadNativeAd(placementId,JsonMapper.ToJson(pairs));
        }

        public bool hasAdReady(string placementId){
            return client.hasAdReady(placementId);
        }

        public string checkAdStatus(string placementId)
        {
            return client.checkAdStatus(placementId);
        }

        public string getValidAdCaches(string placementId)
        {
            return client.getValidAdCaches(placementId);
        }

        public void setListener(ATNativeAdListener listener){
            client.setListener(listener);
        }

        public void renderAdToScene(string placementId, ATNativeAdView anyThinkNativeAdView){
            client.renderAdToScene(placementId, anyThinkNativeAdView, "");
        }

        public void renderAdToScene(string placementId, ATNativeAdView anyThinkNativeAdView, Dictionary<string,string> pairs){
            client.renderAdToScene(placementId, anyThinkNativeAdView, JsonMapper.ToJson(pairs));
        }

        public void cleanAdView(string placementId, ATNativeAdView anyThinkNativeAdView){
            client.cleanAdView(placementId, anyThinkNativeAdView);
        }

        public void onApplicationForces(string placementId, ATNativeAdView anyThinkNativeAdView){
            client.onApplicationForces(placementId, anyThinkNativeAdView);
        }

        public void onApplicationPasue(string placementId, ATNativeAdView anyThinkNativeAdView){
            client.onApplicationPasue(placementId, anyThinkNativeAdView);
        }

        public void cleanCache(string placementId){
            client.cleanCache(placementId);
        }



        public IATNativeAdClient GetATNativeAdClient()
        {
            return AnyThinkAds.ATAdsClientFactory.BuildNativeAdClient();
        }

    }
}