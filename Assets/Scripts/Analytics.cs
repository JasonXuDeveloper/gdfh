using UnityEngine;
using Tianti;

public class Analytics : MonoBehaviour
{
    public string appId;
    public string channel;
    
    private void Awake()
    {
        AppLogger.init(appId, channel);
    }
}
