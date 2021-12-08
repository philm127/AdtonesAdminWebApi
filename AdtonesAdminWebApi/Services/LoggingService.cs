using AdtonesAdminWebApi.Services.Mailer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;

namespace AdtonesAdminWebApi.Services
{
    public interface ILoggingService
    {
        public string PageName { get; set; }
        public string ProcedureName { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string LogLevel { get; set; }
        Task LogError();
        Task LogInfo();
        Task LoggingError(Exception ex, string pageName, string methodCall);
    }


    public class LoggingService : ILoggingService
    {
        public string PageName
        {
            get => pageName;
            set => pageName = value;
        }
        public string ProcedureName
        {
            get => procedureName;
            set => procedureName = value;
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set => errorMessage = value;
        }

        public string StackTrace
        {
            get => stackTrace;
            set => stackTrace = value;
        }

        public string LogLevel
        {
            get => logLevel;
            set => logLevel = value;
        }


        private string pageName;
        private string errorMessage;
        private string procedureName;
        private string stackTrace;
        private string logLevel;
        private readonly IConfiguration _configuration;


        public LoggingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string RootPath()
        {
            /// TODO: Check the working of the file paths
            var WebRootPath = Directory.GetCurrentDirectory();
            string webroot = WebRootPath + "\\Logging\\";
            return webroot;
        }

        public  async Task LoggingError(Exception ex, string pageName, string methodCall)
        {
            ErrorMessage = ex.Message.ToString();
            StackTrace = ex.StackTrace.ToString();
            PageName = pageName;
            ProcedureName = methodCall;
            await LogError();
        }

        public async Task LogError()
        {
            string webroot = RootPath();
            string ResolvePath = "";
            ResolvePath = string.Concat(webroot + "ErrorLogs.txt");
            var filepath = ResolvePath;
            var messageToWrite = LogMessageBuilder();

            await WriteTextAsync(filepath, messageToWrite);
            await CreateErrorEmail();
        }


        public async Task LogInfo()
        {
            string webroot = RootPath();
            // string MainFolderPath = "Logging\\";
            string ResolvePath = "";
            ResolvePath = string.Concat(webroot + "InfoLogs.txt");
            var filepath = ResolvePath;

            var messageToWrite = LogMessageBuilder();

            await WriteTextAsync(filepath, messageToWrite);
        }


        static async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.UTF8.GetBytes(text);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }


        public async Task CreateErrorEmail()
        {
            var test = _configuration.GetValue<string>("Environment:Location");
            var mail = new SendEmailModel();
            string[] cc = new string[1];
            if (test != "development")
            {
                mail.SingleTo = "support@adtones.com";
                cc[0] = "philm127@outlook.com";
                mail.CC = cc;
                mail.From = "support@adtones.com";

                string emailContent = string.Empty;
                emailContent = EmailMessageBuilder();
                mail.Body = string.Format(emailContent);

                if (test == "production")
                    mail.Subject = "Admin Adtone API error log at  " + DateTime.Now;
                else
                    mail.Subject = "UAT Testing Adtone API error log at  " + DateTime.Now;

                await SendErrorEmail(mail);
            }
        }



        public string LogMessageBuilder()
        {
            var w = new StringBuilder();
            w.AppendLine("-------------------START-------------" + DateTime.Now);
            w.AppendLine($"Page: {PageName}, Procedure: {ProcedureName},ErrorMsg: {ErrorMessage}");
            w.AppendLine($"LogLevel: {LogLevel}");
            w.AppendLine($" StackTrace  : {StackTrace}");
            w.AppendLine("------------END-------------------");
            var message = string.Format(w.ToString());
            return message;
        }


        public string EmailMessageBuilder()
        {
            var w = new StringBuilder();
            w.AppendLine("<b> -------------------START------------ - " + DateTime.Now + "</b><br><br>");
            w.AppendLine($"<b>Page: </b>  { PageName},    <b>Procedure:  </b>   { ProcedureName}</b><br>");
            w.AppendLine($"<b>ErrorMsg: </b>  { ErrorMessage}<br><br>");
            w.AppendLine($"<b>StackTrace</b><br><br>");
            w.AppendLine($"{StackTrace}<br><br>");
            w.AppendLine("<b>------------------- END -------------</b>");
            return w.ToString();
        }


        


        public async Task SendErrorEmail(SendEmailModel mail)
        {
            var message = new MimeMessage();

            var test = _configuration.GetValue<bool>("Environment:Test");
            if (test)
                mail.SingleTo = "myinternet21@hotmail.com";

            message.To.Add(MailboxAddress.Parse(mail.SingleTo));
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

                var creds = GetCredentials();

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
                        var msg = ex.Message.ToString();
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


        private SMTPCredentials GetCredentials()
        {
            var creds = new SMTPCredentials();

            creds.pwd = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SMTPPassword").Value;
            creds.usr = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SMTPEmail").Value;
            creds.srv = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SmtpServerAddress").Value;
            creds.port = int.Parse(_configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SmtpServerPort").Value);
            creds.sslSend = bool.Parse(_configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("EnableEmailSending").Value);
            return creds;
        }
    }
}
