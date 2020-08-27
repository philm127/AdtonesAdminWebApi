using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.OperatorSpecific
{
    public class ProvisionModel
    {
        public string isdn { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string code { get; set; }
    }

    public class ProvisionModelRequest
    {
        public string isdn { get; set; }
        public bool provision { get; set; }
    }

    public class ExpressoOperator
    {
        public const string CodeSuccess = "0001";
        public const string CodeNothingChanged = "0002";

        public static bool IsSuccess(ProvisionModel response) => (response?.success ?? false) && response.code == CodeSuccess;

        public static async Task<List<ProvisionModel>> ExpressoProvisionSingle(ProvisionModelRequest request)
        {
            return await ExpressoProvisionBatch(new List<ProvisionModelRequest> { request });
        }


        public static async Task<List<ProvisionModel>> TESTExpressoProvisionBatch(IEnumerable<ProvisionModelRequest> requestBatch)
        {
            try
            {
                
                var response = new List<ProvisionModel>();
                foreach(var rBatch in requestBatch)
                {
                    var rsBatch = new ProvisionModel();
                    rsBatch.code = "200";
                    rsBatch.isdn = rBatch.isdn;
                    rsBatch.success = true;
                    response.Add(rsBatch);
                }

                return response;//.Data;
            }
            catch (Exception e)
            {
                throw new ExpressoException(e.Message, e);
            }
        }

        public static async Task<List<ProvisionModel>> ExpressoProvisionBatch(IEnumerable<ProvisionModelRequest> requestBatch)
        {
            try
            {
                var provisionURL = ConfigurationManager.AppSettings["ExpressoProvisionUrl"];
                var client = new RestClient(provisionURL);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(requestBatch.ToList());
                var response = await client.ExecutePostAsync<List<ProvisionModel>>(request);
                if (response.ErrorException != null || !string.IsNullOrEmpty(response.ErrorMessage))
                    throw new ExpressoException(response.ErrorMessage, response.ErrorException);

                return response.Data;
            }
            catch (Exception e)
            {
                throw new ExpressoException(e.Message, e);
            }
        }

        public static List<ProvisionModel> ExpressoProvision(string phoneNumber, string provision)
        {
            var provisionURL = ConfigurationManager.AppSettings["ExpressoProvisionUrl"];
            var client = new RestClient(provisionURL);
            var request = new RestRequest(Method.POST);
            //request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            //request.AddHeader("Content-Length", "55");
            //request.AddHeader("Accept-Encoding", "gzip, deflate");
            //request.AddHeader("Host", "10.71.48.156:7778");
            // request.AddHeader("Postman-Token", "1b26503a-ba66-41bc-addb-76502fb634ee,8cbddf5e-3918-495a-b5ed-6ca55ba99bfa");
            request.AddHeader("Cache-Control", "no-cache");
            //request.AddHeader("Accept", "*/*");
            //request.AddHeader("User-Agent", "PostmanRuntime/7.17.1");
            request.AddHeader("Content-Type", "application/json");
            //request.AddParameter("undefined", "[\r\n    {\"isdn\": \"221706077071\", \"provision\": false }\r\n]", ParameterType.RequestBody);
            request.AddParameter("undefined", "[\r\n    {\"isdn\": \"" + phoneNumber + "\", \"provision\":" + provision + "}\r\n]", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (!string.IsNullOrEmpty(response.Content))
            {
                var baseObj = JsonConvert.DeserializeObject<List<ProvisionModel>>(response.Content);
                if (baseObj.Count() > 0)
                {
                    return baseObj;
                }
                return baseObj;
            }
            else
            {
                return new List<ProvisionModel>();
            }
        }

        [Serializable]
        public class ExpressoException : Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public ExpressoException()
            {
            }

            public ExpressoException(string message) : base(message)
            {
            }

            public ExpressoException(string message, Exception inner) : base(message, inner)
            {
            }

            protected ExpressoException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }
    }
}