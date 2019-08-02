using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TempMail.API.Constants;
using TempMail.API.Events;
using TempMail.API.Extensions;
using TempMail.API.Utilities;

namespace TempMail.API
{
    public class Inbox
    {
        private readonly TempMailClient client;

        public List<Mail> Mails { get; }

        public event EventHandler<NewMailReceivedEventArgs> NewMailReceived;

        public Inbox(TempMailClient client)
        {
            this.client = client;
            Mails = new List<Mail>();
        }


        public IEnumerable<Mail> Refresh()
        {
            var response = client.SendRequest(HttpMethod.Get, Urls.CHECK_URL,
                referrer: new Uri(Urls.BASE_URL), xml: true, jsDateQuery: true);

            var document = response.Content.ReadAsString();

            UpdateMails(document);

            return Mails;
        }

        public async Task<IEnumerable<Mail>> RefreshAsync()
        {
            var response = await client.SendRequestAsync(HttpMethod.Get, Urls.CHECK_URL,
                referrer: new Uri(Urls.BASE_URL), xml: true, jsDateQuery: true);

            var document = await response.Content.ReadAsStringAsync();

            await UpdateMailsAsync(document);

            return Mails;
        }

        private void UpdateMails(string checkResponse)
        {
            var mailsIds = Parser.ExtractMailsIds(checkResponse);
            var newMailsIds = mailsIds.Where(IsNewMail);
            var newMails = GetMails(newMailsIds);
            Mails.AddRange(newMails);

            foreach (var mail in newMails)
                NewMailReceived?.Invoke(this, new NewMailReceivedEventArgs(mail));
        }

        private async Task UpdateMailsAsync(string checkResponse)
        {
            var mailsIds = Parser.ExtractMailsIds(checkResponse);
            var newMailsIds = mailsIds.Where(IsNewMail);
            var newMails = await GetMailsAsync(newMailsIds);
            Mails.AddRange(newMails);

            foreach (var mail in newMails)
                NewMailReceived?.Invoke(this, new NewMailReceivedEventArgs(mail));
        }

        private bool IsNewMail(string mailId) => !Mails.Any(mail => mail.Id.Equals(mailId));

        private IEnumerable<Mail> GetMails(IEnumerable<string> mailsIds) =>
            mailsIds.Select(id => new Mail(client, id).Load()).ToList();

        private async Task<IEnumerable<Mail>> GetMailsAsync(IEnumerable<string> mailsIds)
        {
            var resultMails = new List<Mail>();
            foreach (var id in mailsIds)
                resultMails.Add(await new Mail(client, id).LoadAsync());
            return resultMails;
        }


        public void Clear()
        {
            Mails.Clear();
        }
    }
}
