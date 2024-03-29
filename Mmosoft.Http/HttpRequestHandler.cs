﻿using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace Mmosoft.Http
{
    public class HttpRequestHandler
    {
        /// <summary>
        /// Contain cookie for making authorized request 
        /// </summary>
        public virtual CookieContainer CookieContainer { get; set; }

        /// <summary>
        /// User-agent
        /// </summary>
        public virtual string UserAgent { get; set; }

        public bool SkipExpect100Continue { get; set; }

        /// <summary>
        /// Initialize new instance of BaseHttp request with default user agent
        /// </summary>
        public HttpRequestHandler()
            : this("Mozilla/5.0 (Windows NT 10.0; WOW64; rv:46.0) Gecko/20100101 Firefox/46.0")
        {
            CookieContainer = new CookieContainer();
        }

        /// <summary>
        /// Initialize new instance of BaseHttp request.
        /// </summary>
        /// <param name="userAgent">User agent</param>
        public HttpRequestHandler(string userAgent)
        {
            UserAgent = userAgent;
        }

        /// <summary>
        /// Make GET request but not send
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public virtual HttpWebRequest CreateGETRequest(Uri requestUri)
        {
            return this.CreateRawRequest(requestUri, WebRequestMethods.Http.Get);
        }

        /// <summary>
        /// Make POST request but not send
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual HttpWebRequest CreatePOSTRequest(Uri requestUri, byte[] content)
        {
            var request = this.CreateRawRequest(requestUri, WebRequestMethods.Http.Post);
            // set content
            request.ContentLength = content.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            return request;
        }

        /// <summary>
        /// Send GET request to requestUri and recv response
        /// </summary>
        /// <param name="requestUri">A string containing the URI of the requested resource</param>
        /// <param name="cookies">A System.Net.CookieContainer containing cookies for this request</param>        
        /// <returns>A System.Net.HttpWebResponse for the specified uri scheme</returns>
        public virtual HttpWebResponse SendGETRequest(string requestUrl)
        {
            var uri = new Uri(requestUrl);
            var request = this.CreateGETRequest(uri);
            var response = request.GetResponse() as HttpWebResponse;
            this.storeCookie(response.Cookies);
            return response;
        }

        /// <summary>
        /// Send POST request to requestUri and recv response
        /// </summary>
        /// <param name="requestUrl">A System.String containing the URI of the requested resource</param>
        /// <param name="content">A System.String containing the content to send</param>
        /// <param name="cookies">A System.Net.CookieContainer containing cookies for this request</param>        
        /// <returns>A System.Net.HttpWebResponse for the specified uri scheme</returns>
        public virtual HttpWebResponse SendPOSTRequest(string requestUrl, string content)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            var postRequest = this.CreatePOSTRequest(new Uri(requestUrl), buffer);
            using (var postRequestStream = postRequest.GetRequestStream())
                postRequestStream.Write(buffer, 0, buffer.Length);
            var response = postRequest.GetResponse() as HttpWebResponse;
            this.storeCookie(response.Cookies);
            return response;
        }

        /// <summary>
        /// Download Html content
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public virtual string DownloadContent(string requestUrl)
        {
            using (var response = this.SendGETRequest(requestUrl))
            {
                var contentEncoding = response.Headers["content-encoding"];
                if (contentEncoding != null && contentEncoding.Contains("gzip")) // cause httphandler only request gzip
                {
                    // using gzip stream reader
                    using (var responseStreamReader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress)))
                    {
                        return responseStreamReader.ReadToEnd();
                    }
                }
                else
                {
                    // using ordinary stream reader
                    using (var responseStreamReader = new StreamReader(response.GetResponseStream()))
                    {
                        return responseStreamReader.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// Redirect request process
        /// </summary>
        /// <param name="location">target location</param>
        /// <returns>HttpWebResponse instance</returns>
        private HttpWebResponse redirectRequestProcess(string location)
        {
            // Redirect and disable delay request to send request immediate
            return SendGETRequest(location);
        }

        /// <summary>
        /// Create a request
        /// </summary>
        /// <param name="requestUri">A System.Uri containing the URI of the requested resource</param>
        /// <param name="cookieContainer">A System.Net.CookieContainer containing cookies for this request</param>              
        /// <returns>A System.Net.HttpWebRequest for the specified URI scheme.</returns>
        public HttpWebRequest CreateRawRequest(Uri requestUri, string method)
        {
            var request = WebRequest.Create(requestUri) as HttpWebRequest;
            request.Accept = "*/*";
            request.AllowAutoRedirect = false;
            request.CookieContainer = this.CookieContainer;
            request.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            request.Headers.Add("Accept-Encoding", "gzip");
            request.Host = requestUri.Host + ":" + requestUri.Port;
            request.KeepAlive = true;
            request.Method = method;
            request.ProtocolVersion = HttpVersion.Version11;
            if (SkipExpect100Continue)
                request.ServicePoint.Expect100Continue = false;
            request.UserAgent = this.UserAgent;
            return request;
        }

        /// <summary>
        /// Store new cookie to cookie containter
        /// </summary>
        /// <param name="cookies"></param>
        private void storeCookie(CookieCollection cookies)
        {
            if (cookies != null && cookies.Count > 0)
                this.CookieContainer.Add(cookies);
        }
    }
}