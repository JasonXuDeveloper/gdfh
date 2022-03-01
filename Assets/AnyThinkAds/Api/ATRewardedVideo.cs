using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

using AnyThinkAds.Common;
using AnyThinkAds.ThirdParty.LitJson;


namespace AnyThinkAds.Api
{
    public class ATRewardedVideo
    {
        private static readonly ATRewardedVideo instance = new ATRewardedVideo();
        private IATRewardedVideoAdClient client;

        private ATRewardedVideo()
        {
            client = GetATRewardedClient();
        }

        public static ATRewardedVideo Instance
        {
            get
            {
                return instance;
            }
        }


		/***
		 * 
		 */
        public void loadVideoAd(string placementId, Dictionary<string,string> pairs)
        {
            client.loadVideoAd(placementId, JsonMapper.ToJson(pairs));
        }

		public void setListener(ATRewardedVideoListener listener)
        {
            client.setListener(listener);
        }

        public bool hasAdReady(string placementId)
        {
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

        public void showAd(string placementId)
        {
            client.showAd(placementId, JsonMapper.ToJson(new Dictionary<string, string>()));
        }

        public void showAd(string placementId, Dictionary<string, string> pairs)
        {
            client.showAd(placementId, JsonMapper.ToJson(pairs));
        }

        public IATRewardedVideoAdClient GetATRewardedClient()
        {
            return AnyThinkAds.ATAdsClientFactory.BuildRewardedVideoAdClient();
        }

    }
}