using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AnyThinkAds.Common;
using AnyThinkAds.Api;

namespace AnyThinkAds.Android
{
    public class ATNativeBannerAdClient :IATNativeBannerAdClient
    {
        public ATNativeBannerAdClient() {

        }

    	public void loadAd(string placementId, string mapJson) {

    	}
    	
		public bool adReady(string placementId) {
			return false;
		}

        public void setListener(ATNativeBannerAdListener listener) {

        }

        public void showAd(string placementId, ATRect rect, Dictionary<string, string> pairs) {

        }

        public void removeAd(string placementId) {

        }
    }
}
