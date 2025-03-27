using System.Net;
using System.Net.Mail;

namespace Company.PL.Helpers
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            // Mail Server : Gmail
            // SMTP Server : smtp.gmail.com
            // Port : 587

            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("ahmedsheta834@gmail.com", "yuydhxxwvrhcvewm");
                client.Send("ahmedsheta834@gmail.com", email.To, email.Subject, email.Body);

                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
