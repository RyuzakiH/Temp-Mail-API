using System;

namespace TempMail.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var session = new Session();

            // To get the available domains
            var availableDomains = session.AvailableDomains;

            // To get Mailbox
            var mails = session.Inbox.Refresh();

            // To change email to a specific login@domain
            session.Change("loginexample", availableDomains[0]);

            // To delete email and get a new one
            session.Delete();

            // To get the current email
            var email = session.Email;
        }
    }
}
