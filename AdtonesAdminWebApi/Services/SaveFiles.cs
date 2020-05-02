using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{
    public interface ISaveFiles
    {
        Task<string> SaveFileToSite(string dir, IFormFile data, string nameOrPath = null);
        bool DeleteFileByPath(string filepath);
        bool DeleteFileByName(string dir, string filename);
    }


    public class SaveFiles : ISaveFiles
    {
        private readonly IWebHostEnvironment env;

        public SaveFiles(IWebHostEnvironment _env)
        {
            env = _env;
        }


        public async Task<string> SaveFileToSite(string dir, IFormFile data,string nameOrPath=null)
        {
            try
            {
                var otherpath = env.ContentRootPath;
                var fileName = DateTime.Now.Ticks + System.IO.Path.GetFileName(data.FileName);
                var filePath = Path.Combine(otherpath + dir, data.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await data.CopyToAsync(fileStream);
                }
                if (nameOrPath.ToLower() == "path")
                    return filePath;
                else if (nameOrPath.ToLower() == "name")
                    return fileName;
                else
                    return filePath;
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
    
    
    }
}
