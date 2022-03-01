
using AnyThinkAds.Common;


namespace AnyThinkAds.Api
{
    public class ATDownloadManager
    {
        private static readonly ATDownloadManager instance = new ATDownloadManager();
        private IATDownloadClient client;

        private ATDownloadManager()
        {
            client = GetATDownloadClient();
        }

        public static ATDownloadManager Instance
        {
            get
            {
                return instance;
            }
        }

		public void setListener(ATDownloadAdListener listener)
        {   
            client.setListener(listener);
        }

        public IATDownloadClient GetATDownloadClient()
        {
            return AnyThinkAds.ATAdsClientFactory.BuildDownloadClient();
        }

    }
}