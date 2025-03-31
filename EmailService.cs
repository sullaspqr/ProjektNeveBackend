using System.Net.Mail;
using System.Threading.Tasks;

namespace ProjektNeveBackend.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            using var mail = new MailMessage();
            using var client = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("backend.registry@kkszki.hu");
            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = body;

            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential(
                "backend.registry@kkszki.hu", 
                "BackEnd-2022");
            client.EnableSsl = true;

            await client.SendMailAsync(mail);
        }
    }
}
