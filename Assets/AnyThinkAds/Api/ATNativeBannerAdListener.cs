using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnyThinkAds.Api
{
    public interface ATNativeBannerAdListener
    {
		/***
		 * 广告请求成功（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 */
        void onAdLoaded(string placementId);
		/***
		 * 广告请求失败（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 */ 
        void onAdLoadFail(string placementId, string code, string message);
		/***
		 * 广告展示（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 */ 
        void onAdImpressed(string placementId, ATCallbackInfo callbackInfo);
		/**
		 * 广告点击（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 */ 
        void onAdClicked(string placementId, ATCallbackInfo callbackInfo);
        /**
		 * 广告自动刷新（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 */ 
        void onAdAutoRefresh(string placementId, ATCallbackInfo callbackInfo);
        /**
		 * 广告自动刷新失败（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 */ 
		void onAdAutoRefreshFailure(string placementId, string code, string message);
        /**
		 * 关闭按钮被点击（内部逻辑不会移除ad view）（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 */ 
        void onAdCloseButtonClicked(string placementId);
    }
}
