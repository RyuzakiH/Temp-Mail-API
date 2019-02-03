using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TempMail.API
{
    public class Change
    {
        private readonly Client sessionClient;

        public Change(Client session)
        {
            sessionClient = session;
        }

        /// <summary>
        /// Changes the temporary email to ex: login@domain .
        /// </summary>
        /// <param name="login">New temporary email login</param>
        /// <param name="domain">New temporary email domain</param>
        public string ChangeEmail(string login, string domain)
        {
            if (!sessionClient.AvailableDomains.Contains(domain))
                throw new Exception("The domain you entered isn't an available domain");

            var csrf = sessionClient.GetCsrfCookie();
            var data = BuildUploadString(csrf.Value, login, domain);

            var response = sessionClient.POST(Client.CHANGE_URL, data, MakeUploadHeaders());

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return $"{login}{NormalizeDomain(domain)}";
        }


        private WebHeaderCollection MakeUploadHeaders()
        {
            return new WebHeaderCollection
            {
                { "Referer", Client.CHANGE_URL },
                { "Content-Type", "application/x-www-form-urlencoded" }
            };
        }

        private string BuildUploadString(string csrf, string login, string domain)
        {
            return $"csrf={csrf}&mail={login}&domain={ NormalizeDomain(domain) }";
        }

        private static string NormalizeDomain(string domain)
        {
            return (domain[0] == '@') ? domain : '@' + domain;
        }



    }
}
