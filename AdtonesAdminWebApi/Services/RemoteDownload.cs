using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{
    public class RemoteDownload
    {
       
        public class HttpRemoteDownload : RemoteDownload
        {

            public bool DownloadFile(string urlString, string descFilePath)
            {
                string fileName = System.IO.Path.GetFileName(urlString);
                string descFilePathAndName =
                    System.IO.Path.Combine(descFilePath, fileName);
                try
                {
                    WebRequest myre = WebRequest.Create(urlString);
                }
                catch
                {
                    return false;
                }
                try
                {
                    byte[] fileData;
                    using (WebClient client = new WebClient())
                    {
                        fileData = client.DownloadData(urlString);
                    }
                    using (FileStream fs =
                          new FileStream(descFilePathAndName, FileMode.OpenOrCreate))
                    {
                        fs.Write(fileData, 0, fileData.Length);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("download field", ex.InnerException);
                }
            }
        }
    }
}
