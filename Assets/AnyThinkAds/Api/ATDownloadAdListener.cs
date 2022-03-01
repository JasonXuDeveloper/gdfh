

namespace AnyThinkAds.Api
{
    public interface ATDownloadAdListener
    {
		
        void onDownloadStart(string placementId, ATCallbackInfo callbackInfo, long totalBytes, long currBytes, string fileName, string appName);
        
        void onDownloadUpdate(string placementId, ATCallbackInfo callbackInfo, long totalBytes, long currBytes, string fileName, string appName);
        
        void onDownloadPause(string placementId, ATCallbackInfo callbackInfo, long totalBytes, long currBytes, string fileName, string appName);
		
        void onDownloadFinish(string placementId, ATCallbackInfo callbackInfo, long totalBytes, string fileName, string appName);
        
        void onDownloadFail(string placementId, ATCallbackInfo callbackInfo, long totalBytes, long currBytes, string fileName, string appName);
        
        void onInstalled(string placementId, ATCallbackInfo callbackInfo, string fileName, string appName);
       
    }
}
