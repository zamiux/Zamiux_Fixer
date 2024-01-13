using System.Net;
using System.Net.Mail;

namespace ZamiuxFixer.Application.SendEmail
{
    public interface IMailSender
    {
        bool Send(string to, string subject, string body);
    }

    public class SendEmail : IMailSender
    {
      

        public bool Send(string to, string subject, string body)
        {
            try
            {
                var mail = new MailMessage();

                var smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("barnamenevisanaspcourse@gmail.com", "Asp course mail");

                mail.To.Add(to);

                mail.Subject = subject;

                mail.Body = body;

                mail.IsBodyHtml = true;

                smtpServer.Port = 587;

                smtpServer.Credentials = new NetworkCredential("barnamenevisanaspcourse@gmail.com", "kokjbtvrwkjlpnao");

                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}