using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnyThinkAds.Api;

namespace AnyThinkAds.Common
{
    public interface IATSDKAPIClient
    {
        void initSDK(string appId, string appKey);
        void initSDK(string appId, string appKey, ATSDKInitListener listener);
        void getUserLocation(ATGetUserLocationListener listener);
        void setGDPRLevel(int level);
        void showGDPRAuth();
        void addNetworkGDPRInfo(int networkType, string mapJson);
        void setChannel(string channel);
        void setSubChannel(string subchannel);
        void initCustomMap(string cutomMap);
        void setCustomDataForPlacementID(string customData, string placementID);
        void setLogDebug(bool isDebug);
        int getGDPRLevel();
        bool isEUTraffic();
        void deniedUploadDeviceInfo(string deniedInfo);

        void setExcludeBundleIdArray(string bundleIds);
        void setExcludeAdSourceIdArrayForPlacementID(string placementID, string adsourceIds);
        void setSDKArea(int area);
        void getArea(ATGetAreaListener listener);
        void setWXStatus(bool install);
        void setLocation(double longitude, double latitude);
    }
}
