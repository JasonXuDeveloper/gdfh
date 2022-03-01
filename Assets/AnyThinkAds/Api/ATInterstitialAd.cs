using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

using AnyThinkAds.Common;
using AnyThinkAds.ThirdParty.LitJson;

namespace AnyThinkAds.Api
{
    public class ATInterstitialAdLoadingExtra
    {
        public static readonly string kATInterstitialAdLoadingExtraInterstitialAdSize = "interstitial_ad_size";
        public static readonly string kATInterstitialAdLoadingExtraInterstitialAdSizeStruct = "interstitial_ad_size_struct";
        public static readonly string kATInterstitialAdSizeUsesPixelFlagKey = "uses_pixel";
    }

    public class ATInterstitialAd
	{
		private static readonly ATInterstitialAd instance = new ATInterstitialAd();
		private IATInterstitialAdClient client;

		private ATInterstitialAd()
		{
            client = GetATInterstitialAdClient();
		}

		public static ATInterstitialAd Instance 
		{
			get
			{
				return instance;
			}
		}

		public void loadInterstitialAd(string placementId, Dictionary<string,object> pairs)
        {
            if (pairs != null && pairs.ContainsKey(ATInterstitialAdLoadingExtra.kATInterstitialAdLoadingExtraInterstitialAdSizeStruct))
            {
                ATSize size = (ATSize)(pairs[ATInterstitialAdLoadingExtra.kATInterstitialAdLoadingExtraInterstitialAdSizeStruct]);
                pairs.Add(ATInterstitialAdLoadingExtra.kATInterstitialAdLoadingExtraInterstitialAdSize, size.width + "x" + size.height);
                pairs.Add(ATInterstitialAdLoadingExtra.kATInterstitialAdSizeUsesPixelFlagKey, size.usesPixel);

                client.loadInterstitialAd(placementId, JsonMapper.ToJson(pairs));
            } else
            {
                client.loadInterstitialAd(placementId, JsonMapper.ToJson(pairs));
            }
        }

		public void setListener(ATInterstitialAdListener listener)
        {
            client.setListener(listener);
        }

        public bool hasInterstitialAdReady(string placementId)
        {
            return client.hasInterstitialAdReady(placementId);
        }

        public string checkAdStatus(string placementId)
        {
            return client.checkAdStatus(placementId);
        }

        public string getValidAdCaches(string placementId)
        {
            return client.getValidAdCaches(placementId);
        }

        public void showInterstitialAd(string placementId)
        {
            client.showInterstitialAd(placementId, JsonMapper.ToJson(new Dictionary<string, string>()));
        }

        public void showInterstitialAd(string placementId, Dictionary<string, string> pairs)
        {
            client.showInterstitialAd(placementId, JsonMapper.ToJson(pairs));
        }

        public IATInterstitialAdClient GetATInterstitialAdClient()
        {
            return AnyThinkAds.ATAdsClientFactory.BuildInterstitialAdClient();
        }
	}
}
