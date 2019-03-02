using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TempMail.API
{
    public class Client
    {
        public const string BASE_URL = "https://temp-mail.org";
        public const string MAIN_PAGE_URL = "https://temp-mail.org/en/";
        public const string CHANGE_URL = "https://temp-mail.org/en/option/change/";
        public const string CHECK_URL = "https://temp-mail.org/en/option/check/";
        public const string DELETE_URL = "https://temp-mail.org/en/option/delete/";

        private static readonly Encoding encoding = Encoding.UTF8;

        private CookieContainer cookies;

        private List<string> availableDomains;
        public List<string> AvailableDomains => availableDomains ?? (availableDomains = GetAvailableDomains());


        private Change change;

        public Inbox Inbox;

        public string Email { get; set; }

        public HttpClient HttpClient { get; private set; }

        public IWebProxy Proxy { get; set; }

        public Client([Optional]IWebProxy proxy)
        {
            Inbox = new Inbox(this);

            change = new Change(this);

            Proxy = proxy;
        }

        /// <summary>
        /// Starts a new client session and get a new temporary email.
        /// </summary>
        public void StartNewSession()
        {
            CreateHttpClient();

            var document = HttpClient.GetHtmlDocument(MAIN_PAGE_URL);

            Email = ExtractEmail(document);
        }

        /// <summary>
        /// Starts a new client session and get a new temporary email.
        /// </summary>
        public async Task StartNewSessionAsync()
        {
            await Task.Run(() => CreateHttpClient());

            var document = await HttpClient.GetHtmlDocumentAsync(MAIN_PAGE_URL);

            Email = await Task.Run(() => ExtractEmail(document));
        }

        private string ExtractEmail(HtmlDocument document)
        {
            return document.GetElementbyId("mail")?.GetAttributeValue("value", null);
        }
        
        
        /// <summary>
        /// Changes the temporary email to ex: login@domain
        /// </summary>
        /// <param name="login">New temporary email login</param>
        /// <param name="domain">New temporary email domain</param>
        public string Change(string login, string domain)
        {
            Email = change.ChangeEmail(login, domain);

            Inbox.Clear();

            return Email;
        }

        /// <summary>
        /// Changes the temporary email to ex: login@domain
        /// </summary>
        /// <param name="login">New temporary email login</param>
        /// <param name="domain">New temporary email domain</param>
        public async Task<string> ChangeAsync(string login, string domain)
        {
            Email = await change.ChangeEmailAsync(login, domain);

            Inbox.Clear();

            return Email;
        }


        /// <summary>
        /// Deletes the temporary email and gets a new one.
        /// </summary>
        public bool Delete()
        {
            HttpResponseMessage response;
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, DELETE_URL))
            {
                requestMessage.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                requestMessage.Headers.Referrer = new Uri(BASE_URL);
                requestMessage.Headers.Add("X-Requested-With", "XMLHttpRequest");
                response = HttpClient.Send(requestMessage);
            }
            
            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            Email = response.Content.ReadAsJsonObject<Dictionary<string, object>>(encoding)?["mail"].ToString();

            UpdateEmailCookie();

            Inbox.Clear();

            return true;
        }

        /// <summary>
        /// Deletes the temporary email and gets a new one.
        /// </summary>
        public async Task<bool> DeleteAsync()
        {
            HttpResponseMessage response;
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, DELETE_URL))
            {
                requestMessage.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                requestMessage.Headers.Referrer = new Uri(BASE_URL);
                requestMessage.Headers.Add("X-Requested-With", "XMLHttpRequest");
                response = await HttpClient.SendAsync(requestMessage);
            }

            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            Email = (await Task.Run(() => response.Content.ReadAsJsonObject<Dictionary<string, object>>(encoding)))?["mail"].ToString();

            await Task.Run(() => UpdateEmailCookie());

            Inbox.Clear();

            return true;
        }


        private List<string> GetAvailableDomains()
        {
            return HttpClient.GetHtmlDocument(CHANGE_URL).GetElementbyId("domain").Descendants("option")
                .Select(s => s.GetAttributeValue("value", null)).ToList();
        }

        public Cookie GetCsrfCookie()
        {
            return cookies.GetCookies(new Uri(BASE_URL))["csrf"];
        }

        private void UpdateEmailCookie()
        {
            cookies.SetCookies(new Uri(BASE_URL), $"mail={Email}");
        }


        private void CreateHttpClient()
        {
            cookies = new CookieContainer();

            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = cookies,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                Proxy = Proxy
                //AllowAutoRedirect = true
            };

            HttpClient = new HttpClient(handler);

            HttpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            //HttpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            HttpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en");
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.109 Safari/537.36");
            HttpClient.DefaultRequestHeaders.Add("Host", "temp-mail.org");
            //HttpClient.DefaultRequestHeaders.Add("Origin", "https://temp-mail.org");
            //HttpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            HttpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            HttpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        }

    }
}
