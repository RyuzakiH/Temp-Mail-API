using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TempMail.API.Extensions;
using TempMail.API.Utilities;

namespace TempMail.API
{
    public class Mail
    {
        private TempMailClient client;

        internal bool CanLoad => Id != null;
        internal bool IsLoaded => MimeMessage != null;

        public MimeMessage MimeMessage { get; set; }

        public string Id { get; private set; }
        public Uri Link { get; private set; }

        public string SenderName => MimeMessage.From.FirstOrDefault()?.Name;
        public string Subject => MimeMessage.Subject;
        public InternetAddressList From => MimeMessage.From;
        public InternetAddressList To => MimeMessage.To;
        public string TextBody => MimeMessage.TextBody;
        public string HtmlBody => MimeMessage.HtmlBody;
        public DateTimeOffset? Date => MimeMessage.Date;
        public IEnumerable<MimePart> Attachments => MimeMessage.Attachments.Cast<MimePart>();


        public Mail(TempMailClient client, string id)
        {
            this.client = client;
            Id = id;
            Link = CreateSourceLink(id);
        }

        public Mail(TempMailClient client, Uri link)
        {
            this.client = client;
            Link = link;
            Id = Parser.GetMailId(link);
        }

        /// <summary>
        /// Loads Mail from the specified link or id.
        /// </summary>
        public Mail Load()
        {
            if (!CanLoad || IsLoaded)
                return this;

            var raw_mail = GetRawMail();
            return LoadFromRaw(raw_mail);
        }

        /// <summary>
        /// Loads Mail from the specified link or id.
        /// </summary>
        public async Task<Mail> LoadAsync()
        {
            if (!CanLoad || IsLoaded)
                return this;

            var raw_mail = await GetRawMailAsync();
            return await LoadFromRawAsync(raw_mail);
        }

        private string GetRawMail() => client.HttpClient.GetString(Link);

        private async Task<string> GetRawMailAsync() => await client.HttpClient.GetStringAsync(Link);

        private Mail LoadFromRaw(string raw_mail)
        {
            MimeMessage = GetMimeMessage(raw_mail);
            return this;
        }

        private async Task<Mail> LoadFromRawAsync(string raw_mail)
        {
            MimeMessage = await GetMimeMessageAsync(raw_mail);
            return this;
        }

        private static MimeMessage GetMimeMessage(string raw_mail) =>
            MimeMessage.Load(raw_mail.ToMemoryStream());

        private static async Task<MimeMessage> GetMimeMessageAsync(string raw_mail) =>
            await MimeMessage.LoadAsync(raw_mail.ToMemoryStream());

        public void SaveAttachment(MimePart attachment, string directory = "", string altFileName = null)
        {
            var fileName = attachment.FileName ?? altFileName ?? $"file{ new Random().Next(10000) }";

            using (var stream = File.Create(Path.Combine(directory, fileName)))
                attachment.Content.DecodeTo(stream);
        }

        private static Uri CreateSourceLink(string id) => new Uri($"https://temp-mail.org/en/source/{id}/");

    }
}