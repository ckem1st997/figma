using figma.Interface;
using figma.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace figma.Services
{
    public class Mailer : IMailer
    {
        private readonly IWebHostEnvironment _env;
        public Smtp _smtp { get; }
        public Mailer(IOptions<Smtp> smtp, IWebHostEnvironment env)
        {
            _smtp = smtp.Value;
            _env = env;
        }
        public async Task SendEmailSync(string email, string title, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtp.SenderName, _smtp.SenderEmail));
                message.To.Add(new MailboxAddress(_smtp.SenderName, email));
                message.Subject = title;
                message.Body = new TextPart("html")
                {
                    Text = body
                };
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    if (_env.IsDevelopment())
                    {
                        await client.ConnectAsync(_smtp.Server, _smtp.Port, true);
                    }
                    else
                    {
                        await client.ConnectAsync(_smtp.Server);
                    }

                    await client.AuthenticateAsync(_smtp.SenderEmail, _smtp.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }
    }
}
