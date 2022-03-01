using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

using AnyThinkAds.Common;
using AnyThinkAds.ThirdParty.LitJson;

namespace AnyThinkAds.Api
{
    public class ATBannerAdLoadingExtra
    {
        public static readonly string kATBannerAdLoadingExtraBannerAdSize = "banner_ad_size";
        public static readonly string kATBannerAdLoadingExtraBannerAdSizeStruct = "banner_ad_size_struct";
        public static readonly string kATBannerAdSizeUsesPixelFlagKey = "uses_pixel";
        public static readonly string kATBannerAdShowingPisitionTop = "top";
        public static readonly string kATBannerAdShowingPisitionBottom = "bottom";

        //Deprecated in v5.7.3
        public static readonly string kATBannerAdLoadingExtraInlineAdaptiveWidth = "inline_adaptive_width";
        public static readonly string kATBannerAdLoadingExtraInlineAdaptiveOrientation = "inline_adaptive_orientation";
        public static readonly int kATBannerAdLoadingExtraInlineAdaptiveOrientationCurrent = 0;
        public static readonly int kATBannerAdLoadingExtraInlineAdaptiveOrientationPortrait = 1;
        public static readonly int kATBannerAdLoadingExtraInlineAdaptiveOrientationLandscape = 2;
        //Deprecated in v5.7.3

        public static readonly string kATBannerAdLoadingExtraAdaptiveWidth = "adaptive_width";
        public static readonly string kATBannerAdLoadingExtraAdaptiveOrientation = "adaptive_orientation";
        public static readonly int kATBannerAdLoadingExtraAdaptiveOrientationCurrent = 0;
        public static readonly int kATBannerAdLoadingExtraAdaptiveOrientationPortrait = 1;
        public static readonly int kATBannerAdLoadingExtraAdaptiveOrientationLandscape = 2;

    }
    public class ATBannerAd 
	{
		private static readonly ATBannerAd instance = new ATBannerAd();
		private IATBannerAdClient client;

		private ATBannerAd() 
		{
            client = GetATBannerAdClient();
		}

		public static ATBannerAd Instance 
		{
			get 
			{
				return instance;
			}
		}

		/**
		API
		*/
		public void loadBannerAd(string placementId, Dictionary<string,object> pairs)
		{   
            if (pairs != null && pairs.ContainsKey(ATBannerAdLoadingExtra.kATBannerAdLoadingExtraBannerAdSize))
            {
                client.loadBannerAd(placementId, JsonMapper.ToJson(pairs));
            }
            else if (pairs != null && pairs.ContainsKey(ATBannerAdLoadingExtra.kATBannerAdLoadingExtraBannerAdSizeStruct))
            {
                ATSize size = (ATSize)(pairs[ATBannerAdLoadingExtra.kATBannerAdLoadingExtraBannerAdSizeStruct]);
                pairs.Add(ATBannerAdLoadingExtra.kATBannerAdLoadingExtraBannerAdSize, size.width + "x" + size.height);
                pairs.Add(ATBannerAdLoadingExtra.kATBannerAdSizeUsesPixelFlagKey, size.usesPixel);

                //Dictionary<string, object> newPaires = new Dictionary<string, object> { { ATBannerAdLoadingExtra.kATBannerAdLoadingExtraBannerAdSize, size.width + "x" + size.height }, { ATBannerAdLoadingExtra.kATBannerAdSizeUsesPixelFlagKey, size.usesPixel } };
                client.loadBannerAd(placementId, JsonMapper.ToJson(pairs));
            }
            else
            {
                client.loadBannerAd(placementId, JsonMapper.ToJson(pairs));
            }
			
		}

        public string checkAdStatus(string placementId)
        {
            return client.checkAdStatus(placementId);
        }

        public string getValidAdCaches(string placementId)
        {
            return client.getValidAdCaches(placementId);
        }


        public void setListener(ATBannerAdListener listener)
        {
            client.setListener(listener);
        }

        public void showBannerAd(string placementId, ATRect rect)
        {
            client.showBannerAd(placementId, rect, "");
        }

        public void showBannerAd(string placementId, ATRect rect, Dictionary<string,string> pairs)
        {
            client.showBannerAd(placementId, rect, JsonMapper.ToJson(pairs));
        }

        public void showBannerAd(string placementId, string position)
        {
            client.showBannerAd(placementId, position, "");
        }

        public void showBannerAd(string placementId, string position, Dictionary<string,string> pairs)
        {
            client.showBannerAd(placementId, position, JsonMapper.ToJson(pairs));
        }

        public void showBannerAd(string placementId)
        {
            client.showBannerAd(placementId);
        }

        public void hideBannerAd(string placementId)
        {
            client.hideBannerAd(placementId);
        }

        public void cleanBannerAd(string placementId)
        {
            client.cleanBannerAd(placementId);
        }

        public IATBannerAdClient GetATBannerAdClient()
        {
            return AnyThinkAds.ATAdsClientFactory.BuildBannerAdClient();
        }
	}
}
