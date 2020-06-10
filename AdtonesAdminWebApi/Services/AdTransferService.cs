using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{
    
    public interface IAdTransferService
    {
        Task<string> CopyAdToOpeartorServer(string conn, UserAdvertResult advert);
    }



    public class AdTransferService : IAdTransferService
    {
        private readonly IAdvertDAL _advertDAL;
        private readonly IAdvertQuery _commandText;

        public AdTransferService(IAdvertDAL advertDAL, IAdvertQuery commandText)
        {
            _advertDAL = advertDAL;
            _commandText = commandText;
        }

        public async Task<string> CopyAdToOpeartorServer(string conn, UserAdvertResult advert)
        {
            try
            {

                var mediaFile = advert.MediaFile;
                if (!string.IsNullOrEmpty(mediaFile) && advert.UploadedToMediaServer == false)
                {
                    FtpDetailsModel getFTPdetails = await _advertDAL.GetFtpDetails(_commandText.GetFtpDetails, advert.OperatorId);

                    if (getFTPdetails != null)
                    {
                        var host = getFTPdetails.Host;
                        var port = Convert.ToInt32(getFTPdetails.Port);
                        var username = getFTPdetails.UserName;
                        var password = getFTPdetails.Password;
                        var localRoot = "";//System.Web.HttpContext.Current.Server.MapPath("~/Media"); ///TODO:
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
                                // var SourceFile = localRoot + @"\" + advert.UserId + @"\" + System.IO.Path.GetFileName(advert.MediaFileLocation);
                                var SourceFile = $"{localRoot}\\{advert.UserId}\\{System.IO.Path.GetFileName(advert.MediaFileLocation)}";
                                var DestinationFile = ftpRoot + "/" + System.IO.Path.GetFileName(advert.MediaFileLocation);
                                var filestream = new FileStream(SourceFile, FileMode.Open);
                                //client.UploadFile(filestream, "/usr/local/arthar" + DestinationFile, null);
                                //client.UploadFile(filestream, DestinationFile);
                                client.UploadFile(filestream, DestinationFile, null);
                                filestream.Close();

                                if (advert.OperatorId == (int)Enums.OperatorTableId.Safaricom) // Second File Transfer
                                {
                                    var adName = System.IO.Path.GetFileName(advert.MediaFile);
                                    var temp = adName.Split('.')[0];
                                    var secondAdname = Convert.ToInt64(temp) + 1;

                                    // var SourceFile2 = localRoot + @"\" + advert.UserId + @"\SecondAudioFile\" + secondAdname + ".wav";
                                    var SourceFile2 = $"{localRoot}\\{advert.UserId}\\SecondAudioFile\\{secondAdname}.wav";
                                    var DestinationFile2 = ftpRoot + "/" + secondAdname + ".wav";
                                    var filestream2 = new FileStream(SourceFile2, FileMode.Open);
                                    client.UploadFile(filestream2, DestinationFile2, null);
                                    filestream2.Close();
                                }
                                var x = _advertDAL.UpdateMediaLoaded(_commandText.UpdateMediaLoaded, advert);
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
    }
}