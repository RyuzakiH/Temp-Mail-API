namespace TempMail.API.Events
{
    public class EmailChangedEventArgs : System.EventArgs
    {
        public string Email { get; set; }

        public EmailChangedEventArgs(string email)
        {
            Email = email;
        }
    }
}
