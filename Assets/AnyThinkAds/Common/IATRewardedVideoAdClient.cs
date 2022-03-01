using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AnyThinkAds.Api;

namespace AnyThinkAds.Common
{
    public interface IATRewardedVideoAdClient
    {
		/**
		 * 请求视屏广告
		 * @param placementId 广告位id
		 * @parm mapJson 平台私有参数 一般不些
		 */
        void loadVideoAd(string placementId, string mapJson);
		/**
		 * @param listener 监听回调
		 */ 
        void setListener(ATRewardedVideoListener listener);
		/**
		 * 是否存在可以展示的广告
		 * @param unityid
		 * 
		 */ 
        bool hasAdReady(string placementId);
        /**
		 * 获取广告状态信息（是否正在加载、是否存在可以展示广告、广告缓存详细信息）
		 * @param unityid
		 * 
		 */
        string checkAdStatus(string placementId);
		/***
		 * 显示广告
		 */
        void showAd(string placementId, string mapJson);

		/***
		 * 获取所有可用缓存广告
		 */
		string getValidAdCaches(string placementId);
	}
}
