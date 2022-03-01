using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AnyThinkAds.Api
{
    public interface ATInterstitialAdListener
    {
		/***
		 * 加载广告成功（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 * @param placementId 广告位id
		 */ 
        void onInterstitialAdLoad(string placementId);
		/***
		 * 加载广告失败（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 * @param placementId 广告位id
		 * @param code 错误码
		 * @param message 错误信息
		 */ 
        void onInterstitialAdLoadFail(string placementId, string code, string message);
		/***
		 * 广告展示（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 * @param placementId 广告位id
		 */ 
        void onInterstitialAdShow(string placementId, ATCallbackInfo callbackInfo);
		/***
		 * 广告展示失败（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 * @param placementId 广告位id
		 */ 
        void onInterstitialAdFailedToShow(string placementId);
		/***
		 * 广告关闭（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 * @param placementId 广告位id
		 */ 
        void onInterstitialAdClose(string placementId, ATCallbackInfo callbackInfo);
		/***
		 * 广告点击（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 * @param placementId 广告位id
		 */ 
        void onInterstitialAdClick(string placementId, ATCallbackInfo callbackInfo);
        /**
        *广告开始播放视频（注意：对于Android来说，所有回调方法均不在Unity的主线程）
        * @param placementId 广告位id
        */
        void onInterstitialAdStartPlayingVideo(string placementId, ATCallbackInfo callbackInfo);
        /**
        *广告视频播放结束（注意：对于Android来说，所有回调方法均不在Unity的主线程）
        * @param placementId 广告位id
        */
        void onInterstitialAdEndPlayingVideo(string placementId, ATCallbackInfo callbackInfo);
        /**
        *广告播放视频失败（注意：对于Android来说，所有回调方法均不在Unity的主线程）
        * @param placementId 广告位id
		* @param code 错误码
		* @param message 错误信息
        */
        void onInterstitialAdFailedToPlayVideo(string placementId, string code, string message);
    }
}
