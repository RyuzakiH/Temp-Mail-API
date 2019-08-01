using CloudflareSolverRe;
using CloudflareSolverRe.CaptchaProviders;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TempMail.API.Constants;
using TempMail.API.Utilities;

namespace TempMail.API
{
    public class TempMailClient : SessionHandler
    {
        private readonly ICaptchaProvider captchaProvider;
        private IWebProxy proxy;

        private List<string> availableDomains;
        public List<string> AvailableDomains => availableDomains ?? (availableDomains = GetAvailableDomains());

        public Inbox Inbox;

        public string Email { get; set; }


        public TempMailClient([Optional]ICaptchaProvider captchaProvider, [Optional]IWebProxy proxy)
        {
            Inbox = new Inbox(this);

            this.captchaProvider = captchaProvider;
            this.proxy = proxy;
        }

        /// <summary>
        /// Starts a new client session and get a new temporary email.
        /// </summary>
        public void StartNewSession()
        {
            CreateHttpClient();
            
            var document = HttpClient.GetHtmlDocument(Urls.MAIN_PAGE_URL);

            Email = ExtractEmail(document);
        }

        /// <summary>
        /// Starts a new client session and get a new temporary email.
        /// </summary>
        public async Task StartNewSessionAsync()
        {
            await Task.Run(() => CreateHttpClient());
            
            var document = await HttpClient.GetHtmlDocumentAsync(Urls.MAIN_PAGE_URL);

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
            if (!AvailableDomains.Contains(domain))
                throw new Exception(string.Format("The domain you entered is not an available domain: {0}", domain));

            var data = new Dictionary<string, string>
            {
                { "csrf", GetCookie("csrf").Value },
                { "mail", login },
                { "domain", Utils.NormalizeDomain(domain) },
            };

            var response = SendRequest(HttpMethod.Post, Urls.CHANGE_URL,
                referrer: new Uri(Urls.CHANGE_URL), content: new FormUrlEncodedContent(data), origin: true);

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            Email = $"{login}{Utils.NormalizeDomain(domain)}";

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
            if (!AvailableDomains.Contains(domain))
                throw new Exception(string.Format("The domain you entered is not an available domain: {0}", domain));

            var data = new Dictionary<string, string>
            {
                { "csrf", GetCookie("csrf").Value },
                { "mail", login },
                { "domain", Utils.NormalizeDomain(domain) },
            };

            var response = await SendRequestAsync(HttpMethod.Post, Urls.CHANGE_URL,
                referrer: new Uri(Urls.CHANGE_URL), content: new FormUrlEncodedContent(data), origin: true);

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            Email = $"{login}{Utils.NormalizeDomain(domain)}";

            Inbox.Clear();

            return Email;
        }


        /// <summary>
        /// Deletes the temporary email and gets a new one.
        /// </summary>
        public bool Delete()
        {
            var response = SendRequest(HttpMethod.Get, Urls.DELETE_URL,
                "application/json, text/javascript, */*; q=0.01",
                new Uri(Urls.MAIN_PAGE_URL), null, true, false, true);

            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            Email = Regex.Match(response.Content.ReadAsString(), @"{""mail""\s*:\s*""(?<mail>.*?)""}").Groups["mail"].Value;

            UpdateEmailCookie();

            Inbox.Clear();

            return true;
        }

        /// <summary>
        /// Deletes the temporary email and gets a new one.
        /// </summary>
        public async Task<bool> DeleteAsync()
        {
            var response = await SendRequestAsync(HttpMethod.Get, Urls.DELETE_URL,
                "application/json, text/javascript, */*; q=0.01",
                new Uri(Urls.MAIN_PAGE_URL), null, true, false, true);

            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            Email = (await Task.Run(() => Regex.Match(response.Content.ReadAsString(), @"{""mail""\s*:\s*""(?<mail>.*?)""}").Groups["mail"].Value));

            await Task.Run(() => UpdateEmailCookie());

            Inbox.Clear();

            return true;
        }


        private List<string> GetAvailableDomains() =>
            HttpClient.GetHtmlDocument(Urls.CHANGE_URL).GetElementbyId("domain").Descendants("option")
                .Select(s => s.GetAttributeValue("value", null)).ToList();

        private void UpdateEmailCookie() => SetCookie("mail", Email);


        private void CreateHttpClient()
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

            HttpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
            HttpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36");
            HttpClient.DefaultRequestHeaders.Add("Host", "temp-mail.org");
            HttpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            HttpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        }

    }
}
