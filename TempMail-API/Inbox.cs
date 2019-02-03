using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace TempMail
{
    public class Inbox
    {
        private readonly Session _session;
        private readonly HtmlDocument _document;

        public List<Mail> Mails;

        public Inbox(Session session)
        {
            _session = session;
            _document = new HtmlDocument();
            Mails = new List<Mail>();
        }


        public IEnumerable<Mail> Refresh()
        {
            _document.LoadHtml(_session.GET("https://temp-mail.org/en/option/check"));

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
            var a = tr.Descendants("td").First().FirstChild;

            var link = a.GetAttributeValue("href", null);

            return new Mail()
            {
                Id = Mail.ExtractId(link),
                Subject = Utilities.GetElementsByClassName(_document, "title-subject").ToList()[0].InnerText,
                StrSender = a.InnerText,
                Link = link
            };
        }

        private IEnumerable<Mail> GetNewMails(IEnumerable<Mail> mails)
        {
            return mails.Where(mail => Mails.Count(m => m.Id == mail.Id) == 0)
                .Select(mail => Mail.FromId(_session, mail.Id)).ToList();
        }

    }
}
