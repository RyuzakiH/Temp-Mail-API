namespace TempMail.API.Events
{
    public class NewMailReceivedEventArgs : System.EventArgs
    {
        public Mail Mail { get; set; }

        public NewMailReceivedEventArgs(Mail mail)
        {
            Mail = mail;
        }
    }
}
