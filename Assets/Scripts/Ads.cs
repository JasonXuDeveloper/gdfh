using System;
using System.Collections.Generic;
using AnyThinkAds.Api;
using AnyThinkAds.Common;
using AnyThinkAds;
using UnityEngine.Advertisements;
using UnityEngine;

public class Ads : MonoBehaviour
{
    public string appID = "a616d1fba511f4";
    public string appKey = "48bdd240756200e6504bf9a3fa0a41b3";
    public string placement = "b616d1fca204be";
    
    public static Ads instance;

    private bool _ready;

    void Awake()
    {
        InitializeAds();
        instance = this;
    }

    public void InitializeAds()
    {
        //设置开启Debug日志（强烈建议测试阶段开启，方便排查问题）
        // ATSDKAPI.setLogDebug(true);

        //判断是否在欧盟地区
        ATSDKAPI.getUserLocation(new GetLocationListener());

        //（必须配置）SDK的初始化
        ATSDKAPI.initSDK(appID, appKey, new AInitListener()); //Use your own app_id & app_key here
    }

    //发布欧盟地区的开发者需使用以下授权代码，询问用户是否同意收集隐私数据
    private class GetLocationListener : ATGetUserLocationListener
    {
        public void didGetUserLocation(int location)
        {
            Debug.Log("Developer callback didGetUserLocation(): " + location);
            if (location == ATSDKAPI.kATUserLocationInEU && ATSDKAPI.getGDPRLevel() == ATSDKAPI.UNKNOWN)
            {
                ATSDKAPI.showGDPRAuth();
            }
        }
    }

    private ATCallbackListener _callbackListener;
    public void LoadVideo()
    {
        if (_callbackListener == null)
        {
            _callbackListener = new ATCallbackListener();
            Debug.Log("Developer init video....placementId:" + placement);
            ATRewardedVideo.Instance.setListener(_callbackListener);
        }

        Dictionary<string, string> jsonmap = new Dictionary<string, string>();
        //如果需要通过开发者的服务器进行奖励的下发（部分广告平台支持此服务器激励），则需要传递下面两个key
        //ATConst.USERID_KEY必传，用于标识每个用户;ATConst.USER_EXTRA_DATA为可选参数，传入后将透传到开发者的服务器
        // jsonmap.Add(ATConst.USERID_KEY, "test_user_id");
        // jsonmap.Add(ATConst.USER_EXTRA_DATA, "test_user_extra_data");

        ATRewardedVideo.Instance.loadVideoAd(placement, jsonmap);
    }

    public void PlayAd(Action success, Action error, Action invalid)
    {
        if (!ATRewardedVideo.Instance.hasAdReady(placement))
        {
            invalid();
            LoadVideo();
            return;
        }

        Ads.success = success;
        Ads.error = error;
        ATRewardedVideo.Instance.showAd(placement);
    }

    private static Action success;
    private static Action error;

    class AInitListener : ATSDKInitListener
    {
        public void initSuccess()
        {
            instance.LoadVideo();
            Debug.Log("初始化成功");
        }

        public void initFail(string message)
        {
            Debug.Log("初始化失败："+message);
        }
    }

    class ATCallbackListener : ATRewardedVideoListener
    {
        //广告加载成功
        public void onRewardedVideoAdLoaded(string placementId)
        {
            Debug.Log("Developer onRewardedVideoAdLoaded------");
            instance._ready = true;
        }

        //广告加载失败
        public void onRewardedVideoAdLoadFail(string placementId, string code, string message)
        {
            Debug.Log("Developer onRewardedVideoAdLoadFail------:code" + code + "--message:" + message);
        }

        //广告开始播放
        public void onRewardedVideoAdPlayStart(string placementId, ATCallbackInfo callbackInfo)
        {
            Debug.Log("Developer onRewardedVideoAdPlayStart------");
        }

        //广告播放结束
        public void onRewardedVideoAdPlayEnd(string placementId, ATCallbackInfo callbackInfo)
        {
            Debug.Log("Developer onRewardedVideoAdPlayEnd------");
            instance.LoadVideo();
        }

        //广告播放失败
        public void onRewardedVideoAdPlayFail(string placementId, string code, string message)
        {
            Debug.Log("Developer onRewardedVideoAdPlayFail------code:" + code + "---message:" + message);
            error?.Invoke();
        }

        //广告被关闭，其中isReward仅表示onRewardedVideoAdPlayClosed被回调时onReward()方法被回调了没有
        public void onRewardedVideoAdPlayClosed(string placementId, bool isReward, ATCallbackInfo callbackInfo)
        {
            Debug.Log("Developer onRewardedVideoAdPlayClosed------isReward:" + isReward);
            instance.LoadVideo();
        }

        //广告被点击
        public void onRewardedVideoAdPlayClicked(string placementId, ATCallbackInfo callbackInfo)
        {
            Debug.Log("Developer onRewardVideoAdPlayClicked------");
        }

        //激励成功，开发者可在此回调中下发奖励，一般先于onRewardedVideoAdPlayClosed回调，服务器激励则不一定
        public void onReward(string placementId, ATCallbackInfo callbackInfo)
        {
            Debug.Log("Developer onReward------");
            success?.Invoke();
            Debug.Log("success");
        }
    }
}