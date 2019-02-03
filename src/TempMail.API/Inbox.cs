using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using TempMail.API.Extensions;

namespace TempMail.API
{
    public class Inbox
    {
        private readonly Client sessionClient;

        private HtmlDocument _document;

        public List<Mail> Mails;

        public Inbox(Client session)
        {
            sessionClient = session;
            _document = new HtmlDocument();
            Mails = new List<Mail>();
        }


        public IEnumerable<Mail> Refresh()
        {
            _document = sessionClient.GetHtmlDocument(Client.CHECK_URL);

            Mails.AddRange(GetNewMails(ExtractSimpleMails()));

            return Mails;
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
                .Select(mail => Mail.FromId(sessionClient, mail.Id)).ToList();
        }

    }
}
