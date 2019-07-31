using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TempMail.Sample
{
    public static class MailUtility
    {
        public static void SendMail(MailAddress from, MailAddress to, string password, string subject, string body)
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

        public static async Task SendMailAsync(MailAddress from, MailAddress to, string password, string subject, string body)
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

            client.SendAsync(mail, null);
        }
    }
}
