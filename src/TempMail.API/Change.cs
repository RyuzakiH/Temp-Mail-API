using System.Net;
using System.Threading.Tasks;
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
            var data = BuildUploadString(csrf.Value, login, domain);

            var response = client.Post(Client.CHANGE_URL, data, MakeUploadHeaders());

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
            var data = BuildUploadString(csrf.Value, login, domain);

            var response = await client.PostAsync(Client.CHANGE_URL, data, MakeUploadHeaders());

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
