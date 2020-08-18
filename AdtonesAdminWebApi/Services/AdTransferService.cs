using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{
    
    public interface IAdTransferService
    {
        Task<string> CopyAdToOperatorServer(string conn, UserAdvertResult advert);
        Task<bool> CopyPromoAdToOperatorServer(PromotionalCampaignResult model, string fileName);
    }



    public class AdTransferService : IAdTransferService
    {
        private readonly IAdvertDAL _advertDAL;
        private readonly IWebHostEnvironment env;

        public AdTransferService(IAdvertDAL advertDAL, IWebHostEnvironment _env)
        {
            _advertDAL = advertDAL;
            env = _env;
        }

        public async Task<string> CopyAdToOperatorServer(string conn, UserAdvertResult advert)
        {
            try
            {
                var otherpath = env.ContentRootPath;
                var mediaFile = advert.MediaFile;
                if (!string.IsNullOrEmpty(mediaFile) && advert.UploadedToMediaServer == false)
                {
                    FtpDetailsModel getFTPdetails = await _advertDAL.GetFtpDetails(advert.OperatorId);

                    if (getFTPdetails != null)
                    {
                        var dir = "Media";// 
                        var host = getFTPdetails.Host;
                        var port = Convert.ToInt32(getFTPdetails.Port);
                        var username = getFTPdetails.UserName;
                        var password = getFTPdetails.Password;
                        var localRoot = Path.Combine(otherpath, dir); //System.Web.HttpContext.Current.Server.MapPath("~/Media"); ///TODO:
                        var ftpRoot = getFTPdetails.FtpRoot;

                        // Test FTP Details
                        //var host = "138.68.177.47";
                        //var port = 22;
                        //var username = "provisioning";
                        //var password = "adtonespassword";
                        //var localRoot = System.Web.HttpContext.Current.Server.MapPath("~/Media");
                        //var ftpRoot = "/usr/local/arthar/adds";

                        //Live FTP Details
                        //var host = "sftp://10.5.46.46";
                        //var port = 22;
                        //var username = "usdpuser";
                        //var password = "Huawei_123";
                        //var localRoot = System.Web.HttpContext.Current.Server.MapPath("~/Media");
                        //var ftpRoot = "/mnt/Y:/share";

                        using (var client = new Renci.SshNet.SftpClient(host, port, username, password))
                        {
                            client.Connect();
                            if (client.IsConnected)
                            {
                                var SourceFile = $"{localRoot}\\{advert.UserId}\\{System.IO.Path.GetFileName(advert.MediaFileLocation)}";
                                var DestinationFile = ftpRoot + "/" + System.IO.Path.GetFileName(advert.MediaFileLocation);
                                var filestream = new FileStream(SourceFile, FileMode.Open);
                                client.UploadFile(filestream, DestinationFile, null);
                                filestream.Close();

                                if (advert.OperatorId == (int)Enums.OperatorTableId.Safaricom) // Second File Transfer
                                {
                                    var adName = System.IO.Path.GetFileName(advert.MediaFile);
                                    var temp = adName.Split('.')[0];
                                    var secondAdname = Convert.ToInt64(temp) + 1;

                                    var SourceFile2 = $"{localRoot}\\{advert.UserId}\\SecondAudioFile\\{secondAdname}.wav";
                                    var DestinationFile2 = ftpRoot + "/" + secondAdname + ".wav";
                                    var filestream2 = new FileStream(SourceFile2, FileMode.Open);
                                    client.UploadFile(filestream2, DestinationFile2, null);
                                    filestream2.Close();
                                }
                                var x = _advertDAL.UpdateMediaLoaded(advert);
                            }
                            client.Disconnect();
                        }
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        public async Task<bool> CopyPromoAdToOperatorServer(PromotionalCampaignResult model, string fileName)
        {
            try
            {
                var otherpath = env.ContentRootPath;
                var mediaFile = model.AdvertLocation;
                if (!string.IsNullOrEmpty(mediaFile))
                {
                    FtpDetailsModel getFTPdetails = await _advertDAL.GetFtpDetails(model.OperatorId);

                    if (getFTPdetails != null)
                    {
                        var host = getFTPdetails.Host;
                        var port = Convert.ToInt32(getFTPdetails.Port);
                        var username = getFTPdetails.UserName;
                        var password = getFTPdetails.Password;
                        var localRoot = Path.Combine(otherpath, mediaFile);
                        var ftpRoot = getFTPdetails.FtpRoot;

                        using (var client = new Renci.SshNet.SftpClient(host, port, username, password))
                        {
                            client.Connect();
                            if (client.IsConnected)
                            {
                                var SourceFile = localRoot;
                                var DestinationFile = ftpRoot + "/" + fileName;
                                var filestream = new FileStream(SourceFile, FileMode.Open);
                                client.UploadFile(filestream, DestinationFile, null);
                                filestream.Close();
                            }
                            client.Disconnect();
                        }
                    }
                }
                return true;
            }
            catch
            {
                throw;
            }
        }



    }
}