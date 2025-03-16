using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace ArticalProject.Code
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("EMAIL", "PASSWORD"),
                EnableSsl = true,
            };

            return smtpClient.SendMailAsync("EMAIL@gmail.com", email, subject, htmlMessage);
        }
    }
}
