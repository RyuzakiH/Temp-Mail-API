using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TempMail.API.Extensions;

namespace TempMail.API
{
    public class Inbox
    {
        private readonly Client client;

        private HtmlDocument _document;

        public List<Mail> Mails { get; }

        public Inbox(Client client)
        {
            this.client = client;
            _document = new HtmlDocument();
            Mails = new List<Mail>();
        }


        public IEnumerable<Mail> Refresh()
        {
            HttpResponseMessage response;
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, Client.CHECK_URL))
            {
                requestMessage.Headers.Referrer = new Uri(Client.BASE_URL);
                requestMessage.Headers.Add("X-Requested-With", "XMLHttpRequest");
                response = client.HttpClient.Send(requestMessage);
            }

            _document = response.Content.ReadAsHtmlDocument(Encoding.UTF8);

            Mails.AddRange(GetNewMails(ExtractSimpleMails()));

            return Mails;
        }

        public async Task<IEnumerable<Mail>> RefreshAsync()
        {
            HttpResponseMessage response;
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, Client.CHECK_URL))
            {
                requestMessage.Headers.Referrer = new Uri(Client.BASE_URL);
                requestMessage.Headers.Add("X-Requested-With", "XMLHttpRequest");
                response = await client.HttpClient.SendAsync(requestMessage);
            }

            _document = await response.Content.ReadAsHtmlDocumentAsync(Encoding.UTF8);

            Mails.AddRange(await Task.Run(() => GetNewMails(ExtractSimpleMails())));

            return Mails;
        }


        public void Clear()
        {
            Mails.Clear();
            _document = new HtmlDocument();
        }


        public IEnumerable<Mail> ExtractSimpleMails()
        {
            return _document.GetElementbyId("mails").Descendants("tbody")
                .FirstOrDefault()?.Descendants("tr").Select(ExtractSimpleMail).ToList();
        }

        private Mail ExtractSimpleMail(HtmlNode tr)
        {
            var a = tr.Descendants("td").FirstOrDefault()?.FirstChild;

            var link = a.GetAttributeValue("href", null);

            return new Mail()
            {
                Id = Mail.ExtractId(link),
                Subject = tr.GetElementsByClassName("title-subject").FirstOrDefault()?.InnerText,
                StrSender = a?.InnerText,
                Link = link
            };
        }

        private IEnumerable<Mail> GetNewMails(IEnumerable<Mail> mails)
        {
            return mails.Where(mail => Mails.Count(m => m.Id == mail.Id) == 0)
                .Select(mail => Mail.FromId(client, mail.Id)).ToList();
        }

    }
}
