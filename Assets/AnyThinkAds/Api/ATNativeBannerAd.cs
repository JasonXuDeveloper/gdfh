using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

using AnyThinkAds.Common;
using AnyThinkAds.ThirdParty.LitJson;


namespace AnyThinkAds.Api
{
	public class ATNativeBannerAdShowingExtra
	{
		public static readonly string kATNativeBannerAdShowingExtraBackgroundColor = "background_color";
		public static readonly string kATNativeBannerAdShowingExtraAutorefreshInterval = "autorefresh_interval";
		public static readonly string kATNativeBannerAdShowingExtraHideCloseButtonFlag = "hide_close_button_flag";
		public static readonly string kATNativeBannerAdShowingExtraCTAButtonBackgroundColor = "cta_button_background_color";
		public static readonly string kATNativeBannerAdShowingExtraCTATextColor = "cta_button_title_color";//of type string, example:#3e2f10
		public static readonly string kATNativeBannerAdShowingExtraCTATextFont = "cta_text_font";//of type double
		public static readonly string kATNativeBannerAdShowingExtraTitleColor = "title_color";
		public static readonly string kATNativeBannerAdShowingExtraTitleFont = "title_font";
		public static readonly string kATNativeBannerAdShowingExtraTextColor = "text_color";
		public static readonly string kATNativeBannerAdShowingExtraTextFont = "text_font";
		public static readonly string kATNativeBannerAdShowingExtraAdvertiserTextFont = "sponsor_text_font";
		public static readonly string kATNativeBannerAdShowingExtraAdvertiserTextColor = "spnosor_text_color";
	}

    public class ATNativeBannerAd
    {
    	private static readonly ATNativeBannerAd instance = new ATNativeBannerAd();
		private IATNativeBannerAdClient client;
		public ATNativeBannerAd() {
            client = GetATNativeBannerAdClient();
		}
		
		public static ATNativeBannerAd Instance {
			get {
				return instance;
			}
		}

		public void loadAd(string placementId, Dictionary<String, String> pairs) {
			Debug.Log("ATNativeBannerAd::loadAd(" + placementId + ")");
			client.loadAd(placementId, JsonMapper.ToJson(pairs));
		}

		public bool adReady(string placementId) {
            Debug.Log("ATNativeBannerAd::adReady(" + placementId + ")");
			return client.adReady(placementId);
		}

		public void setListener(ATNativeBannerAdListener listener) {
            Debug.Log("ATNativeBannerAd::setListener");
			client.setListener(listener);
		}

		public void showAd(string placementId, ATRect rect, Dictionary<string, string> pairs) {
            Debug.Log("ATNativeBannerAd::showAd");
			client.showAd(placementId, rect, pairs);
		}

		public void removeAd(string placementId) {
            Debug.Log("ATNativeBannerAd::removeAd");
			client.removeAd(placementId);
		}

		public IATNativeBannerAdClient GetATNativeBannerAdClient()
        {
            return AnyThinkAds.ATAdsClientFactory.BuildNativeBannerAdClient();
        }
    }
}
