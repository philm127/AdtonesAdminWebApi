using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.Services.Mailer;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using MimeKit;

namespace AdtonesAdminWebApi.Services
{
    public class XErrorLogging //: IErrorLogging
    {
        public string PageName { get; set; }
        public string ProcedureName { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string LogLevel { get; set; }


        public void LogError()
        {
            /// TODO: Check the working of the file paths
            var WebRootPath = Directory.GetCurrentDirectory();

            string webroot = WebRootPath + "\\Logging\\";
            // string MainFolderPath = "Logging\\";
            string ResolvePath = "";
            ResolvePath = string.Concat(webroot + "ErrorLogs.txt");
            var filepath = ResolvePath;

            //if (System.IO.File.Exists(filepath))
            //{
            using (StreamWriter w = File.AppendText(filepath))
            {
                Log(PageName,ProcedureName,ErrorMessage,LogLevel,StackTrace, w);
                //Log("Test2", w);
            }
            //using (StreamWriter writer = new StreamWriter(filepath, true))
            //    {
            //        writer.WriteLine("-------------------START-------------" + DateTime.Now);
            //        writer.WriteLine("Page: " + PageName + ", Procedure: " + ProcedureName + ",ErrorMsg: " + ErrorMessage + ", StackTrace: " + StackTrace);
            //        writer.WriteLine("-------------------END-------------" + DateTime.Now);
            //        writer.Flush();
            //        writer.Close();
            //    }
            //}
            //else
            //{
            //    using (StreamWriter writer = System.IO.File.CreateText(filepath))
            //    {
            //        writer.WriteLine("-------------------START-------------" + DateTime.Now);
            //        writer.WriteLine("Page: " + PageName + ", Procedure: " + ProcedureName + ",ErrorMsg: " + ErrorMessage + ", StackTrace: " + StackTrace);
            //        writer.WriteLine("-------------------END-------------" + DateTime.Now);
            //        writer.Flush();
            //        writer.Close();
            //    }
            //}

            var mail = new SendEmailModel();
            string emailContent = string.Empty;

            emailContent = "<b>-------------------START-------------" + DateTime.Now + "</b><br><br>\n";
            emailContent += $"<b>Page: </b>  { PageName},    <b>Procedure:  </b>   { ProcedureName}</b><br>\n";
            emailContent += $"<b>ErrorMsg: </b>  { ErrorMessage}<br><br>\n";
            emailContent += $"<b>StackTrace</b><br><br>\n";
            emailContent += $"{StackTrace}<br><br>\n";
            emailContent += "<b>------------------- END -------------</b>\n";


            emailContent = string.Format(emailContent);
            mail.From = "support@adtones.com";
            mail.Subject = "Admin Adtone API error log at  " + DateTime.Now;
            mail.Body = emailContent;
            SendErrorEmail(mail);
        }

        public static void Log(string PageName,string ProcedureName,string ErrorMessage,string LogLevel,string StackTrace, TextWriter w)
        {
            w.WriteLine("-------------------START-------------" + DateTime.Now);
            w.WriteLine($"Page: {PageName}, Procedure: {ProcedureName},ErrorMsg: {ErrorMessage}");
            w.WriteLine($"LogLevel: {LogLevel}");
            w.WriteLine($" StackTrace  : {StackTrace}");
            w.WriteLine("------------END-------------------");
        }


        public void SendErrorEmail(SendEmailModel mail)
        {
            var message = new MimeMessage();
            //mail.SingleTo = "support@adtones.com";
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
            message.Cc.Add(MailboxAddress.Parse("philm127@gmail.com"));
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
                        var _logging = new ErrorLogging()
                        {
                            ErrorMessage = ex.Message.ToString(),
                            StackTrace = ex.StackTrace.ToString(),
                            PageName = "Services-Mailer-SendEmailMailer",
                            ProcedureName = "ErrorLogging - SendEmail"
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


        private SMTPCredentials GetCredentials()
        {
            var creds = new SMTPCredentials();
            creds.pwd = "Supp0rtPa55w0rd!";
            creds.usr = "support@adtones.com";
            creds.srv = "auth.smtp.1and1.co.uk";
            creds.port = 587;
            creds.sslSend = false;

            return creds;
        }


        public async void LogInfo()
        {
            /// TODO: Check the working of the file paths
            var WebRootPath = Directory.GetCurrentDirectory();

            string webroot = WebRootPath + "\\Logging\\";
            // string MainFolderPath = "Logging\\";
            string ResolvePath = "";
            ResolvePath = string.Concat(webroot + "InfoLogs.txt");
            var filepath = ResolvePath;

            if (System.IO.File.Exists(filepath))
            {
                using (StreamWriter writer = new StreamWriter(filepath, true))
                {
                    writer.WriteLine("-------------------START-------------" + DateTime.Now);
                    writer.WriteLine("Level: " + LogLevel + ", Page: " + PageName + ", Procedure: " + ProcedureName + ",TnfoMsg: " + ErrorMessage);
                    writer.WriteLine("-------------------END-------------" + DateTime.Now);
                    writer.Flush();
                    writer.Close();
                }
            }
            else
            {
                using (StreamWriter writer = System.IO.File.CreateText(filepath))
                {
                    writer.WriteLine("-------------------START-------------" + DateTime.Now);
                    writer.WriteLine("Level: " + LogLevel + ", Page: " + PageName + ", Procedure: " + ProcedureName + ",InfoMsg: " + ErrorMessage);
                    writer.WriteLine("-------------------END-------------" + DateTime.Now);
                    writer.Flush();
                    writer.Close();
                }
            }
        }
    }


    
}