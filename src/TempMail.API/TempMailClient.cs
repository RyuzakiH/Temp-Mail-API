using CloudflareSolverRe.CaptchaProviders;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using TempMail.API.Constants;
using TempMail.API.Events;
using TempMail.API.Extensions;
using TempMail.API.Utilities;

namespace TempMail.API
{
    public class TempMailClient : SessionHandler, IDisposable
    {
        private bool disposed;

        private CancellationTokenSource CancellationToken;
        
        public bool IsAutoCheckRunning { get => CancellationToken != null && CancellationToken.Token.CanBeCanceled; }

        /// <summary>
        /// Gets available domains.
        /// </summary>
        public List<string> AvailableDomains { get; private set; }

        /// <summary>
        /// Get current email.
        /// </summary>
        public string Email { get => Uri.UnescapeDataString(GetCookie(Cookies.Mail).Value); }
        
        public Inbox Inbox { get; }

        /// <summary>
        /// Occurs when the temporary email changes.
        /// </summary>
        public event EventHandler<EmailChangedEventArgs> EmailChanged;


        /// <summary>
        /// Creates a new temp-mail client.
        /// </summary>
        public static TempMailClient Create([Optional]ICaptchaProvider captchaProvider, [Optional]IWebProxy proxy)
        {
            var client = new TempMailClient(captchaProvider, proxy);
            client.StartSession();
            return client;
        }

        /// <summary>
        /// Creates a new temp-mail client.
        /// </summary>
        public static async Task<TempMailClient> CreateAsync([Optional]ICaptchaProvider captchaProvider, [Optional]IWebProxy proxy)
        {
            var client = new TempMailClient(captchaProvider, proxy);
            await client.StartSessionAsync();
            return client;
        }

        internal TempMailClient([Optional]ICaptchaProvider captchaProvider, [Optional]IWebProxy proxy)
        {
            Inbox = new Inbox(this);
            this.captchaProvider = captchaProvider;
            this.proxy = proxy;
        }


        internal void StartSession()
        {
            CreateHttpClient();

            EmailChanged += (o, e) => Inbox.Clear();

            LoadMainPage();

            AvailableDomains = GetAvailableDomains();
        }

        internal async Task StartSessionAsync()
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
            ThrowIfDisposed();

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
            ThrowIfDisposed();

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
            ThrowIfDisposed();

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
            ThrowIfDisposed();

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


        /// <summary>
        /// Starts inbox mails auto checking every delay of time.
        /// </summary>
        public void StartAutoCheck(int delay = 10000)
        {
            ThrowIfDisposed();

            if (IsAutoCheckRunning)
                return;

            CancellationToken = new CancellationTokenSource();

            RunAutoCheck(delay);
        }

        private async Task RunAutoCheck(int delay = 10000)
        {
            while (CancellationToken != null && !CancellationToken.IsCancellationRequested)
            {
                await Inbox.RefreshAsync();
                await Task.Delay(delay);
            }
        }

        /// <summary>
        /// Stops inbox mails auto checking.
        /// </summary>
        public void StopAutoCheck()
        {
            ThrowIfDisposed();

            if (!IsAutoCheckRunning)
                return;

            CancellationToken.Cancel();
            CancellationToken = null;
        }


        public void Dispose()
        {
            HttpClient.Dispose();
            cookieContainer = null;

            AvailableDomains.Clear();
            Inbox.Clear();

            disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException("TempMailClient");
        }

        private void LoadMainPage() => HttpClient.GetString(Urls.MAIN_PAGE_URL);
        private async Task LoadMainPageAsync() => await HttpClient.GetStringAsync(Urls.MAIN_PAGE_URL);

        private List<string> GetAvailableDomains() => Parser.GetAvailableDomains(HttpClient.GetString(Urls.CHANGE_URL));
        private async Task<List<string>> GetAvailableDomainsAsync() =>
            Parser.GetAvailableDomains(await HttpClient.GetStringAsync(Urls.CHANGE_URL));

        private void UpdateEmailCookie(string email) => SetCookie(Cookies.Mail, email);

    }
}