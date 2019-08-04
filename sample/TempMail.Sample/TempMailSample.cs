using CloudflareSolverRe.CaptchaProviders;
using System;
using System.Net;
using System.Threading;
using TempMail.API;

namespace TempMail.Sample
{
    public class TempMailSample
    {
        public static void Sample()
        {
            var client = TempMailClient.Create();

            Sample(client);
        }

        public static void SampleWithCaptchaProvider()
        {
            // Used if temp-mail is using cloudflare protection and it's not solved with JS (if needed).
            var client = TempMailClient.Create(
                captchaProvider: new AntiCaptchaProvider("YOUR_API_KEY"));

            Sample(client);
        }

        public static void SampleWithProxy()
        {
            var client = TempMailClient.Create(
                proxy: new WebProxy("163.172.220.221", 8888));

            Sample(client);
        }

        public static void SampleWithCaptchaProviderAndProxy()
        {
            var client = TempMailClient.Create(
                captchaProvider: new AntiCaptchaProvider("YOUR_API_KEY"),
                proxy: new WebProxy("163.172.220.221", 8888));

            Sample(client);
        }

        private static void Sample(TempMailClient client)
        {
            // To get the available domains
            var availableDomains = client.AvailableDomains;

            // To get the current email
            var email = client.Email;

            client.EmailChanged += (o, e) => Console.WriteLine($"Email changed: {e.Email}");
            client.Inbox.NewMailReceived += (o, e) => Common.PrintMail(e.Mail);

            // Sends fake mails from your email to generated temporary email
            // Note: edit sender email credentials (email, password)
            // Note: you can use any free email sender service online instead
            Common.SendFakeMails(2, client.Email, 1000);

            // Checks for incoming mails every period of time
            client.StartAutoCheck();

            // Wait for the mails to reach the temp-mail            
            WaitForMailsToReach(client, 2, 1000);

            Common.SendFakeMails(1, client.Email, 1000);

            WaitForMailsToReach(client, 3, 1000);

            client.StopAutoCheck();

            // give it 10s delay to make sure it reached temp-mail
            Common.SendFakeMails(1, client.Email, 10000, true);

            // Prints client session data like current email, mails, ...etc
            // Note: edit to print what you need
            Console.WriteLine("Only 3 mails, as we stopped auto check so we need to explicitly use refresh");
            Common.PrintClientData(client);

            // To get all mails in mailbox
            client.Inbox.Refresh();

            // To access mails
            var mails = client.Inbox.Mails;

            Console.WriteLine("4 mails (all mails)");
            Common.PrintClientData(client);

            // To save attachments
            mails.ForEach(mail => mail.SaveAttachments());

            // To change email to a specific login@domain
            client.ChangeEmail("loginexample", availableDomains[0]);

            // To delete email and get a new one
            client.Delete();
        }

        private static void WaitForMailsToReach(TempMailClient client, int mailsCount, int delay = 1000)
        {
            while (client.Inbox.Mails.Count < mailsCount)
                Thread.Sleep(delay);
        }
    }
}