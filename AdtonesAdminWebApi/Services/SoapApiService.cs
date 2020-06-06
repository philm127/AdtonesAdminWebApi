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
    public class SoapApiService
    {

        private static IConfiguration _configuration;
        private readonly IUserProfileDAL _profDAL;
        private readonly IUserProfileQuery _profQuery;

        public SoapApiService(IConfiguration configuration, IUserProfileDAL profDAL, IUserProfileQuery profQuery)
        {
            _configuration = configuration;
            _profDAL = profDAL;
            _profQuery = profQuery;
        }

        static string portalAccount = _configuration.GetValue<string>("AppSettings:PortalAccount"];
        static string portalPassword = _configuration.GetValue<string>("AppSettings:PortalPassword"];
        static string portalType = _configuration.GetValue<string>("AppSettings:PortalType"];
        static string role = _configuration.GetValue<string>("AppSettings:Role"];
        static string roleCode = _configuration.GetValue<string>("AppSettings:RoleCode"];
        static string corpCode = _configuration.GetValue<string>("AppSettings:CorpCode"];
        static string moduleCode = _configuration.GetValue<string>("AppSettings:ModuleCode"];
        static string corpId = _configuration.GetValue<string>("AppSettings:CorpID"];
        static string safricomUrl = _configuration.GetValue<string>("AppSettings:SafricomSoapUrl"];
        static string TIBCOUrl = _configuration.GetValue<string>("AppSettings:AdtonesTIBCOBinding"];
        static string TIBCOUserName = _configuration.GetValue<string>("AppSettings:AdtonesTIBCOUserName"];
        static string TIBCOPassword = _configuration.GetValue<string>("AppSettings:AdtonesTIBCOPassword"];
        

        public static string AddCorpUser(string phoneNum)
        {
            try
            {

                var phoneNumber = phoneNum;
                string soapUIUrl = _configuration.GetValue<string>("AppSettings:SoapUIUrl"];
                var client = new RestClient(soapUIUrl);

                var request = new RestRequest(Method.POST);

                #region Old Soap API Request
                // Old Soap Api Request
                //request.AddParameter("undefined",
                //    "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:cor=\"http://corpusermanage.ivas.huawei.com\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n   <soapenv:Header/>\r\n   <soapenv:Body>\r\n      <cor:addCorpUser soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n" +
                //    "<event xsi:type=\"even:AddCorpUserEvt\" xmlns:even=\"http://event.corpusermanage.ivas.huawei.com\">\r\n           \t" +
                //        "<portalAccount>" + portalAccount + "</portalAccount>\r\n\t\t\t<portalPwd>" + portalPassword + "</portalPwd>\r\n\t\t\t" +
                //        "<portalType>" + portalType + "</portalType>\r\n\t\t\t<role>" + role + "</role>\r\n\t\t\t" +
                //        "<roleCode>" + roleCode + "</roleCode>\r\n\t\t\t" +
                //        "<corpID>" + corpId + "</corpID>\r\n\t\t\t<phoneNumber>" + phoneNumber + "</phoneNumber>\r\n" +
                //    "</event>\r\n      </cor:addCorpUser>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>", ParameterType.RequestBody);
                #endregion


                request.AddParameter("undefined", "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:cor=\"http://corpusermanage.ivas.huawei.com\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n  <soapenv:Header/>\r\n  <soapenv:Body>\r\n     <cor:addCorpUser soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n        <event xsi:type=\"even:AddCorpUserEvt\" xmlns:even=\"http://event.corpusermanage.ivas.huawei.com\">\r\n" +
                                    "<portalAccount xsi:type=\"xsd:string\">" + portalAccount + "</portalAccount>\r\n" +
                                    "<portalPwd xsi:type=\"xsd:string\">" + portalPassword + "</portalPwd>\r\n" +
                                    "<portalType xsi:type=\"xsd:string\">" + portalType + "</portalType>\r\n" +
                                    "<moduleCode xsi:type=\"xsd:string\">" + moduleCode + "</moduleCode>\r\n" +
                                    "<role xsi:type=\"xsd:string\">" + role + "</role>\r\n" +
                                    "<roleCode xsi:type=\"xsd:string\">" + roleCode + "</roleCode>\r\n" +
                                    "<corpCode xsi:type=\"xsd:string\">" + corpCode + "</corpCode>\r\n" +
                                    "<phoneNumber xsi:type=\"cor:ArrayOfString\" soapenc:arrayType=\"xsd:string[]\"><item>" + phoneNumber + "</item></phoneNumber>\r\n</event>\r\n" +
                                    "</cor:addCorpUser>\r\n  </soapenv:Body>\r\n</soapenv:Envelope>", ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseContent = response.Content;

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(responseContent);
                    // XmlNodeList nodeList = xmldoc.GetElementsByTagName("addCorpUserReturn");
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
                    // return "5555555";
                }

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SoapApiService",
                    ProcedureName = "AddCorpUser"
                };
                _logging.LogError();
                return "100001"; // "Unknown Error" 
            }

            return "100001"; // "Unknown Error" 

        }

        public static string DeleteCorpUser(User usr)
        {

            try
            {
                EFMVCDataContex db = new EFMVCDataContex();

                var phoneNumber = "";
                var userProfile = usr.UserProfiles.FirstOrDefault();
                if (userProfile != null)
                    phoneNumber = userProfile.MSISDN;

                var userList = db.Users.Where(s => s.UserId == usr.UserId).FirstOrDefault();
                var mobile = GetMobileWithoutCountryCode(phoneNumber, userList.Operator.CountryId);
                string soapUIUrl = _configuration.GetValue<string>("AppSettings:SoapUIUrl"];
                var client = new RestClient(soapUIUrl);

                var request = new RestRequest(Method.POST);

                #region Old delete request
                //request.AddParameter("undefined", "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:cor=\"http://corpusermanage.ivas.huawei.com\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n   <soapenv:Header/>\r\n   <soapenv:Body>\r\n      <cor:delCorpUser soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n         <event xsi:type=\"even:DelCorpUserEvt\" xmlns:even=\"http://event.corpusermanage.ivas.huawei.com\">\r\n            " +
                //    "<portalAccount xsi:type=\"xsd:string\">" + portalAccount + "</portalAccount>\r\n" +
                //    "<portalPwd xsi:type=\"xsd:string\">" + portalPassword + "</portalPwd>\r\n" +
                //    "<portalType xsi:type=\"xsd:string\">" + portalType + "</portalType>\r\n" +
                //    "<moduleCode xsi:type=\"xsd:string\">" + "?" + "</moduleCode>\r\n" +
                //    "<role xsi:type=\"xsd:string\">" + role + "</role>\r\n" +
                //    "<roleCode xsi:type=\"xsd:string\">" + roleCode + "</roleCode>\r\n " +
                //    "<corpID xsi:type=\"xsd:string\">" + corpId + "</corpID>\r\n" +
                //    "<phoneNumber>" + phoneNumber + "</phoneNumber>\r\n" +
                //    "</event>\r\n      </cor:delCorpUser>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>", ParameterType.RequestBody);
                #endregion

                request.AddParameter("undefined", "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:cor=\"http://corpusermanage.ivas.huawei.com\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n  <soapenv:Header/>\r\n  <soapenv:Body>\r\n     <cor:delCorpUser soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n        <event xsi:type=\"even:DelCorpUserEvt\" xmlns:even=\"http://event.corpusermanage.ivas.huawei.com\">\r\n" +
                                    "<portalAccount xsi:type=\"xsd:string\">" + portalAccount + "</portalAccount>\r\n" +
                                    "<portalPwd xsi:type=\"xsd:string\">" + portalPassword + "</portalPwd>\r\n" +
                                    "<portalType xsi:type=\"xsd:string\">" + portalType + "</portalType>\r\n" +
                                    "<moduleCode xsi:type=\"xsd:string\">" + moduleCode + "</moduleCode>\r\n" +
                                    "<role xsi:type=\"xsd:string\">" + role + "</role>\r\n" +
                                    "<roleCode xsi:type=\"xsd:string\">" + roleCode + "</roleCode>\r\n" +
                                    "<corpID xsi:type=\"xsd:string\">" + corpId + "</corpID>\r\n" +
                                    "<phoneNumber xsi:type=\"cor:ArrayOfString\" soapenc:arrayType=\"xsd:string[1]\">\r\n" +
                                        "<item>" + mobile + "</item>\r\n" +
                                     "</phoneNumber>\r\n        </event>\r\n" +
                                    "</cor:delCorpUser>\r\n  </soapenv:Body>\r\n</soapenv:Envelope>", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var responseContent = response.Content;

                if (!string.IsNullOrEmpty(responseContent))
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(responseContent);
                    // XmlNodeList nodeList = xmldoc.GetElementsByTagName("delCorpUserReturn");
                    XmlNodeList nodeList = xmldoc.GetElementsByTagName("multiRef");
                    if (nodeList.Count > 0)
                    {
                        foreach (XmlNode node in nodeList)
                        {
                            var returnCode = node.SelectSingleNode("returnCode").InnerXml;
                            if (returnCode == "000000")
                            {
                                var getUser = db.Users.Where(s => s.UserId == usr.UserId).FirstOrDefault();
                                getUser.IsMsisdnMatch = false;
                                getUser.Activated = 3;
                                db.SaveChanges();
                            }
                            return returnCode;
                        }
                    }
                }
                else
                {
                    return response.StatusCode.ToString();
                }
                return "100001";
            }
            catch (Exception ex)
            {
                return "100001";
            }



        }

        public static string GetMobileWithoutCountryCode(string mobile, int? countryId)
        {
            string phoneNumber = "";
            if (countryId == 9)  // Kenya
            {
                phoneNumber = mobile.Substring(mobile.Length - 9); // 9 Digit number
            }
            else
            {
                phoneNumber = mobile.Substring(mobile.Length - 10); // 10 Digit number
            }
            return phoneNumber;
        }


        public static string UpdateToneSoapApi(int advertId)
        {
            try
            {
                EFMVCDataContex db = new EFMVCDataContex();
                var advertData = db.Adverts.Where(s => s.AdvertId == advertId).FirstOrDefault();
                var mediaFileLocation = advertData.MediaFileLocation;


                if (!string.IsNullOrEmpty(mediaFileLocation))
                {

                    var adName = mediaFileLocation.Split('/')[3];
                    var temp = adName.Split('.')[0];
                    var secondAdname = Convert.ToInt64(temp) + 1;

                    //ddmmyy000000
                    var firstUploadId = DateTime.Now.ToString("dd") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yy") + "000000";
                    long uploadId = Convert.ToInt64(firstUploadId);
                    var soapUploadToneData = db.SoapUploadTone.Where(s => EntityFunctions.TruncateTime(s.CreatedDateTime) == EntityFunctions.TruncateTime(DateTime.Now)).OrderByDescending(s => s.Id).FirstOrDefault();
                    if (soapUploadToneData != null)
                    {
                        uploadId = soapUploadToneData.UploadId + 1;
                    }
                    SoapUploadTone soapUploadToneTable = new SoapUploadTone();
                    soapUploadToneTable.UploadId = uploadId;
                    soapUploadToneTable.CreatedDateTime = DateTime.Now;
                    db.SoapUploadTone.Add(soapUploadToneTable);
                    db.SaveChanges();

                    string soapUIUrl = _configuration.GetValue<string>("AppSettings:MediaSoapUIUrl"];
                    var client = new RestClient(soapUIUrl);
                    var request = new RestRequest(Method.POST);

                    #region Old Soap API Call
                    //request.AddParameter("undefined", "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ton=\"http://toneprovide.ivas.huawei.com\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n   <soapenv:Header/>\r\n   <soapenv:Body>\r\n      <ton:uploadTone soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n         <event xsi:type=\"even:UploadToneEvt\" xmlns:even=\"http://event.toneprovide.ivas.huawei.com\">\r\n            " +
                    //    "<portalAccount>" + portalAccount + "</portalAccount>\r\n" +
                    //    "<portalPwd>" + portalPassword + "</portalPwd>\r\n" +
                    //    "<portalType>" + portalType + "</portalType>\r\n" +
                    //    "<role>" + role + "</role>\r\n" +
                    //    "<roleCode>" + roleCode + "</roleCode>\r\n" +
                    //    "<auditionFileNames>" + portalAccount + adName + "</auditionFileNames>\r\n" +
                    //    "<corpID>" + corpId + "</corpID>\r\n" +
                    //    "<language>0</language>\r\n" +
                    //    "<needApproved>0</needApproved>\t\t\t\r\n" +
                    //    "<price>0</price>\r\n" +
                    //    "<singerName>Ad</singerName>\r\n" +
                    //    "<singerNameLetter>A</singerNameLetter>\r\n" +
                    //    "<singerSex>1</singerSex>\r\n" +
                    //    "<toneInfo>ABCD</toneInfo>\t\r\n" +
                    //    "<toneName>Ad 1</toneName>\r\n" +
                    //    "<toneNameLetter>A</toneNameLetter>\t\r\n" +
                    //    "<toneValidDay>2020-11-11</toneValidDay>\t\t\r\n" +
                    //    "<uploadID>340023</uploadID>\t\t\r\n" +
                    //    "<uploadType>4</uploadType>\r\n" +
                    //    "</event>\r\n      </ton:uploadTone>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>", ParameterType.RequestBody);

                    #endregion

                    request.AddParameter("undefined", "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ton=\"http://toneprovide.ivas.huawei.com\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n  <soapenv:Header/>\r\n  <soapenv:Body>\r\n     <ton:uploadTone soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n        <event xsi:type=\"even:UploadToneEvt\" xmlns:even=\"http://event.toneprovide.ivas.huawei.com\">\r\n" +
                        "<portalAccount xsi:type=\"xsd:string\">" + portalAccount + "</portalAccount>\r\n" +
                        "<portalPwd xsi:type=\"xsd:string\">" + portalPassword + "</portalPwd>\r\n" +
                        "<portalType xsi:type=\"xsd:string\">" + portalType + "</portalType>\r\n" +
                        "<moduleCode xsi:type=\"xsd:string\">" + moduleCode + "</moduleCode>\r\n" +
                        "<role xsi:type=\"xsd:string\">" + role + "</role>\r\n" +
                        "<roleCode xsi:type=\"xsd:string\">" + roleCode + "</roleCode>\r\n" +
                        "<auditionFileNames soapenc:arrayType=\"xsd:string[2]\" xsi:type=\"soapenc:Array\">\r\n" +
                            "<auditionFileNames xsi:type=\"xsd:string\">web" + adName + "</auditionFileNames>\r\n" +
                            "<auditionFileNames xsi:type=\"xsd:string\">ivr" + secondAdname + ".wav</auditionFileNames>\r\n" +
                        "</auditionFileNames>\r\n" +
                        "<catalog xsi:type=\"xsd:string\"/>\r\n" +
                        "<corpID xsi:type=\"xsd:string\">" + corpId + "</corpID>\r\n" +
                        "<cutFlag xsi:type=\"xsd:string\">0</cutFlag>\r\n" +
                        "<language xsi:type=\"xsd:string\">4</language>\r\n" +
                        "<needApproved xsi:type=\"xsd:string\">0</needApproved>\r\n" +
                        "<price xsi:type=\"xsd:string\">0</price>\r\n" +
                        "<relativeTime xsi:type=\"xsd:string\">50</relativeTime>\r\n" +
                        "<singerName xsi:type=\"xsd:string\">Ad</singerName>\r\n" +
                        "<singerNameLetter xsi:type=\"xsd:string\">A</singerNameLetter>\r\n" +
                        "<singerSex xsi:type=\"xsd:string\">1</singerSex>\r\n" +
                        "<toneInfo xsi:type=\"xsd:string\">ABC</toneInfo>\r\n" +
                        "<toneName xsi:type=\"xsd:string\">ABC</toneName>\r\n" +
                        "<toneNameLetter xsi:type=\"xsd:string\">A</toneNameLetter>\r\n" +
                        "<toneValidDay xsi:type=\"xsd:string\">" + DateTime.Now.AddYears(4) + "</toneValidDay>\r\n" +
                        "<totalPrice xsi:type=\"xsd:string\">0</totalPrice>\r\n" +
                        "<uploadID xsi:type=\"xsd:string\">" + uploadId + "</uploadID>\r\n" +
                        "<uploadType xsi:type=\"xsd:string\">4</uploadType>\r\n" +
                        "</event>\r\n     </ton:uploadTone>\r\n  </soapenv:Body>\r\n" +
                        "</soapenv:Envelope>", ParameterType.RequestBody);



                    IRestResponse response = client.Execute(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseContent = response.Content;

                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(responseContent);
                        //XmlNodeList nodeList = xmldoc.GetElementsByTagName("uploadToneReturn");
                        XmlNodeList nodeList = xmldoc.GetElementsByTagName("multiRef");
                        if (nodeList.Count > 0)
                        {
                            foreach (XmlNode node in nodeList)
                            {
                                var returnCode = node.SelectSingleNode("returnCode").InnerXml;
                                if (returnCode == "000000")
                                {
                                    var toneID = node.SelectSingleNode("toneID").InnerXml;
                                    if (toneID != "?")
                                    {
                                        advertData.SoapToneId = toneID;
                                        db.SaveChanges();
                                    }
                                    var toneCode = node.SelectSingleNode("toneCode").InnerXml;
                                    if (toneCode != "?")
                                    {
                                        advertData.SoapToneCode = toneCode;
                                        db.SaveChanges();
                                    }
                                    // Success
                                }
                                return returnCode;
                            }
                        }

                    }
                    else
                    {
                        return response.StatusCode.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return "100001"; // "Unknown Error" 
            }
            return "100001"; // "Unknown Error" 
        }

        public static string DeleteToneSoapApi(int advertId)
        {
            try
            {
                EFMVCDataContex db = new EFMVCDataContex();
                var advertData = db.Adverts.Where(s => s.AdvertId == advertId).FirstOrDefault();

                if (!string.IsNullOrEmpty(advertData.MediaFileLocation))
                {
                    //var portalAccount = "adtones";
                    //var portalPassword = "TDD";
                    //var portalType = "1";
                    var role = "3";
                    var roleCode = "admin";
                    var toneID = advertData.SoapToneId;
                    var toneCode = advertData.SoapToneCode;
                    string soapUIUrl = _configuration.GetValue<string>("AppSettings:MediaSoapUIUrl"];
                    var client = new RestClient(soapUIUrl);

                    var request = new RestRequest(Method.POST);

                    #region Old DeleteTone
                    //request.AddParameter("undefined", "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ton=\"http://toneprovide.ivas.huawei.com\">\r\n   <soapenv:Header/>\r\n   <soapenv:Body>\r\n      <ton:delTone soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n         <event xsi:type=\"even:DelToneEvt\" xmlns:even=\"http://event.toneprovide.ivas.huawei.com\">\r\n" +
                    //    "<portalAccount>" + portalAccount + "</portalAccount>\r\n" +
                    //    "<portalPwd>" + portalPassword + "</portalPwd>\r\n" +
                    //    "<portalType>" + portalType + "</portalType>\r\n" +
                    //    "<role>" + role + "</role>\r\n" +
                    //    "<roleCode>" + roleCode + "</roleCode>\r\n" +
                    //    "<reason>asas</reason>\r\n" +
                    //    "<toneID>" + toneID + "</toneID>\r\n" +
                    //    "</event>\r\n      </ton:delTone>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>", ParameterType.RequestBody);
                    #endregion

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

        //0 = accepted
        //1 = rejected - MSISDN not recognised
        //2 = rejected - Advert ID not recognized
        //3 = Message ID is not unique
        //4 = Username / Password Combination is not recognised
        public static string TIBCOSubscribeUser(string phoneNumber)
        {
            try
            {
                if (!string.IsNullOrEmpty(phoneNumber))
                {

                    var client = new RestClient(TIBCOUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddParameter("undefined", "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:sch=\"https://adtones.com/schemas/Adtones_TIBCO/SharedResources/Schemas/Schema.xsd2\">\r\n<soapenv:Header/>\r\n   <soapenv:Body>\r\n" +
                        "<sch:SubscribeUnsubscribe>\r\n" +
                            "<sch:Header>\r\n" +
                                "<sch:Username>" + TIBCOUserName + "</sch:Username>\r\n" +
                                "<sch:Password>" + TIBCOPassword + "</sch:Password>\r\n" +
                                "<sch:SourceIP>?</sch:SourceIP>\r\n" +
                            "</sch:Header>\r\n" +
                            "<sch:SubscriberNo>" + phoneNumber + "</sch:SubscriberNo>\r\n" +
                            "<sch:Status>subscribe</sch:Status>\r\n" +
                            "<sch:MessageID>?</sch:MessageID>\r\n" +
                        "</sch:SubscribeUnsubscribe>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    var responseContent = response.Content;
                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(responseContent);
                        var serviceResponseNode = xmldoc.GetElementsByTagName("sch:ServiceResponse")[0];
                        var responseCode = serviceResponseNode.ChildNodes[0].InnerXml;
                        return responseCode;
                    }
                    return response.StatusCode.ToString();

                }
            }
            catch (Exception ex)
            {

            }
            return "1";
        }

        public static string TIBCOUnSubscribeUser(string phoneNumber)
        {
            try
            {
                if (!string.IsNullOrEmpty(phoneNumber))
                {

                    var client = new RestClient(TIBCOUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddParameter("undefined", "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:sch=\"https://adtones.com/schemas/Adtones_TIBCO/SharedResources/Schemas/Schema.xsd2\">\r\n<soapenv:Header/>\r\n   <soapenv:Body>\r\n" +
                        "<sch:SubscribeUnsubscribe>\r\n" +
                            "<sch:Header>\r\n" +
                                "<sch:Username>" + TIBCOUserName + "</sch:Username>\r\n" +
                                "<sch:Password>" + TIBCOPassword + "</sch:Password>\r\n" +
                                "<sch:SourceIP>?</sch:SourceIP>\r\n" +
                            "</sch:Header>\r\n" +
                            "<sch:SubscriberNo>" + phoneNumber + "</sch:SubscriberNo>\r\n" +
                            "<sch:Status>unsubscribe</sch:Status>\r\n" +
                            "<sch:MessageID>?</sch:MessageID>\r\n" +
                        "</sch:SubscribeUnsubscribe>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    var responseContent = response.Content;
                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(responseContent);
                        var serviceResponseNode = xmldoc.GetElementsByTagName("sch:ServiceResponse")[0];
                        var responseCode = serviceResponseNode.ChildNodes[0].InnerXml;
                        return responseCode;
                    }
                    return response.StatusCode.ToString();

                }
            }
            catch (Exception ex)
            {

            }
            return "1";
        }

        #region Safaricom API Call
        public static string AddSoapUser(string phoneNumber)
        {
            var url = safricomUrl + "AddUserFromWebsite?phoneNumber=" + phoneNumber;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content.Replace("\"", "");
        }

        public static string DeleteSoapUser(int userId, string phoneNumber)
        {
            var url = safricomUrl + "DeleteUserFromWebsite?userId=" + userId + "&phoneNumber=" + phoneNumber;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content.Replace("\"", "");
        }

        public static string UploadSoapTone(int advertId)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            //var advertData = db.Adverts.Where(s => s.AdvertId == advertId).FirstOrDefault();
            //var mediaFileLocation = advertData.MediaFileLocation;

            var url = safricomUrl + "UploadTone?advertId=" + advertId;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content.Replace("\"", "");
        }

        public static string DeleteSoapTone(int advertId)
        {
            var url = safricomUrl + "DeleteTone?advertId=" + advertId;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content.Replace("\"", "");
        }

        public static string UploadToneOnCRBTServer(int advertId)
        {
            var safariProjURL = _configuration.GetValue<string>("AppSettings:SafricomProjUrl"];
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