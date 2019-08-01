using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TempMail.API.Constants;
using TempMail.API.Extensions;

namespace TempMail.API
{
    public class Inbox
    {
        private readonly TempMailClient client;

        private HtmlDocument _document;

        public List<Mail> Mails { get; }

        public Inbox(TempMailClient client)
        {
            this.client = client;
            _document = new HtmlDocument();
            Mails = new List<Mail>();
        }


        public IEnumerable<Mail> Refresh()
        {
            HttpResponseMessage response;
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, Urls.CHECK_URL))
            {
                requestMessage.Headers.Referrer = new Uri(Urls.BASE_URL);
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
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, Urls.CHECK_URL))
            {
                requestMessage.Headers.Referrer = new Uri(Urls.BASE_URL);
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

        public IEnumerable<Mail> ExtractSimpleMails() =>
            _document.DocumentNode.Descendants("li").Select(ExtractSimpleMail).ToList();

        private Mail ExtractSimpleMail(HtmlNode li)
        {
            var sender = li.Descendants("span").Where(d =>   d.Attributes["class"].Value.Equals("inboxSenderName")).FirstOrDefault().InnerText;

            var span = li.Descendants("span").Where(d =>   d.Attributes["class"].Value.Equals("inboxSubject subject-title")).FirstOrDefault();

            var a =  span.Descendants("a").FirstOrDefault();

            var link = a.GetAttributeValue("href", null);
            var subject = a.GetAttributeValue("title", null);

            return new Mail()
            {
                Id = Mail.ExtractId(link),
                Subject = subject,
                StrSender = sender,
                Link = link
            };
        }

        private IEnumerable<Mail> GetNewMails(IEnumerable<Mail> mails) => 
            mails.Where(mail => Mails.Count(m => m.Id == mail.Id) == 0)
                .Select(mail => Mail.FromId(client, mail.Id)).ToList();

    }
}
