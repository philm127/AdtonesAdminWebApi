using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Model;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace AdtonesAdminWebApi.Services
{
    
    public interface ISoapApiService
    {
        Task<string> DeleteToneSoapApi(int advertId);
        string AddSoapUser(string phoneNumber);
        string DeleteSoapUser(int userId, string phoneNumber);
        string UploadSoapTone(int advertId);
        string DeleteSoapTone(int advertId);
        string UploadToneOnCRBTServer(int advertId);
    }


    public class SoapApiService : ISoapApiService
    {

        private static IConfiguration _configuration;
        private readonly IUserProfileDAL _profDAL;
        private readonly IUserProfileQuery _profQuery;
        private readonly IUserManagementDAL _userDAL;
        private readonly IUserManagementQuery _userText;
        private readonly IAdvertDAL _adDAL;
        private readonly IAdvertQuery _adText;

        public SoapApiService(IConfiguration configuration, IUserProfileDAL profDAL, IUserProfileQuery profQuery,
                                IUserManagementDAL userDAL, IUserManagementQuery userText, IAdvertDAL adDAL, IAdvertQuery adText)
        {
            _configuration = configuration;
            _profDAL = profDAL;
            _profQuery = profQuery;
            _userDAL = userDAL;
            _userText = userText;
            _adDAL = adDAL;
            _adText = adText;
        }

        static string portalAccount = _configuration.GetValue<string>("AppSettings:PortalAccount");
        static string portalPassword = _configuration.GetValue<string>("AppSettings:PortalPassword");
        static string portalType = _configuration.GetValue<string>("AppSettings:PortalType");
        //static string role = _configuration.GetValue<string>("AppSettings:Role");
        //static string roleCode = _configuration.GetValue<string>("AppSettings:RoleCode");
        //static string corpCode = _configuration.GetValue<string>("AppSettings:CorpCode");
        static string moduleCode = _configuration.GetValue<string>("AppSettings:ModuleCode");
        //static string corpId = _configuration.GetValue<string>("AppSettings:CorpID");
        static string safricomUrl = _configuration.GetValue<string>("AppSettings:SafricomSoapUrl");
        //static string TIBCOUrl = _configuration.GetValue<string>("AppSettings:AdtonesTIBCOBinding");
        //static string TIBCOUserName = _configuration.GetValue<string>("AppSettings:AdtonesTIBCOUserName");
        //static string TIBCOPassword = _configuration.GetValue<string>("AppSettings:AdtonesTIBCOPassword");
        

        public async Task<string> DeleteToneSoapApi(int advertId)
        {
            try
            {
                var advertData = await _adDAL.GetAdvertDetail(_adText.GetAdvertDetail, advertId);

                if (!string.IsNullOrEmpty(advertData.MediaFileLocation))
                {
                    //var portalAccount = "adtones";
                    //var portalPassword = "TDD";
                    //var portalType = "1";
                    var role = "3";
                    var roleCode = "admin";
                    var toneID = advertData.SoapToneId;
                    var toneCode = advertData.SoapToneCode;
                    string soapUIUrl = _configuration.GetValue<string>("AppSettings:MediaSoapUIUrl");
                    var client = new RestClient(soapUIUrl);

                    var request = new RestRequest(Method.POST);

                    //Role = 3, roleCode = admin, transactionID = should be sequential like toneCode  for delete tone as per the latest changes
                    request.AddParameter("undefined", "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ton=\"http://toneprovide.ivas.huawei.com\">\r\n  <soapenv:Header/>\r\n  <soapenv:Body>\r\n     <ton:delTone soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n        <event xsi:type=\"even:DelToneEvt\" xmlns:even=\"http://event.toneprovide.ivas.huawei.com\">\r\n" +
                        "<portalAccount xsi:type=\"xsd:string\">" + portalAccount + "</portalAccount>\r\n" +
                        "<portalPwd xsi:type=\"xsd:string\">" + portalPassword + "</portalPwd>\r\n" +
                        "<portalType xsi:type=\"xsd:string\">" + portalType + "</portalType>\r\n" +
                        "<moduleCode xsi:type=\"xsd:string\">" + moduleCode + "</moduleCode>\r\n" +
                        "<role xsi:type=\"xsd:string\">" + role + "</role>\r\n" +
                        "<roleCode xsi:type=\"xsd:string\">" + roleCode + "</roleCode>\r\n" +
                        "<needApproved xsi:type=\"xsd:string\">0</needApproved>\r\n" +
                        "<toneID xsi:type=\"xsd:string\">" + toneID + "</toneID>\r\n" +
                        "<transactionID xsi:type=\"xsd:string\">" + toneCode + "</transactionID>\r\n" +
                        "</event>\r\n     </ton:delTone>\r\n  </soapenv:Body>\r\n</soapenv:Envelope>", ParameterType.RequestBody);

                    IRestResponse response = client.Execute(request);
                    var responseContent = response.Content;

                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(responseContent);
                        // XmlNodeList nodeList = xmldoc.GetElementsByTagName("delToneReturn");
                        XmlNodeList nodeList = xmldoc.GetElementsByTagName("multiRef");
                        if (nodeList.Count > 0)
                        {
                            foreach (XmlNode node in nodeList)
                            {
                                return node.SelectSingleNode("returnCode").InnerXml;
                            }
                        }
                    }
                    else
                    {
                        return response.StatusCode.ToString();
                    }
                }

            }
            catch
            {
                return "100001"; // "Unknown Error" 
            }
            return "100001"; // "Unknown Error" 
        }

        
        #region Safaricom API Call
        public string AddSoapUser(string phoneNumber)
        {
            var url = safricomUrl + "AddUserFromWebsite?phoneNumber=" + phoneNumber;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content.Replace("\"", "");
        }

        public string DeleteSoapUser(int userId, string phoneNumber)
        {
            var url = safricomUrl + "DeleteUserFromWebsite?userId=" + userId + "&phoneNumber=" + phoneNumber;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content.Replace("\"", "");
        }

        public string UploadSoapTone(int advertId)
        {
            var url = safricomUrl + "UploadTone?advertId=" + advertId;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content.Replace("\"", "");
        }

        public string DeleteSoapTone(int advertId)
        {
            var url = safricomUrl + "DeleteTone?advertId=" + advertId;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content.Replace("\"", "");
        }

        public string UploadToneOnCRBTServer(int advertId)
        {
            var safariProjURL = _configuration.GetValue<string>("AppSettings:SafricomProjUrl");
            //var url = safariProjURL + "AdTransfer/Index?advertId=" + advertId;
            // var url = "http://172.29.128.103/AdTransfer/Index?advertId=1332";
            var url = safricomUrl + "AdvertTransfer?advertId=" + advertId;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content.Replace("\"", "");
        }
        #endregion

    }
}