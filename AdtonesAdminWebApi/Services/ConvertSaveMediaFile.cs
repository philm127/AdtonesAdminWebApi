﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
// using MadScripterWrappers;
using System.Text.Json;
using AdtonesAdminWebApi.ViewModels;
using MadScripterWrappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AdtonesAdminWebApi.Services
{
    
    public interface IConvertSaveMediaFile
    {
        string ConvertAndSaveMediaFile(string audioFilePath, string extension, string outputFormat, string onlyFileName, string directoryName);
    }
    public class ConvertSaveMediaFile : IConvertSaveMediaFile
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly ILoggingService _logServ;

        public ConvertSaveMediaFile(IWebHostEnvironment env, IConfiguration configuration, ILoggingService logServ)
        {
            _env = env;
            _configuration = configuration;
            _logServ = logServ;
        }

        public string ConvertAndSaveMediaFile(string audioFilePath, string extension, string outputFormat, string onlyFileName, string directoryName)
        {
            var otherpath = _configuration.GetValue<string>("AppSettings:adtonesServerDirPath");
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

                    string archiveDirectoryName = Path.Combine(otherpath, @"PromotionalMedia/Archive/");

                    string archivePath = Path.Combine(archiveDirectoryName, fileName + "." + outputFormat);

                    System.IO.File.Copy(savePath, archivePath, true);

                    System.IO.File.Delete(audioFilePath);

                    return fileName + "." + outputFormat;
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = "ConvertSaveMediaFile";
                _logServ.ProcedureName = "ConvertAndSaveMediaFile";
                _logServ.LogError();
                
                return "fail";
            }
        }
    }
}

