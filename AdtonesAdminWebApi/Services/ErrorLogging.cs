using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace AdtonesAdminWebApi.Services
{
    public class ErrorLogging //: IErrorLogging
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
        }

        public static void Log(string PageName,string ProcedureName,string ErrorMessage,string LogLevel,string StackTrace, TextWriter w)
        {
            w.WriteLine("-------------------START-------------" + DateTime.Now);
            w.WriteLine($"Page: {PageName}, Procedure: {ProcedureName},ErrorMsg: {ErrorMessage}");
            w.WriteLine($"LogLevel: {LogLevel}");
            w.WriteLine($" StackTrace  : {StackTrace}");
            w.WriteLine("------------END-------------------");
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