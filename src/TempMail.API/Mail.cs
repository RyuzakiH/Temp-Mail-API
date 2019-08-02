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
        public IEnumerable<MimeEntity> Attachments => MimeMessage.Attachments;


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


        public void SaveAttachments() => Attachments.ToList().ForEach(attachment => SaveAttachment(attachment, Path.Combine(client.Email, Id)));

        public async Task SaveAttachmentsAsync()
        {
            foreach (var attachment in Attachments)
                await SaveAttachmentAsync(attachment, Path.Combine(client.Email, Id));
        }

        public static void SaveAttachment(MimeEntity attachment, string directory = "", string altFileName = null)
        {
            Directory.CreateDirectory(directory);

            if (attachment is MessagePart)
                SaveAttachment((MessagePart)attachment, directory, altFileName);
            else
                SaveAttachment((MimePart)attachment, directory, altFileName);
        }

        public static async Task SaveAttachmentAsync(MimeEntity attachment, string directory = "", string altFileName = null)
        {
            Directory.CreateDirectory(directory);

            if (attachment is MessagePart)
                await SaveAttachmentAsync((MessagePart)attachment, directory, altFileName);
            else
                await SaveAttachmentAsync((MimePart)attachment, directory, altFileName);
        }

        private static void SaveAttachment(MessagePart attachment, string directory = "", string altFileName = null)
        {
            var fileName = altFileName ?? Path.GetRandomFileName();
            using (var stream = File.Create(Path.Combine(directory, fileName)))
                attachment.Message.WriteTo(stream);
        }

        private static async Task SaveAttachmentAsync(MessagePart attachment, string directory = "", string altFileName = null)
        {
            var fileName = altFileName ?? Path.GetRandomFileName();
            using (var stream = File.Create(Path.Combine(directory, fileName)))
                await attachment.Message.WriteToAsync(stream);
        }

        private static void SaveAttachment(MimePart attachment, string directory = "", string altFileName = null)
        {
            var fileName = attachment.FileName ?? altFileName ?? Path.GetRandomFileName();
            using (var stream = File.Create(Path.Combine(directory, fileName)))
                attachment.Content.DecodeTo(stream);
        }

        private static async Task SaveAttachmentAsync(MimePart attachment, string directory = "", string altFileName = null)
        {
            var fileName = attachment.FileName ?? altFileName ?? Path.GetRandomFileName();
            using (var stream = File.Create(Path.Combine(directory, fileName)))
                await attachment.Content.DecodeToAsync(stream);
        }


        private static Uri CreateSourceLink(string id) => new Uri($"https://temp-mail.org/en/source/{id}/");

    }
}