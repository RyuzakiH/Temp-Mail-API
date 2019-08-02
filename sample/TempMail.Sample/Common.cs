using System;
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
        private const string FromDisplayName = "Ryuzaki";
        private const string FromPassword = "password";


        public static void SendFakeMails(int count, string temp_email, int delay = 1000)
        {
            var from = new MailAddress(FromEmail, FromDisplayName);
            var to = new MailAddress(temp_email, "TempMail Name");

            for (int i = 0; i < count; i++)
            {
                MailUtility.SendMail(from, to, FromPassword, $"Subject {i}", $"Fake Body {i}");

                Thread.Sleep(delay);
            }
        }

        public static async Task SendFakeMailsAsync(int count, string temp_email, int delay = 1000)
        {
            var from = new MailAddress(FromEmail, FromDisplayName);
            var to = new MailAddress(temp_email, "TempMail Name");

            for (int i = 0; i < count; i++)
            {
                await MailUtility.SendMailAsync(from, to, FromPassword, $"Subject {i}", $"Fake Body {i}");

                Thread.Sleep(delay);
            }
        }


        public static void PrintClientData(TempMailClient client)
        {
            Console.WriteLine($"Email: {client.Email}");
            Console.WriteLine($"Inbox:");
            client.Inbox.Mails.ForEach(PrintMail);
        }

        public static void PrintMail(Mail mail) =>
            Console.WriteLine($"\tSender: {mail.SenderName}\n\tSubject: {mail.Subject}\n\tBody: {mail.TextBody}");

    }
}