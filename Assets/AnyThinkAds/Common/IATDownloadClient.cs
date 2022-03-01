
using AnyThinkAds.Api;

namespace AnyThinkAds.Common
{
    public interface IATDownloadClient
    {
		
		/**
		 * @param listener 
		 */ 
        void setListener(ATDownloadAdListener listener);
	}
}
