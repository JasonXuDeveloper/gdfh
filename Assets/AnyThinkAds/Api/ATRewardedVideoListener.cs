using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnyThinkAds.Api
{
    public interface ATRewardedVideoListener
    {
		/***
		 * 加载广告成功（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 */ 
        void onRewardedVideoAdLoaded(string placementId);
		/***
		 * 加载广告失败（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 */ 
        void onRewardedVideoAdLoadFail(string placementId,string code, string message);
		/***
		 * 视频播放开始（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 */ 
        void onRewardedVideoAdPlayStart(string placementId, ATCallbackInfo callbackInfo);
		/***
		 * 视频播放结束（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 */ 
        void onRewardedVideoAdPlayEnd(string placementId, ATCallbackInfo callbackInfo);
		/***
		 * 视频播放失败（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 * @param code 错误码
		 * @param message 错误信息
		 */ 
        void onRewardedVideoAdPlayFail(string placementId,string code, string message);
		/**
		 * 视频页面关闭（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 * @param isReward 视频是否播放完成
		 */ 
        void onRewardedVideoAdPlayClosed(string placementId,bool isReward, ATCallbackInfo callbackInfo);
		/***
		 * 视频点击（注意：对于Android来说，所有回调方法均不在Unity的主线程）
		 */ 
        void onRewardedVideoAdPlayClicked(string placementId, ATCallbackInfo callbackInfo);
        /**
         * 激励下发（注意：对于Android来说，所有回调方法均不在Unity的主线程）
         */
        void onReward(string placementId, ATCallbackInfo callbackInfo);

    }
}
