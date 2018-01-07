using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsIKnow.Mail
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public MailOptions Options { get; set; }
        public EmailSender(IOptions<MailOptions> options)
        {
            Options = options.Value;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return SendEmailAsync(email, subject, message, Options.NoreplayAddress);
        }

        public Task SendEmailAsync(string to, string subject, string message, string from)
        {
            MimeMessage mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(from, from));
            mailMessage.To.Add(new MailboxAddress(to, to));
            mailMessage.Subject = subject;

            mailMessage.Body = new TextPart("html")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(Options.SmtpServer, 25, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(Options.User, Options.Password);

                client.Send(mailMessage);
                client.Disconnect(true);
            }

            return Task.CompletedTask;
        }
    }
}
