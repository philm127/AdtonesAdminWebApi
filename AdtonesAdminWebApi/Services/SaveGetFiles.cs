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
        Task<string> SaveFileToSite(string dir, IFormFile data, string originalFileName = null);
        Task<string> SaveOriginalFileToSite(string dir, IFormFile data, string originalFileName = null);
        bool DeleteFileByName(string dir, string filename);
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


        public async Task<string> SaveFileToSite(string dir, IFormFile data, string originalFileName = null)
        {
            try
            {
                string fileName = string.Empty;
                var otherpath = _configuration.GetValue<string>("AppSettings:adtonesServerDirPath");
                if (originalFileName != null)
                    fileName = originalFileName;
                else
                    fileName = DateTime.Now.Ticks + System.IO.Path.GetFileName(data.FileName);
                var directoryName = Path.Combine(otherpath, dir);

                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                var filePath = Path.Combine(directoryName, fileName);
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


        

        public async Task<string> SaveOriginalFileToSite(string dir, IFormFile data, string originalFileName = null)
        {
            try
            {
                var otherpath = _configuration.GetValue<string>("AppSettings:adtonesServerDirPath");
                var fileName = System.IO.Path.GetFileName(data.FileName);
                var directoryName = Path.Combine(otherpath, dir);

                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                var filePath = Path.Combine(directoryName, fileName);
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
                var otherpath = _configuration.GetValue<string>("AppSettings:adtonesServerDirPath");
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


        
    }
}
