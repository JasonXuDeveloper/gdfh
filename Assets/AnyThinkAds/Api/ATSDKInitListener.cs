using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnyThinkAds.Api
{
    public interface ATSDKInitListener
    {

        void initSuccess();
        void initFail(string message);
    }
}
