using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using TempMail.API;

namespace TempMail.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();

            // To get the available domains
            var availableDomains = client.AvailableDomains;

            // Sends fake mails from your email to generated temporary email
            // Note: edit sender email credentials (email, password)
            // Note: you can put a break point and use any free email sender service online instead
            SendFakeMails(2, client.Email, 1000);

            // Wait for the mails to reach the tempmail
            Thread.Sleep(10000);

            // To get Mailbox
            var mails = client.Inbox.Refresh();

            // Prints Client Session data like current email, mails, ...etc
            // Note: edit to print what you need
            PrintClientData(client);

            // To change email to a specific login@domain
            client.Change("loginexample", availableDomains[0]);

            // To delete email and get a new one
            client.Delete();

            // To get the current email
            var email = client.Email;
        }

        private static void PrintClientData(Client client)
        {
            Console.WriteLine($"Email: {client.Email}");

            Console.WriteLine($"Inbox:");

            Console.WriteLine(string.Join('\n',
                client.Inbox.Mails.Select(mail => $"\tSender: {mail.From.FirstOrDefault().Name}\n\tSubject: {mail.Subject}\n")));
        }

        private static void SendFakeMails(int count, string temp_email, int delay = 1000)
        {
            //var from = new MailAddress("from@gmail.com", "Ryuzaki"); // Edit this
            var from = new MailAddress("zomablacktest@gmail.com", "Ryuzaki"); // Edit this
            var to = new MailAddress(temp_email, "TempMail Name");

            for (int i = 0; i < count; i++)
            {
                SendMail(from, to, "BlackZer0", $"Subject {i}", $"Fake Body {i}"); // Edit password here

                Thread.Sleep(delay);
            }
        }

        private static void SendMail(MailAddress from, MailAddress to, string password, string subject, string body)
        {
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from.Address, password)
            };

            MailMessage mail = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body
            };

            client.Send(mail);
        }

    }
}
