using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Hosting;

namespace AdtonesAdminWebApi.Services.Mailer
{
    /// <summary>
    /// TODO: Need to sort whole of the mailing out
    /// </summary>
    public class SendEmailMailer : ISendEmailMailer
    {
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _configuration;

        public SendEmailMailer(IConfiguration configuration, IWebHostEnvironment _env)
        {
            env = _env;
            _configuration = configuration;
        }


        public async Task SendEmail(SendEmailModel mail)
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
            // message.Body = new TextPart("html") { Text = mail.Body };
            var builder = new BodyBuilder { HtmlBody = mail.Body };
            if (mail.attachment != null)
            {
                var otherpath = env.ContentRootPath;
                var filePath = Path.Combine(otherpath, mail.attachment);
                var filename = Path.GetFileName(filePath);
                var ms = new MemoryStream();

                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(ms);
                    builder.Attachments.Add(filename, ms.ToArray());
                }
                ms.Dispose();
            }

            message.Body = builder.ToMessageBody();

            var pwd = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SMTPPassword").Value;
            var usr = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SMTPEmail").Value;
            var srv = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SmtpServerAddress").Value;
            var port = int.Parse(_configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SmtpServerPort").Value);

            using SmtpClient client = new SmtpClient();
            {
                client.Connect(srv, port, SecureSocketOptions.StartTls);
                client.Authenticate(usr, pwd);
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    var _logging = new ErrorLogging()
                    {
                        ErrorMessage = ex.Message.ToString(),
                        StackTrace = ex.StackTrace.ToString(),
                        PageName = "Services-Mailer-SendEmailMailer",
                        ProcedureName = "SendEmail"
                    };
                    _logging.LogError();
                    // return new FormFile();
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
        }
    }
}