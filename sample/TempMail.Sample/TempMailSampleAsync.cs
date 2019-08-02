using System.Threading;
using System.Threading.Tasks;
using TempMail.API;

namespace TempMail.Sample
{
    public class TempMailSampleAsync
    {
        public static async Task Sample()
        {
            var client = await TempMailClient.CreateAsync();

            // To get the available domains (not async)
            var availableDomains = client.AvailableDomains;

            // Sends fake mails from your email to generated temporary email
            // Note: edit sender email credentials (email, password)
            // Note: you can put a break point and use any free email sender service online instead
            await Common.SendFakeMailsAsync(2, client.Email, 1000);

            // Wait for the mails to reach the tempmail
            await Task.Run(() => Thread.Sleep(10000));

            // To get Mailbox
            var mails = await client.Inbox.RefreshAsync();

            // Prints Client Session data like current email, mails, ...etc
            // Note: edit to print what you need
            Common.PrintClientData(client);

            // To change email to a specific login@domain
            await client.ChangeAsync("loginexample", availableDomains[0]);

            // To delete email and get a new one
            await client.DeleteAsync();

            // To get the current email
            var email = client.Email;
        }
    }
}
