using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnyThinkAds.Api;

namespace AnyThinkAds.Common
{
    public interface IATBannerAdClient 
    {
		/***
		 * 请求广告  
		 * @param placementId  广告位id
		 * @parm mapJson 各平台的私有属性 一般可以不调用
		 */
        void loadBannerAd(string placementId, string mapJson);
         /**
         * 获取广告状态信息（是否正在加载、是否存在可以展示广告、广告缓存详细信息）
         * @param unityid
         *
         */
        string checkAdStatus(string placementId);
		/***
		 * 
		 * 设置监听回调接口
		 * 
		 * @param listener  
		 */
        void setListener(ATBannerAdListener listener);
        /***
         * 
         * 展示广告,
         * @param placementId 
         * @param pass bottom or top for position
		 * @parm mapJson
         */
        void showBannerAd(string placementId, string position, string mapJson);
        /***
		 * 
		 * 展示广告,
		 * @param placementId 
		 * @param rect the region used to show banner ad; currently only x&y fields in rect are used(as the origin, or top left corner of the banner).
		 * @parm mapJson
		 */
        void showBannerAd(string placementId, ATRect rect, string mapJson);
		/***
		 * 
		 * 清理广告
		 * @param placementId 
		 * @param anyThinkNativeAdView  这里的属性是显示区域坐标等配置,需要自行设置
		 */
        void cleanBannerAd(string placementId);
        /***
		 * 
		 * 隐藏广告
		 * @param placementId 
		 * @param rect the region used to show banner ad.
		 */
        void hideBannerAd(string placementId);
        /***
		 * 
		 * （重新）展示之前隐藏的广告
		 * @param placementId 
		 */
        void showBannerAd(string placementId);
		/***
		 * 清理缓存
		 */ 
        void cleanCache(string placementId);

		/***
		 * 获取所有可用缓存广告
		 */
		string getValidAdCaches(string placementId);
    }
}
