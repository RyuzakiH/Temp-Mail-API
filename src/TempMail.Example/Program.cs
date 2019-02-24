using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using TempMail.API;

namespace TempMail.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // Here, we test the synchronous way and the asynchronous one (uncomment any)

            //var client = new Client(new WebProxy("163.172.220.221", 8888));

            //var client = new Client()
            //{
            //    Proxy = new WebProxy("163.172.220.221", 8888)
            //};

            Test();

            //TestAsync().Wait();
        }

        private static void Test()
        {
            var client = new Client();
            
            // To get a new temporary email
            client.StartNewSession();
            
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

        private static async Task TestAsync()
        {
            var client = new Client();

            // To get a new temporary email
            await client.StartNewSessionAsync();

            // To get the available domains (not async)
            var availableDomains = await Task.Run(() => client.AvailableDomains);

            // Sends fake mails from your email to generated temporary email
            // Note: edit sender email credentials (email, password)
            // Note: you can put a break point and use any free email sender service online instead
            await Task.Run(() => SendFakeMails(2, client.Email, 1000));

            // Wait for the mails to reach the tempmail
            await Task.Run(() => Thread.Sleep(10000));

            // To get Mailbox
            var mails = await client.Inbox.RefreshAsync();

            // Prints Client Session data like current email, mails, ...etc
            // Note: edit to print what you need
            await Task.Run(() => PrintClientData(client));

            // To change email to a specific login@domain
            await client.ChangeAsync("loginexample", availableDomains[0]);

            // To delete email and get a new one
            await client.DeleteAsync();

            // To get the current email
            var email = client.Email;
        }


        private static void PrintClientData(Client client)
        {
            Console.WriteLine($"Email: {client.Email}");

            Console.WriteLine($"Inbox:");

            Console.WriteLine(string.Join('\n',
                client.Inbox.Mails.Select(mail =>
                $"\tSender: {mail.From.FirstOrDefault().Name}\n\tSubject: {mail.Subject}\n\tBody: {mail.TextBody}")));
        }

        private static void SendFakeMails(int count, string temp_email, int delay = 1000)
        {
            var from = new MailAddress("login@gmail.com", "Ryuzaki"); // Edit this

            var to = new MailAddress(temp_email, "TempMail Name");

            for (int i = 0; i < count; i++)
            {
                SendMail(from, to, "password", $"Subject {i}", $"Fake Body {i}"); // Edit password here

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
