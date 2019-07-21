using System;
using System.ComponentModel;
using System.Net;

namespace Mmosoft.Http
{
    public static class FileDownloader
    {
        /// <summary>
        /// Get file size
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static uint GetFileSizeInByte(string url)
        {
            var req = new HttpRequestHandler().CreateRawRequest(new Uri(url), "HEAD");
            return uint.Parse(req.GetResponse().Headers["Content-Length"]);
        }

        // Download file and report progress
        public static void Download(
            string url, 
            string savePath,
            DownloadProgressChangedEventHandler DownloadProgressChanged,
            AsyncCompletedEventHandler DownloadFileCompleted)
        {
            using (WebClient client = new WebClient())
            {
                if (DownloadFileCompleted != null)
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
                
                if (DownloadProgressChanged != null)
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);

                client.DownloadFileAsync(new Uri(url), savePath);
            }
        }
    }
}
