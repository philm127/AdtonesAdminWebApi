using AdtonesAdminWebApi.TicketingModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{

    public interface ILiveAgentService
    {
        string CreateTicket(string subject, string message, string email);
        string ReplyTicket(string subject, string message, string email, string status, string agentMail, int roleId);
        string GetAgent();
        void DeleteTicket(string ticketCode);
    }


    public class LiveAgentService : ILiveAgentService
    {
        private readonly IConfiguration _configuration;
        static string url;
        static string key;
        static string liveAgentEmail;
        public LiveAgentService(IConfiguration configuration)
        {
            _configuration = configuration;
            url = _configuration.GetValue<string>("AppSettings:LiveAgentUrl");
            key = _configuration.GetValue<string>("AppSettings:LiveAgentKey");
            liveAgentEmail = _configuration.GetValue<string>("AppSettings:LiveAgentRecipient");
        }


        // status(string, optional): 
        //I - init
        //N - new
        //T - chatting
        //P - calling
        //R - resolved
        //X - deleted
        //B - spam
        //A - answered
        //C - open
        //W - postponed = ['I', 'N', 'T', 'P', 'R', 'X', 'B', 'A', 'C', 'W'],

        // 1. You can use "Retrieve list of departments" API to get ID of departments https://support.ladesk.com/840770-Complete-API-reference#7492dac92d066e148de4b67c72406f40

        //2. The error message "Visitor user can only create conversation addressed to application mail account." is pretty self-explanatory especially if you read the API documentation thoroughly:

        //useridentifier text    Ticket creator identifier - can be userid or email of an existing agent or visitor.It can not be email of a LiveAgent mail account.
        //recipient   text Recipient email.If useridentifier is visitor, recipient must be LiveAgent mail account. If useridentifier is agent, recipient must be visitor.

        //If useridentifier is email address which doesn't belong to any agent then it is a visitor/customer and visitor can create only tickets addressed to the mail accounts added in LiveAgent in Configuration->Email->Mail accounts.


        public string CreateTicket(string subject, string message, string email)
        {
            try
            {
                var param = new Ticket { subject = subject, departmentid = "default", message = message, recipient = liveAgentEmail, useridentifier = email, status = "N" };
                var fullUrl = url + "tickets";
                var client = new RestClient(fullUrl);
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("apikey", key);
                request.AddJsonBody(param);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var baseObj = JsonConvert.DeserializeObject<TicketModel>(response.Content);
                    TransferTicket(baseObj.code, "N");
                    return baseObj.code;
                }
                return response.StatusDescription;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        private void TransferTicket(string code, string status)
        {
            try
            {
                var agentid = "7872dcda"; // agent id of "ben@adtones.xyz"

                var agentList = GetAllAgent();
                if (agentList != null && agentList.Count() > 0)
                {
                    foreach (var item in agentList)
                    {
                        if (item.email == "adtones.test@gmail.com")
                        {
                            agentid = item.id;
                            break;
                        }
                    }
                }

                var fullUrl = url + "tickets/" + code;
                var param = new TicketUpdatable { agentid = agentid, status = status };
                var client = new RestClient(fullUrl);
                var request = new RestRequest(Method.PUT);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("apikey", key);
                request.AddJsonBody(param);
                IRestResponse response = client.Execute(request);


            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "LiveAgentService",
                    ProcedureName = "TransferTicket"
                };
                _logging.LogError();
            }
        }


        public string ReplyTicket(string subject, string message, string email, string status, string agentMail, int roleId)
        {
            try
            {

                Ticket param = new Ticket();
                if (roleId == (int)Enums.UserRole.Admin)
                    param = new Ticket { subject = subject, departmentid = "default", message = message, recipient = email, useridentifier = agentMail, status = status };
                else if (roleId == (int)Enums.UserRole.Advertiser || roleId == (int)Enums.UserRole.User)
                    param = new Ticket { subject = subject, departmentid = "default", message = message, recipient = liveAgentEmail, useridentifier = email, status = status };

                var fullUrl = url + "tickets";
                var client = new RestClient(fullUrl);
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("apikey", key);
                request.AddJsonBody(param);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var baseObj = JsonConvert.DeserializeObject<TicketModel>(response.Content);
                    TransferTicket(baseObj.code, status);
                    return baseObj.code;

                }
                return response.StatusDescription;

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "LiveAgentService",
                    ProcedureName = "ReplyTicket"
                };
                _logging.LogError();
            }
            return "error";
        }


        public string GetAgent()
        {
            try
            {

                var fullUrl = url + "agents/?_page=1&_perPage=10&_sortDir=ASC";
                var client = new RestClient(fullUrl);
                var request = new RestRequest(Method.GET);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("apikey", key);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var baseObj = JsonConvert.DeserializeObject<List<AgentModel>>(response.Content);
                    return baseObj.FirstOrDefault().email;
                }
                return response.StatusDescription;

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "LiveAgentService",
                    ProcedureName = "GetAgent"
                };
                _logging.LogError();
            }
            return "error";
        }


        public void DeleteTicket(string ticketCode)
        {
            try
            {

                var fullUrl = url + "tickets/" + ticketCode;
                var client = new RestClient(fullUrl);
                var request = new RestRequest(Method.DELETE);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("apikey", key);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "LiveAgentService",
                    ProcedureName = "DeleteTicket"
                };
                _logging.LogError();
            }
        }


        public static List<AgentModel> GetAllAgent()
        {
            try
            {
                var fullUrl = url + "agents/?_page=1&_perPage=10&_sortDir=ASC";
                var client = new RestClient(fullUrl);
                var request = new RestRequest(Method.GET);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("apikey", key);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<List<AgentModel>>(response.Content);

                }
                return null;

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "LiveAgentService",
                    ProcedureName = "GetAllAgent"
                };
                _logging.LogError();
                return null;
            }
        }


        public string GetLiveAgentEmail()
        {
            // c6939032 - Id of helpdesk@adtones.com
            var fullUrl = url + "mail_accounts/c6939032";
            var client = new RestClient(fullUrl);
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("apikey", key);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var baseObj = JsonConvert.DeserializeObject<List<TicketMailAccount>>(response.Content);
                return baseObj.LastOrDefault().email;
            }
            return response.StatusDescription;

        }
    
    }
}