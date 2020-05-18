using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MadScripterWrappers;
using System.Text.Json;

namespace AdtonesAdminWebApi.Services
{
    public class ConvertSaveMediaFile
    {
        //Convert File
        public async Task<bool> ConvertAndSaveMediaFile(string audioFilePath, string extension, string operatorId, string fileName, string outputFormat)
        {
            try
            {
                var id = Convert.ToInt32(operatorId);
                string inputFormat = extension.Replace(".", "");

                CloudConvert api = new CloudConvert("WNdHFlLrT9GdETzjTJC4BoUsjE6tXbRi8sZX5aokQbua3D2hbJITOTylPs7Nre1A");
                var url = api.GetProcessURL(inputFormat, outputFormat);

                Dictionary<string, string> options = new Dictionary<string, string>()
            {
                { "audio_codec", "PCM MU-LAW" },
                { "audio_bitrate", "64 kbps" },
                { "audio_frequency", "8000" },
                { "audio_channels", "1" }
            };

                var convertedFile = api.UploadFile(url, audioFilePath, outputFormat, null, options);
                var convertedMediaData =  new JavaScriptSerializer().Deserialize<CloudConvertModel.RootObject>(convertedFile);

                using (WebClient webClient = new WebClient())
                {
                    var downloadUrl = "http:" + convertedMediaData.output.url;
                    string directoryName = Server.MapPath("~/PromotionalMedia/");
                    directoryName = Path.Combine(directoryName, operatorId);

                    if (!Directory.Exists(directoryName))
                        Directory.CreateDirectory(directoryName);

                    string savePath = Path.Combine(directoryName, fileName + "." + outputFormat);

                    webClient.DownloadFile(downloadUrl, savePath);

                    string archiveDirectoryName = Server.MapPath("~/PromotionalMedia/Archive/");

                    if (!Directory.Exists(archiveDirectoryName))
                        Directory.CreateDirectory(archiveDirectoryName);

                    string archivePath = Path.Combine(archiveDirectoryName, fileName + "." + outputFormat);

                    System.IO.File.Copy(savePath, archivePath, true);

                    System.IO.File.Delete(audioFilePath);

                    return true;
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignService",
                    ProcedureName = "ConvertSaveMediaFile"
                };
                _logging.LogError();
                return false;
            }
        }
    }
}

