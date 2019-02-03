using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TempMail.API.Models;
using TempMail.API.Utilities;

namespace TempMail.API
{
    public class Client
    {
        public const string BASE_URL = "https://temp-mail.org";
        public const string MAIN_PAGE_URL = "https://temp-mail.org/en/";
        public const string CHANGE_URL = "https://temp-mail.org/en/option/change";
        public const string CHECK_URL = "https://temp-mail.org/en/option/check";
        public const string DELETE_URL = "https://temp-mail.org/en/option/delete";

        
        private CookieContainer cookies;

        private List<string> availableDomains;
        public List<string> AvailableDomains => availableDomains ?? (availableDomains = GetAvailableDomains());


        private Change ChangePage;

        public Inbox Inbox;

        public string Email { get; set; }



        public Client()
        {
            //_document = new HtmlDocument();
            cookies = new CookieContainer();

            Inbox = new Inbox(this);

            ChangePage = new Change(this);

            CreateNewSession();
        }


        private void CreateNewSession()
        {
            var document = GetHtmlDocument(MAIN_PAGE_URL);

            Email = ExtractEmail(document);
        }

        private string ExtractEmail(HtmlDocument document)
        {
            return document.GetElementbyId("mail").GetAttributeValue("value", null);
        }



        /// <summary>
        /// Changes the temporary email to ex: login@domain .
        /// </summary>
        /// <param name="login">New temporary email login</param>
        /// <param name="domain">New temporary email domain</param>
        public string Change(string login, string domain)
        {
            return (Email = ChangePage.ChangeEmail(login, domain));
        }


        /// <summary>
        /// Deletes the temporary email and gets a new one.
        /// </summary>
        public bool Delete()
        {
            var response = GET(DELETE_URL);

            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            Email = (JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Result))?["mail"].ToString();

            UpdateEmailCookie();

            return true;
        }



        private List<string> GetAvailableDomains()
        {
            var document = GetHtmlDocument(CHANGE_URL);

            return document.GetElementbyId("domain").Descendants("option")
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



        /// <summary>
        /// Sends a get request to the Url provided using this session cookies.
        /// </summary>
        /// <returns> Response object containing the status code and the result of the response </returns>
        public Response GET(string url)
        {
            using (var client = CreateHttpClient())
            {
                return new Response { StatusCode = client.StatusCode, Result = client.DownloadString(url) };
            }
        }

        /// <summary>
        /// Sends a get request to the Url provided using this session cookies and returns the HtmlDocument of the result.
        /// </summary>
        public HtmlDocument GetHtmlDocument(string url)
        {
            using (var client = CreateHttpClient())
            {
                var response = client.DownloadString(url);

                var document = new HtmlDocument();
                document.LoadHtml(response);

                return document;
            }
        }

        /// <summary>
        /// Sends a post request to the Url provided using this session cookies and returns the string result.
        /// </summary>
        public string POST(string url, string data)
        {
            using (var client = CreateHttpClient())
            {
                return client.UploadString(url, data);
            }
        }

        /// <summary>
        /// Sends a post request to the Url provided using this session cookies and provided headers.
        /// </summary>
        /// <returns> Response object containing the status code and the result of the response </returns>
        public Response POST(string url, string data, WebHeaderCollection headers)
        {
            using (var client = CreateHttpClient())
            {
                foreach (string header in headers)
                    client.Headers.Set(header, headers[header]);

                return new Response { StatusCode = client.StatusCode, Result = client.UploadString(url, data) };
            }
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
                Encoding = Encoding.UTF8,
                CookieContainer = cookies
            };
        }

    }
}
