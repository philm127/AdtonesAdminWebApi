using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using MadScripterWrappers;
using System.Text.Json;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace AdtonesAdminWebApi.Services
{
    public static class ConvertSaveMediaFile
    {
        public static IWebHostEnvironment env { get; private set; }
        public static string ConvertAndSaveMediaFile(string audioFilePath, string extension, string outputFormat, string onlyFileName, string directoryName)
        {
            var otherpath = env.ContentRootPath;
            try
            {
                string inputFormat = extension.Replace(".", "");
                string fileName = DateTime.Now.Ticks + onlyFileName;

                CloudConvert api = new CloudConvert("WNdHFlLrT9GdETzjTJC4BoUsjE6tXbRi8sZX5aokQbua3D2hbJITOTylPs7Nre1A");
                var url = api.GetProcessURL(inputFormat, outputFormat);

                Dictionary<string, string> options = new Dictionary<string, string>()
            {
                { "audio_codec", "PCM MU-LAW" },
                { "audio_bitrate", "64 kbps" },
                { "audio_frequency", "8000" },
                { "audio_channels", "1" }
            };

                var fullAudioPath = Path.Combine(otherpath, audioFilePath);

                var convertedFile = api.UploadFile(url, fullAudioPath, outputFormat, null, options);
                var convertedMediaData = JsonSerializer.Deserialize<CloudConvertModel.RootObject>(convertedFile);

                using (WebClient webClient = new WebClient())
                {
                    var downloadUrl = "http:" + convertedMediaData.output.url;

                    var fullDirectoryName = Path.Combine(otherpath, directoryName);

                    if (!Directory.Exists(fullDirectoryName))
                        Directory.CreateDirectory(fullDirectoryName);

                    string savePath = Path.Combine(fullDirectoryName, fileName + "." + outputFormat);

                    webClient.DownloadFile(downloadUrl, savePath);

                    string archiveDirectoryName = Path.Combine(otherpath, "PromotionalMedia/Archive/");

                    string archivePath = Path.Combine(archiveDirectoryName, fileName + "." + outputFormat);

                    System.IO.File.Copy(savePath, archivePath, true);

                    System.IO.File.Delete(audioFilePath);

                    return fileName + "." + outputFormat;
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "ConvertSaveMediaFile",
                    ProcedureName = "ConvertAndSaveMediaFile"
                };
                _logging.LogError();
                return "fail";
            }
        }
    }
}

