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
            SendErrorEmail();
        }


        static async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }


        public void SendErrorEmail()
        {
            var test = _configuration.GetValue<string>("Environment:Location");
            if (test != "development")
            {
                var mail = new SendEmailModel();
                string emailContent = string.Empty;
                emailContent = EmailMessageBuilder();
                emailContent = string.Format(emailContent);
                mail.From = "support@adtones.com";
                if (test == "production")
                    mail.Subject = "Admin Adtone API error log at  " + DateTime.Now;
                else
                    mail.Subject = "UAT Testing Adtone API error log at  " + DateTime.Now;
                mail.Body = emailContent;
                _mailer.SendBasicEmail(mail);
            }
        }

            

        public string LogMessageBuilder()
        {
            var w = new StringBuilder();
            w.Append("-------------------START-------------" + DateTime.Now);
            w.AppendLine($"Page: {PageName}, Procedure: {ProcedureName},ErrorMsg: {ErrorMessage}");
            w.AppendLine($"LogLevel: {LogLevel}");
            w.AppendLine($" StackTrace  : {StackTrace}");
            w.AppendLine("------------END-------------------");
            return w.ToString();
        }

    public string EmailMessageBuilder()
    {
        var w = new StringBuilder();
        w.Append("<b> -------------------START------------ - " + DateTime.Now + "</b><br><br>");
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
