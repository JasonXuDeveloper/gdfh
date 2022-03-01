using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;



using AnyThinkAds.Common;
using AnyThinkAds.ThirdParty.LitJson;


namespace AnyThinkAds.Api
{
    public interface ATGetUserLocationListener
    {
        void didGetUserLocation(int location);
    }

    public interface ATGetAreaListener
    {
        void onArea(string area);
        void onError(string message);
    }

    public class ATSDKAPI
    {
        public static readonly int kATUserLocationUnknown = 0;
        public static readonly int kATUserLocationInEU = 1;
        public static readonly int kATUserLocationOutOfEU = 2;

        public static readonly int PERSONALIZED = 0;
        public static readonly int NONPERSONALIZED = 1;
        public static readonly int UNKNOWN = 2;

        public static readonly int AREA_GLOBAL = 0;
        public static readonly int AREA_CHINESE_MAINLAND = 1;


        //for android and ios
        public static readonly string OS_VERSION_NAME = "os_vn";
        public static readonly string OS_VERSION_CODE = "os_vc";
        public static readonly string APP_PACKAGE_NAME = "package_name";
        public static readonly string APP_VERSION_NAME = "app_vn";
        public static readonly string APP_VERSION_CODE = "app_vc";

        public static readonly string BRAND = "brand";
        public static readonly string MODEL = "model";
        public static readonly string DEVICE_SCREEN_SIZE = "screen";
        public static readonly string MNC = "mnc";
        public static readonly string MCC = "mcc";

        public static readonly string LANGUAGE = "language";
        public static readonly string TIMEZONE = "timezone";
        public static readonly string USER_AGENT = "ua";
        public static readonly string ORIENTATION = "orient";
        public static readonly string NETWORK_TYPE = "network_type";

        //for android
        public static readonly string INSTALLER = "it_src";
        public static readonly string ANDROID_ID = "android_id";
        public static readonly string GAID = "gaid";
        public static readonly string MAC = "mac";
        public static readonly string IMEI = "imei";
        public static readonly string OAID = "oaid";

        //for ios
        public static readonly string IDFA = "idfa";
        public static readonly string IDFV = "idfv";



        private static readonly IATSDKAPIClient client = GetATSDKAPIClient();

        public static void initSDK(string appId, string appKey)
        {
            client.initSDK(appId, appKey);
        }

        public static void initSDK(string appId, string appKey, ATSDKInitListener listener)
        {
            client.initSDK(appId, appKey, listener);
        }

        public static void setGDPRLevel(int level)
        {
            client.setGDPRLevel(level);
        }

        public static void getUserLocation(ATGetUserLocationListener listener)
        {
            client.getUserLocation(listener);
        }

        public static int getGDPRLevel() {
            return client.getGDPRLevel();
        }

        public static bool isEUTraffic() {
            return client.isEUTraffic();
        }

        public static void setChannel(string channel)
        {
            client.setChannel(channel);
        }

        public static void setSubChannel(string subChannel)
        {
            client.setSubChannel(subChannel);
        }

        public static void initCustomMap(Dictionary<string, string> customMap)
        {
            client.initCustomMap(JsonMapper.ToJson(customMap));
        }

        public static void setCustomDataForPlacementID(Dictionary<string, string> customData, string placementID)
        {
            client.setCustomDataForPlacementID(JsonMapper.ToJson(customData), placementID);
        }

        public static void showGDPRAuth()
        {
            client.showGDPRAuth();
        }

        public static void setLogDebug(bool isDebug)
        {
            client.setLogDebug(isDebug);
        }

		public static void addNetworkGDPRInfo(int networkType, Dictionary<string,object> dictionary)
        {
            client.addNetworkGDPRInfo(networkType, JsonMapper.ToJson(dictionary));
        }

        public static void deniedUploadDeviceInfo(string[] deniedInfo)
        {
            if (deniedInfo != null && deniedInfo.Length > 0)
            {
                string deniedString = JsonMapper.ToJson(deniedInfo);
                client.deniedUploadDeviceInfo(deniedString);
                Debug.Log("deniedUploadDeviceInfo, deniedInfo === " + deniedString);
            }
            else
            {
                Debug.Log("deniedUploadDeviceInfo, deniedInfo = null");
            }
            
        }

        private static IATSDKAPIClient GetATSDKAPIClient(){
            Debug.Log("GetATSDKAPIClient");
            return AnyThinkAds.ATAdsClientFactory.BuildSDKAPIClient();
        }

        public static void setExcludeBundleIdArray(string[] bundleIds)
        {
            if (bundleIds != null && bundleIds.Length > 0)
            {
                string bundleIdsString = JsonMapper.ToJson(bundleIds);
                Debug.Log("setExcludeBundleIdArray, bundleIdsString === " + bundleIdsString);

                client.setExcludeBundleIdArray(bundleIdsString);      
            }
            else
            {
                Debug.Log("setExcludeBundleIdArray, bundleIdsString = null");
            }

        }

        public static void setExcludeAdSourceIdArrayForPlacementID(string placementID, string[] adSourceIds)
        {
            if (adSourceIds != null && adSourceIds.Length > 0)
            {
                string adSourceIdsString = JsonMapper.ToJson(adSourceIds);
                Debug.Log("setExcludeAdSourceIdArrayForPlacementID, adSourceIdsString === " + adSourceIdsString);

                client.setExcludeAdSourceIdArrayForPlacementID(placementID, adSourceIdsString);   
            }
            else
            {
                Debug.Log("setExcludeAdSourceIdArrayForPlacementID, adSourceIdsString = null");
            }

        }

        public static void setSDKArea(int area)
        {
            client.setSDKArea(area);
        }

        public static void getArea(ATGetAreaListener listener)
        {
            client.getArea(listener);
        }

        public static void setWXStatus(bool install)
        {
            client.setWXStatus(install);
        }

        public static void setLocation(double longitude, double latitude)
        {
            client.setLocation(longitude, latitude);
        }

    }
}

