using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using HtmlAgilityPack;

namespace TempMail
{
    public class Session
    {
        private readonly HtmlDocument _document;

        private CookieContainer _cookies;


        public List<string> AvailableDomains => GetAvailableDomains();

        public Inbox Inbox;

        public string Email { get; set; }



        public Session()
        {
            _document = new HtmlDocument();
            _cookies = new CookieContainer();

            Inbox = new Inbox(this);

            CreateNewSession();
        }


        /// <summary>
        /// Changes the temporary email to ex: login@domain .
        /// </summary>
        /// <param name="login">New temporary email login</param>
        /// <param name="domain">New temporary email domain</param>
        public string Change(string login, string domain)
        {
            if (!AvailableDomains.Contains(domain))
                throw new Exception("The domain you entered isn't an available domain");

            using (var client = CreateHttpClient())
            {
                var csrf = GetCsrfCookie(client);
                var data = BuildUploadString(csrf.Value, login, domain);

                AddUploadHeaders(client);

                client.UploadString("https://temp-mail.org/en/option/change", data);

                if (client.StatusCode != HttpStatusCode.OK)
                    return null;

                return (Email = $"{login}{NormalizeDomain(domain)}");
            }
        }

        private static Cookie GetCsrfCookie(HttpClient client)
        {
            return client.CookieContainer.GetCookies(new Uri("https://temp-mail.org/"))["csrf"];
        }

        private void AddUploadHeaders(HttpClient client)
        {
            client.Headers.Set("Referer", "https://temp-mail.org/en/option/change");
            client.Headers.Set("Content-Type", "application/x-www-form-urlencoded");
        }

        private string BuildUploadString(string csrf, string login, string domain)
        {
            return $"csrf={csrf}&mail={login}&domain={ NormalizeDomain(domain) }";
        }

        private static string NormalizeDomain(string domain)
        {
            return (domain[0] == '@') ? domain : '@' + domain;
        }

        /// <summary>
        /// Deletes the temporary email and gets a new one.
        /// </summary>
        public bool Delete()
        {
            using (var client = CreateHttpClient())
            {
                var res = client.DownloadString("https://temp-mail.org/en/option/delete");

                if (client.StatusCode != HttpStatusCode.OK)
                    return false;

                Email = (new JavaScriptSerializer().Deserialize<object>(res) as Dictionary<string, object>)?["mail"].ToString();

                UpdateEmailCookie();

                return true;
            }
        }

        private void UpdateEmailCookie()
        {
            _cookies.SetCookies(new Uri("https://temp-mail.org/"), $"mail={Email}");
        }


        /// <summary>
        /// Sends a get request to the Url provided using this session cookies and returns the string result.
        /// </summary>
        /// <param name="url"></param>
        public string GET(string url)
        {
            using (var client = CreateHttpClient())
            {
                return client.DownloadString(url);
            }
        }

        private List<string> GetAvailableDomains()
        {
            using (var client = CreateHttpClient())
            {
                _document.LoadHtml(client.DownloadString("https://temp-mail.org/en/option/change"));
            }

            return ExtractDomains();
        }

        private List<string> ExtractDomains()
        {
            return _document.GetElementbyId("domain").Descendants("option")
                .Select(s => s.GetAttributeValue("value", null)).ToList();
        }

        private void CreateNewSession()
        {
            using (var client = CreateHttpClient())
            {
                _document.LoadHtml(LoadMainPage(client));
            }

            Email = ExtractEmail();
        }

        private static string LoadMainPage(HttpClient client)
        {
            return client.DownloadString("https://temp-mail.org/en/");
        }

        private string ExtractEmail()
        {
            return _document.GetElementbyId("mail").GetAttributeValue("value", null);
        }


        /// <summary>
        /// Returns an HttpClient that have this session cookies.
        /// </summary>
        private HttpClient CreateHttpClient()
        {
            return new HttpClient()
            {
                DefaultHeaders = new WebHeaderCollection()
                {
                    { "Accept", "*/*" },
                    { "Accept-Encoding", "gzip, deflate" },
                    { "Accept-Language", "en-US,en" },
                    { "User-Agent", "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36" },
                    { "Host", "temp-mail.org" },
                    { "Origin", "https://temp-mail.org"},
                    { "X-Requested-With", "XMLHttpRequest"},
                    { "Upgrade-Insecure-Requests", "1"}
                },
                CookieContainer = _cookies
            };
        }

    }
}
