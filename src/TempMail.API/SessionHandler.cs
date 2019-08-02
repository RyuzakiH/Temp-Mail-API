using CloudflareSolverRe;
using CloudflareSolverRe.CaptchaProviders;
using System;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TempMail.API.Constants;
using TempMail.API.Extensions;
using TempMail.API.Utilities;

namespace TempMail.API
{
    public class SessionHandler
    {
        protected ICaptchaProvider captchaProvider;
        protected IWebProxy proxy;

        internal CookieContainer cookieContainer;

        internal HttpClient HttpClient;


        internal void PrepareHttpRequest(HttpRequestMessage request, [Optional]string accept, [Optional]Uri referrer, [Optional]HttpContent content, bool xml = false, bool origin = false)
        {
            if (content != null)
                request.Content = content;

            if (accept != null)
                request.Headers.Add(HttpHeaders.Accept, accept);

            if (referrer != null)
                request.Headers.Referrer = referrer;

            if (xml)
                request.Headers.Add(HttpHeaders.XRequestedWith, HttpHeaderValues.XmlHttpRequest);

            if (origin)
                request.Headers.Add(HttpHeaders.Origin, request.RequestUri.Host);
        }

        internal HttpResponseMessage SendRequest(HttpMethod method, string requestUrl, [Optional]string accept, [Optional]Uri referrer, [Optional]HttpContent content, bool xml = false, bool origin = false, bool jsDateQuery = false)
        {
            if (jsDateQuery)
                requestUrl += $"{(requestUrl.EndsWith("/") ? "" : @"/")}?_={Utils.GetJavascriptDate()}";

            using (var request = new HttpRequestMessage(method, requestUrl))
            {
                PrepareHttpRequest(request, accept, referrer, content, xml, origin);
                return HttpClient.Send(request);
            }
        }

        internal async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string requestUrl, [Optional]string accept, [Optional]Uri referrer, [Optional]HttpContent content, bool xml = false, bool origin = false, bool jsDateQuery = false)
        {
            if (jsDateQuery)
                requestUrl += $"{(requestUrl.EndsWith("/") ? "" : @"/")}?_={Utils.GetJavascriptDate()}";

            using (var request = new HttpRequestMessage(method, requestUrl))
            {
                PrepareHttpRequest(request, accept, referrer, content, xml, origin);
                return await HttpClient.SendAsync(request);
            }
        }


        internal Cookie GetCookie(string name) => cookieContainer.GetCookie(Urls.BASE_URL, name);

        internal void SetCookie(string name, string value) => cookieContainer.SetCookie(Urls.BASE_URL, name, value);


        protected void CreateHttpClient()
        {
            cookieContainer = new CookieContainer();

            var handler = new ClearanceHandler(captchaProvider)
            {
                InnerHandler = new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    Proxy = proxy
                },
                MaxTries = 5,
                ClearanceDelay = 3000
            };

            HttpClient = new HttpClient(handler);

            HttpClient.DefaultRequestHeaders.Add(HttpHeaders.Accept, HttpHeaderValues.GeneralAccept);
            HttpClient.DefaultRequestHeaders.Add(HttpHeaders.AcceptLanguage, HttpHeaderValues.EnUs);
            HttpClient.DefaultRequestHeaders.Add(HttpHeaders.UserAgent, HttpHeaderValues.Chrome75_Win10);
            HttpClient.DefaultRequestHeaders.Add(HttpHeaders.Host, HttpHeaderValues.Host);
            HttpClient.DefaultRequestHeaders.Add(HttpHeaders.UpgradeInsecureRequests, "1");
            HttpClient.DefaultRequestHeaders.Add(HttpHeaders.Connection, HttpHeaderValues.KeepAlive);
        }
    }
}