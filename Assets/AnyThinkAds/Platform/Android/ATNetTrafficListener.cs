using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnyThinkAds.Api;

namespace AnyThinkAds.Android
{
    public class ATNetTrafficListener : AndroidJavaProxy
    {
        ATGetUserLocationListener mListener;
        public ATNetTrafficListener(ATGetUserLocationListener listener): base("com.anythink.unitybridge.sdkinit.SDKEUCallbackListener")
        {
            mListener = listener;
        }


        public void onResultCallback(bool isEu)
        {
            if (mListener != null)
            {
                if (isEu)
                {
                    mListener.didGetUserLocation(ATSDKAPI.kATUserLocationInEU);
                }
                else
                {
                    mListener.didGetUserLocation(ATSDKAPI.kATUserLocationOutOfEU);
                }
            }
        }

        public void onErrorCallback(string s)
        {
            if (mListener != null)
            {
               mListener.didGetUserLocation(ATSDKAPI.kATUserLocationUnknown);
            }
        }
    }
}
