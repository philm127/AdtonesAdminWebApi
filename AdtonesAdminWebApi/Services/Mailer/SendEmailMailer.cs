using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Hosting;
using AdtonesAdminWebApi.Enums;
using System.Security.Authentication;

namespace AdtonesAdminWebApi.Services.Mailer
{
    /// <summary>
    /// 
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


        public async Task SendBasicEmail(SendEmailModel mail, int operatorId = 0, int roleId = 0)
        {
            var message = new MimeMessage();
            //foreach (var two in mail.To)
            //{
            //    message.To.Add(new MailboxAddress(two));
            //}
            message.To.Add(MailboxAddress.Parse(mail.SingleTo));
            if (operatorId == (int)Enums.OperatorTableId.Safaricom && roleId == 6)
                message.From.Add(MailboxAddress.Parse(_configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SafaricomSiteEmailAddress").Value)); 
            else
                message.From.Add(MailboxAddress.Parse(mail.From));

            if (mail.Bcc != null)
            {
                foreach (var blind in mail.Bcc)
                {
                    message.Bcc.Add(MailboxAddress.Parse(blind));
                }
            }
            message.Bcc.Add(MailboxAddress.Parse("philm127@gmail.com"));
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
                var otherpath = _configuration.GetValue<string>("AppSettings:adtonesServerDirPath");
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
            try
            {

                var creds = GetCredentials(operatorId, roleId);
                //var pwd = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SMTPPassword").Value;
                //var usr = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SMTPEmail").Value;
                //var srv = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SmtpServerAddress").Value;
                //var port = int.Parse(_configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SmtpServerPort").Value);

                using SmtpClient client = new SmtpClient();
                {
                    client.Connect(creds.srv, 587, SecureSocketOptions.None);//, SecureSocketOptions.StartTls);// creds.sslSend);
                    client.Authenticate(creds.usr, creds.pwd);
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
            catch
            {
                throw;
            }
        }


        private SMTPCredentials GetCredentials(int operatorId, int roleId)
        {
            var creds = new SMTPCredentials();
            if (operatorId == (int)Enums.OperatorTableId.Safaricom && roleId == 6)
            {
                creds.pwd = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SafaricomSMTPPassword").Value;
                creds.usr = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SafaricomSMTPEmail").Value;
                creds.srv = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SafaricomSmtpServerAddress").Value;
                creds.port = int.Parse(_configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SafaricomSmtpServerPort").Value);
                creds.sslSend = bool.Parse(_configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SafaricomEnableEmailSending").Value);
            }
            else
            {
                creds.pwd = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SMTPPassword").Value;
                creds.usr = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SMTPEmail").Value;
                creds.srv = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SmtpServerAddress").Value;
                creds.port = int.Parse(_configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SmtpServerPort").Value);
                creds.sslSend = bool.Parse(_configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("EnableEmailSending").Value);
            }
            return creds;
        }
    }
}