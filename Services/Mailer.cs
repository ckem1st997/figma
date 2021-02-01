using figma.Interface;
using figma.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SmtpClient = System.Net.Mail.SmtpClient;

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
        //  public async Task SendEmailSync(string email, string subject, string body)
        //  {
        //try
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress(_smtp.SenderName, _smtp.SenderEmail));
        //    message.To.Add(new MailboxAddress(_smtp.SenderName, email));
        //    message.Subject = subject;
        //    message.Body = new TextPart("html")
        //    {
        //        Text = body
        //    };
        //    using (var client = new SmtpClient())
        //    {

        //        client.ServerCertificateValidationCallback += (s, c, h, e) => true;
        //        client.CheckCertificateRevocation = false;
        //        if (_env.IsDevelopment())
        //            await client.ConnectAsync(_smtp.Server, _smtp.Port, true);
        //        else
        //            await client.ConnectAsync(_smtp.Server);
        //        await client.AuthenticateAsync(_smtp.SenderEmail, _smtp.Password);
        //        await client.SendAsync(message);
        //        await client.DisconnectAsync(true);
        //    }
        //}
        //catch (Exception e)
        //{
        //    throw new InvalidOperationException(e.Message);
        //}
        //  }

        // email, tiêu đề, nội dung
        public async Task<string> SendEmailSync(string email, string subject, string body)
        {
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(_smtp.Username, _smtp.SenderName);
                    message.To.Add(email);
                    message.Subject = subject;
                    message.SubjectEncoding = Encoding.UTF8;
                    message.Body = body;
                    message.BodyEncoding = Encoding.UTF8;
                    message.IsBodyHtml = true;
                    using (SmtpClient smtpClient = new SmtpClient(_smtp.Server, _smtp.Port))
                    {
                        smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(_smtp.Username, _smtp.Password);
                        smtpClient.EnableSsl = true;
                        await smtpClient.SendMailAsync(message);
                    }
                    return "Send Successfull";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}


