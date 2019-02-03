using System;
using TempMail.API;

namespace TempMail.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var session = new Client();

            // To get the available domains
            var availableDomains = session.AvailableDomains;

            // To get Mailbox
            var mails = session.Inbox.Refresh();

            var ms = session.Inbox.ExtractSimpleMails();

            // To change email to a specific login@domain
            session.Change("loginexample", availableDomains[0]);

            // To delete email and get a new one
            session.Delete();

            // To get the current email
            var email = session.Email;
        }
    }
}
