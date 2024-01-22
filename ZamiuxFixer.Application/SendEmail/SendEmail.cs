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

                var smtpServer = new SmtpClient("sandbox.smtp.mailtrap.io", 2525);

                mail.From = new MailAddress("mohsen.1408@gmail.com", "Asp course mail");

                mail.To.Add(to);

                mail.Subject = subject;

                mail.Body = body;

                mail.IsBodyHtml = true;

                smtpServer.Port = 587;

                smtpServer.Credentials = new NetworkCredential("b768dea544f23d", "********dd64");

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