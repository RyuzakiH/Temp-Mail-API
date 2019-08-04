using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using TempMail.API;

namespace TempMail.Sample
{
    public class Common
    {
        // Edit these
        private const string FromEmail = "login@gmail.com";
        private const string FromDisplayName = "name";
        private const string FromPassword = "password";


        public static void SendFakeMails(int count, string temp_email, int delay = 1000, bool attachments = false)
        {
            var from = new MailAddress(FromEmail, FromDisplayName);
            var to = new MailAddress(temp_email, "TempMail Name");

            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                if (attachments)
                {
                    var attachmentsPaths = Directory.EnumerateFiles(@"..\..\..\Attachments\");
                    var selectedAttachments = attachmentsPaths.Where((c, j) => (random.Next(0, 10) > 5) ? (j % 2 != 0) : (j % 2 == 0));
                    if (!selectedAttachments.Any())
                        selectedAttachments = attachmentsPaths.Where((c, j) => random.Next(0, attachmentsPaths.Count() - 1).Equals(j));

                    MailUtility.SendMail(from, to, FromPassword, $"Subject {i}", $"Fake Body {i}", selectedAttachments.ToArray());
                }
                else
                {
                    MailUtility.SendMail(from, to, FromPassword, $"Subject {i}", $"Fake Body {i}");
                }

                Thread.Sleep(delay);
            }
        }

        public static async Task SendFakeMailsAsync(int count, string temp_email, int delay = 1000, bool attachments = false)
        {
            var from = new MailAddress(FromEmail, FromDisplayName);
            var to = new MailAddress(temp_email, "TempMail Name");

            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                if (attachments)
                {
                    var attachmentsPaths = Directory.EnumerateFiles(@"..\..\..\Attachments\");
                    var selectedAttachments = attachmentsPaths.Where((c, j) => (random.Next(0, 10) > 5) ? (j % 2 != 0) : (j % 2 == 0));
                    if (!selectedAttachments.Any())
                        selectedAttachments = attachmentsPaths.Where((c, j) => random.Next(0, attachmentsPaths.Count() - 1).Equals(j));

                    await MailUtility.SendMailAsync(from, to, FromPassword, $"Subject {i}", $"Fake Body {i}", selectedAttachments.ToArray());
                }
                else
                {
                    await MailUtility.SendMailAsync(from, to, FromPassword, $"Subject {i}", $"Fake Body {i}");
                }

                await Task.Delay(delay);
            }
        }


        public static void PrintClientData(TempMailClient client)
        {
            Console.WriteLine("========================================");
            Console.WriteLine($"Email: {client.Email}");
            Console.WriteLine("Inbox:");
            client.Inbox.Mails.ForEach(PrintMail);
            Console.WriteLine("========================================");
        }

        public static void PrintMail(Mail mail) =>
            Console.WriteLine($"\tSender: {mail.SenderName}\n\tSubject: {mail.Subject}\n\tBody: {mail.TextBody}");

    }
}