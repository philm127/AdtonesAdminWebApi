using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{
    public interface ISaveGetFiles
    {
        Task<string> SaveFileToSite(string dir, IFormFile data);
        bool DeleteFileByPath(string filepath);
        bool DeleteFileByName(string dir, string filename);
        Task<bool> SaveFileToAdtones(string dir, IFormFile data, string filename = null);
        string TempGetGeneralJsonFile(string name = null);
        Task<IFormFile> GetIformFileFromPath(string path);
    }


    public class SaveGetFiles : ISaveGetFiles
    {
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _configuration;

        public SaveGetFiles(IWebHostEnvironment _env, IConfiguration configuration)
        {
            env = _env;
            _configuration = configuration;
        }


        public async Task<string> SaveFileToSite(string dir, IFormFile data)
        {
            try
            {
                var otherpath = env.ContentRootPath;
                var fileName = DateTime.Now.Ticks + System.IO.Path.GetFileName(data.FileName);
                var directoryName = Path.Combine(otherpath, dir);

                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                var filePath = Path.Combine(otherpath + dir, data.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await data.CopyToAsync(fileStream);
                }
                return fileName;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "Common-SaveFiles",
                    ProcedureName = "SaveFileToSite"
                };
                _logging.LogError();
                return "failed";
            }
        }


        public async Task<bool> SaveFileToAdtones(string dir, IFormFile data, string filename = null)
        {
            try
            {
                var otherpath = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress");
                var directoryName = otherpath + dir;
                string filePath = string.Empty;
                if(filename != null)
                    filePath = Path.Combine(directoryName, data.FileName);
                else
                    filePath = Path.Combine(directoryName, filename);

                using (var webClient = new WebClient())
                {
                    using (Stream uploadStream = webClient.OpenWrite(filePath))
                    {
                        data.CopyTo(uploadStream);
                    }
                }

                //using (var fileStream = new FileStream(filePath, FileMode.CreateNew))
                //{
                //    await data.CopyToAsync(fileStream);
                //}
                return true;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "Common-SaveFiles",
                    ProcedureName = "SaveFileToAdtones"
                };
                _logging.LogError();
                return false;
            }
        }


        /// <summary>
        /// Used when the whole filepath is supplied as a value
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public bool DeleteFileByPath(string filepath)
        {
            try
            {
                System.IO.File.Delete(filepath);
                return true;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "Common-SaveFiles",
                    ProcedureName = "DeleteFile"
                };
                _logging.LogError();
                return false;
            }
        }


        /// <summary>
        /// When filepath has to be constructed from directory and filename
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool DeleteFileByName(string dir, string filename)
        {
            try
            {
                var otherpath = env.ContentRootPath;
                var filePath = Path.Combine(otherpath + dir, filename);
                System.IO.File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "Common-SaveFiles",
                    ProcedureName = "DeleteFileByName"
                };
                _logging.LogError();
                return false;
            }
        }


        public async Task<IFormFile> GetIformFileFromPath(string path)
        {
            // path = "https://my.adtones.com/Invoice/Adtones_invoice_A54928820.pdf";// + pdfModel.InvoiceNumber + ".pdf");
            path = "Invoice/Adtones_invoice_A54928820.pdf";
            var otherpath = env.ContentRootPath;
            var filePath = Path.Combine(otherpath,path);
            var ms = new MemoryStream();
            try
            {
               
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(ms);
                }
                
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "Services-SaveFiles",
                    ProcedureName = "GetIformFileFromPath"
                };
                _logging.LogError();
                // return new FormFile();
            }
            var res = new FileStreamResult(ms, "doc/pdf");
            using (var fs = res.FileStream)
            {
                return new FormFile(fs, 0, fs.Length, "name", res.FileDownloadName);
            }

        }


        //public (string fileType, byte[] archiveData, string archiveName) FetechFiles(string filepath,string filetype)
        //{
        //    var otherpath = env.ContentRootPath;
        //    var fileName = Path.GetFileName(filepath);
        //    var mimeType = "application/pdf";
        //    long fileBytes = filepath.Length;

        //    //return new FileContentResult(fileBytes, mimeType)
        //    //{
        //    //    FileDownloadName = fileName
        //    //};

        //}

        public string TempGetGeneralJsonFile(string name = null)
        {
            var directoryName = string.Empty;
            var dir = "\\TempGenPermissions\\general.json";
            var otherpath = env.ContentRootPath;
            if(name == "op")
                directoryName = "C:\\Development\\Adtones-Admin\\AdtonesAdminWebApi\\AdtonesAdminWebApi\\AdtonesAdminWebApi\\TempGenPermissions\\generalOp.json";//Path.Combine(otherpath, dir);
            else
                directoryName = "C:\\Development\\Adtones-Admin\\AdtonesAdminWebApi\\AdtonesAdminWebApi\\AdtonesAdminWebApi\\TempGenPermissions\\general.json";
            
            return directoryName;
        }

    }
}
