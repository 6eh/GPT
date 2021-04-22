using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
//using MimeKit;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace GPT.Services
{
    public class EmailService : IEmailSender
    {
        /*public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "servicedesk@sibeer.su"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.beget.com", 2525, true);
                await client.AuthenticateAsync("servicedesk@sibeer.su", "Farma646");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }*/

        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string smtp = Configuration["EmailService:SmtpServer"];
            int port = Convert.ToInt16(Configuration["EmailService:SmtpPort"]);
            string sender = Configuration["EmailService:MailSender"];
            string pass = Configuration["EmailService:Pass"];

            var client = new SmtpClient(smtp, port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sender, pass)
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(sender)
            };
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true; // Включить ссылки в теле письма
            mailMessage.Body = htmlMessage;
            return client.SendMailAsync(mailMessage);
        }
    }
}
