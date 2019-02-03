using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using MimeKit;

namespace TempMail.API
{
    public class Mail : MimeMessage
    {
        public new InternetAddressList From { get; private set; }
        public new InternetAddressList ResentFrom { get; private set; }
        public new InternetAddressList ReplyTo { get; private set; }
        public new InternetAddressList ResentReplyTo { get; private set; }
        public new InternetAddressList To { get; private set; }
        public new InternetAddressList ResentTo { get; private set; }
        public new InternetAddressList Cc { get; private set; }
        public new InternetAddressList ResentCc { get; private set; }
        public new InternetAddressList Bcc { get; private set; }
        public new InternetAddressList ResentBcc { get; private set; }
        public new string TextBody { get; private set; }
        public new string HtmlBody { get; private set; }
        public new HeaderList Headers { get; private set; }
        public new IEnumerable<MimeEntity> BodyParts { get; private set; }
        public new IEnumerable<MimePart> Attachments { get; private set; }
        public string Id { get; set; }
        public string Link { get; set; }
        public string StrSender { get; set; }


        public static Mail FromId(Client session, string id)
        {
            var sourceUrl = $"https://temp-mail.org/en/source/{id}";

            var raw_mail = session.GET(sourceUrl).Result;

            return GetMailFromRaw(raw_mail, id);
        }

        public static Mail FromLink(Client session, string link)
        {
            var id = ExtractId(link);

            var sourceUrl = string.Format("https://temp-mail.org/en/source/{0}", id);

            var raw_mail = session.GET(sourceUrl).Result;

            return GetMailFromRaw(raw_mail, id);
        }

        private static Mail GetMailFromRaw(string raw_mail, string id)
        {
            var mail = Parse(raw_mail);
            mail.Id = id;

            return mail;
        }


        public static Mail Parse(string raw_mail)
        {
            var message = Load(GenerateStreamFromString(raw_mail));

            return ConvertMessageToMail(message);
        }

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

        private static Mail ConvertMessageToMail(MimeMessage message)
        {
            return (Mail)Utilities.CastObjectToAnotherType(message, typeof(Mail));
        }


        public void SaveAttachment(MimePart attachment, string directory = "", string tempFileName = null)
        {
            var fileName = attachment.FileName ?? tempFileName;

            using (var stream = File.Create(Path.Combine(directory, fileName)))
            {
                attachment.Content.DecodeTo(stream);
            }
        }


        public static string ExtractId(string link)
        {
            return Regex.Match(link, @"https://temp-mail.org/en/.*?/(?<id>.*)").Groups["id"].Value;
        }

    }

}
