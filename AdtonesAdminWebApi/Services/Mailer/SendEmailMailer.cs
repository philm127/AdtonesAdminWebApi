using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MailKit.Security;

namespace AdtonesAdminWebApi.Services.Mailer
{
    /// <summary>
    /// TODO: Need to sort whole of the mailing out
    /// </summary>
    public class SendEmailMailer : ISendEmailMailer
    {
        private readonly IConfiguration _configuration;

        public SendEmailMailer(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void SendEmail(SendEmailModel mail)
        {

            var message = new MimeMessage();
            //foreach (var two in mail.To)
            //{
            //    message.To.Add(new MailboxAddress(two));
            //}
            message.To.Add(MailboxAddress.Parse(mail.SingleTo));
            message.From.Add(MailboxAddress.Parse(mail.From));

            if (mail.Bcc != null)
            {
                foreach (var blind in mail.Bcc)
                {
                    message.Bcc.Add(MailboxAddress.Parse(blind));
                }
            }
            if (mail.CC != null)
            {
                foreach (var share in mail.CC)
                {
                    message.Cc.Add(MailboxAddress.Parse(share));
                }
            }
            message.Subject = mail.Subject;
            message.Body = new TextPart("html") { Text = mail.Body };

            var pwd = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SMTPPassword").Value;
            var usr = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SMTPEmail").Value;
            var srv = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SmtpServerAddress").Value;
            var port = int.Parse(_configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SmtpServerPort").Value);

            using SmtpClient client = new SmtpClient();
            {
                client.Connect(srv, port, SecureSocketOptions.StartTls);
                client.Authenticate(usr, pwd);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}