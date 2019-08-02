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
        internal CookieContainer cookieContainer;

        internal HttpClient HttpClient;


        internal void PrepareHttpRequest(HttpRequestMessage request, [Optional]string accept, [Optional]Uri referrer, [Optional]HttpContent content, bool xml = false, bool origin = false)
        {
            if (content != null)
                request.Content = content;

            if (accept != null)
                request.Headers.Add("Accept", accept);

            if (referrer != null)
                request.Headers.Referrer = referrer;

            if (xml)
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            if (origin)
                request.Headers.Add("Origin", request.RequestUri.Host);
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

    }
}