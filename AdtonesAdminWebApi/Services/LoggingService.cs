using AdtonesAdminWebApi.Services.Mailer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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
        private readonly ISendEmailMailer _mailer;
        private readonly IConfiguration _configuration;


        public LoggingService(ISendEmailMailer mailer, IConfiguration configuration)
        {
            _mailer = mailer;
            _configuration = configuration;
        }


        public string RootPath()
        {
            /// TODO: Check the working of the file paths
            var WebRootPath = Directory.GetCurrentDirectory();
            string webroot = WebRootPath + "\\Logging\\";
            return webroot;
        }


        public async Task LogError()
        {
            string webroot = RootPath();
            string ResolvePath = "";
            ResolvePath = string.Concat(webroot + "ErrorLogs.txt");
            var filepath = ResolvePath;
            var messageToWrite = LogMessageBuilder();

            await WriteTextAsync(filepath, messageToWrite);
            await SendErrorEmail();
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


        public async Task SendErrorEmail()
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

                await _mailer.SendBasicEmail(mail);
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


        public async void LogInfo()
        {
            string webroot = RootPath();
            // string MainFolderPath = "Logging\\";
            string ResolvePath = "";
            ResolvePath = string.Concat(webroot + "InfoLogs.txt");
            var filepath = ResolvePath;

            var messageToWrite = LogMessageBuilder();

            await WriteTextAsync(filepath, messageToWrite);
        }
    }
}
