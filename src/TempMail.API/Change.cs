using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TempMail.API.Constants;
using TempMail.API.Exceptions;

namespace TempMail.API
{
    public class Change
    {
        private readonly Client client;

        public Change(Client client)
        {
            this.client = client;
        }

        /// <summary>
        /// Changes the temporary email to ex: login@domain
        /// </summary>
        /// <param name="login">New temporary email login</param>
        /// <param name="domain">New temporary email domain</param>
        public string ChangeEmail(string login, string domain)
        {
            if (!client.AvailableDomains.Contains(domain))
                throw new InvalidDomainException(domain);

            var csrf = client.GetCsrfCookie();
            var data = BuildPostData(csrf.Value, login, domain);

            var response = SendRequest(data);

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return $"{login}{NormalizeDomain(domain)}";
        }

        /// <summary>
        /// Changes the temporary email to ex: login@domain async
        /// </summary>
        /// <param name="login">New temporary email login</param>
        /// <param name="domain">New temporary email domain</param>
        public async Task<string> ChangeEmailAsync(string login, string domain)
        {
            if (!client.AvailableDomains.Contains(domain))
                throw new InvalidDomainException(domain);

            var csrf = client.GetCsrfCookie();
            var data = BuildPostData(csrf.Value, login, domain);

            var response = await SendRequestAsync(data);

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return $"{login}{NormalizeDomain(domain)}";
        }
        

        private Dictionary<string,string> BuildPostData(string csrf, string login, string domain)
        {
            return new Dictionary<string, string>
            {
                { "csrf", csrf },
                { "mail", login },
                { "domain", NormalizeDomain(domain) },
            };
        }

        private HttpResponseMessage SendRequest(Dictionary<string, string> data)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, Urls.CHANGE_URL))
            {
                requestMessage.Content = new FormUrlEncodedContent(data);
                requestMessage.Headers.Referrer = new Uri(Urls.CHANGE_URL);
                return client.HttpClient.Send(requestMessage);
            }
        }

        private async Task<HttpResponseMessage> SendRequestAsync(Dictionary<string, string> data)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, Urls.CHANGE_URL))
            {
                requestMessage.Content = new FormUrlEncodedContent(data);
                requestMessage.Headers.Referrer = new Uri(Urls.CHANGE_URL);
                requestMessage.Headers.Add("Origin", "https://temp-mail.org");
                return await client.HttpClient.SendAsync(requestMessage);
            }
        }

        private static string NormalizeDomain(string domain)
        {
            return (domain[0] == '@') ? domain : '@' + domain;
        }


    }
}
