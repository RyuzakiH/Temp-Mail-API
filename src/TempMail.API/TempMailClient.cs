using CloudflareSolverRe;
using CloudflareSolverRe.CaptchaProviders;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TempMail.API.Constants;
using TempMail.API.Events;
using TempMail.API.Extensions;
using TempMail.API.Utilities;

namespace TempMail.API
{
    public class TempMailClient : SessionHandler
    {
        private readonly ICaptchaProvider captchaProvider;
        private IWebProxy proxy;

        public List<string> AvailableDomains { get; private set; }

        public string Email { get => Uri.UnescapeDataString(GetCookie(Cookies.Mail).Value); }

        public Inbox Inbox { get; }

        public event EventHandler<EmailChangedEventArgs> EmailChanged;


        public static TempMailClient Create([Optional]ICaptchaProvider captchaProvider, [Optional]IWebProxy proxy)
        {
            var client = new TempMailClient(captchaProvider, proxy);
            client.StartNewSession();
            return client;
        }

        public static async Task<TempMailClient> CreateAsync([Optional]ICaptchaProvider captchaProvider, [Optional]IWebProxy proxy)
        {
            var client = new TempMailClient(captchaProvider, proxy);
            await client.StartNewSessionAsync();
            return client;
        }

        internal TempMailClient([Optional]ICaptchaProvider captchaProvider, [Optional]IWebProxy proxy)
        {
            Inbox = new Inbox(this);
            this.captchaProvider = captchaProvider;
            this.proxy = proxy;
        }


        /// <summary>
        /// Starts a new client session and get a new temporary email.
        /// </summary>
        internal void StartNewSession()
        {
            CreateHttpClient();

            EmailChanged += (o, e) => Inbox.Clear();

            LoadMainPage();

            AvailableDomains = GetAvailableDomains();
        }

        /// <summary>
        /// Starts a new client session and get a new temporary email.
        /// </summary>
        internal async Task StartNewSessionAsync()
        {
            await Task.Run(() => CreateHttpClient());

            EmailChanged += (o, e) => Inbox.Clear();

            await LoadMainPageAsync();

            AvailableDomains = await GetAvailableDomainsAsync();
        }


        /// <summary>
        /// Changes the temporary email to ex: login@domain
        /// </summary>
        /// <param name="login">New temporary email login</param>
        /// <param name="domain">New temporary email domain</param>
        public string ChangeEmail(string login, string domain)
        {
            if (!AvailableDomains.Contains(domain))
                throw new Exception($"{Errors.InvalidDomain}: {domain}");

            var data = new Dictionary<string, string>
            {
                { Cookies.Csrf, GetCookie(Cookies.Csrf).Value },
                { Cookies.Mail, login },
                { General.Domain, Utils.NormalizeDomain(domain) }
            };

            var response = SendRequest(HttpMethod.Post, Urls.CHANGE_URL,
                referrer: new Uri(Urls.CHANGE_URL), content: new FormUrlEncodedContent(data), origin: true);

            if (!response.IsSuccessStatusCode)
                return null;

            EmailChanged?.Invoke(this, new EmailChangedEventArgs(Email));

            return Email;
        }

        /// <summary>
        /// Changes the temporary email to ex: login@domain
        /// </summary>
        /// <param name="login">New temporary email login</param>
        /// <param name="domain">New temporary email domain</param>
        public async Task<string> ChangeEmailAsync(string login, string domain)
        {
            if (!AvailableDomains.Contains(domain))
                throw new Exception($"{Errors.InvalidDomain}: {domain}");

            var data = new Dictionary<string, string>
            {
                { Cookies.Csrf, GetCookie(Cookies.Csrf).Value },
                { Cookies.Mail, login },
                { General.Domain, Utils.NormalizeDomain(domain) }
            };

            var response = await SendRequestAsync(HttpMethod.Post, Urls.CHANGE_URL,
                referrer: new Uri(Urls.CHANGE_URL), content: new FormUrlEncodedContent(data), origin: true);

            if (!response.IsSuccessStatusCode)
                return null;

            EmailChanged?.Invoke(this, new EmailChangedEventArgs(Email));

            return Email;
        }


        /// <summary>
        /// Deletes the temporary email and gets a new one.
        /// </summary>
        public bool Delete()
        {
            var response = SendRequest(HttpMethod.Get, Urls.DELETE_URL,
                HttpHeaderValues.JsonAccept, new Uri(Urls.MAIN_PAGE_URL), null, true, false, true);
            
            if (!response.IsSuccessStatusCode)
                return false;

            UpdateEmail(response);

            return true;
        }

        /// <summary>
        /// Deletes the temporary email and gets a new one.
        /// </summary>
        public async Task<bool> DeleteAsync()
        {
            var response = await SendRequestAsync(HttpMethod.Get, Urls.DELETE_URL,
                HttpHeaderValues.JsonAccept, new Uri(Urls.MAIN_PAGE_URL), null, true, false, true);

            if (!response.IsSuccessStatusCode)
                return false;

            await UpdateEmailAsync(response);

            return true;
        }

        private void UpdateEmail(HttpResponseMessage response)
        {
            var result = response.Content.ReadAsString();

            var email = Parser.GetEmail(result);
            UpdateEmailCookie(email);

            EmailChanged?.Invoke(this, new EmailChangedEventArgs(Email));
        }

        private async Task UpdateEmailAsync(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();

            var email = await Task.Run(() => Parser.GetEmail(result));
            UpdateEmailCookie(email);

            EmailChanged?.Invoke(this, new EmailChangedEventArgs(Email));
        }
        

        private void LoadMainPage() => HttpClient.GetString(Urls.MAIN_PAGE_URL);
        private async Task LoadMainPageAsync() => await HttpClient.GetStringAsync(Urls.MAIN_PAGE_URL);

        private List<string> GetAvailableDomains() => Parser.GetAvailableDomains(HttpClient.GetString(Urls.CHANGE_URL));
        private async Task<List<string>> GetAvailableDomainsAsync() =>
            Parser.GetAvailableDomains(await HttpClient.GetStringAsync(Urls.CHANGE_URL));

        private void UpdateEmailCookie(string email) => SetCookie(Cookies.Mail, email);


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

            HttpClient.DefaultRequestHeaders.Add(HttpHeaders.Accept, HttpHeaderValues.GeneralAccept);
            HttpClient.DefaultRequestHeaders.Add(HttpHeaders.AcceptLanguage, HttpHeaderValues.EnUs);
            HttpClient.DefaultRequestHeaders.Add(HttpHeaders.UserAgent, HttpHeaderValues.Chrome75_Win10);
            HttpClient.DefaultRequestHeaders.Add(HttpHeaders.Host, HttpHeaderValues.Host);
            HttpClient.DefaultRequestHeaders.Add(HttpHeaders.UpgradeInsecureRequests, "1");
            HttpClient.DefaultRequestHeaders.Add(HttpHeaders.Connection, HttpHeaderValues.KeepAlive);
        }

    }
}