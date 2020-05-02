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
        public string clientName { get; set; }


        public void LogError()
        {
            /// TODO: Check the working of the file paths
            var WebRootPath = Directory.GetCurrentDirectory();

            string webroot = WebRootPath + "\\Logging\\";
            // string MainFolderPath = "Logging\\";
            string ResolvePath = "";
            ResolvePath = string.Concat(webroot + "ErrorLogs.txt");
            var filepath = ResolvePath;

            if (System.IO.File.Exists(filepath))
            {
                using (StreamWriter writer = new StreamWriter(filepath, true))
                {
                    writer.WriteLine("-------------------START-------------" + DateTime.Now);
                    writer.WriteLine("Client: " + clientName + ", Page: " + PageName + ", Procedure: " + ProcedureName + ",ErrorMsg: " + ErrorMessage + ", StackTrace: " + StackTrace);
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
                    writer.WriteLine("Client: " + clientName + ", Page: " + PageName + ", Procedure: " + ProcedureName + ",ErrorMsg: " + ErrorMessage + ", StackTrace: " + StackTrace);
                    writer.WriteLine("-------------------END-------------" + DateTime.Now);
                    writer.Flush();
                    writer.Close();
                }
            }
        }
    }
}