using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.TicketingModels
{
    public class AgentModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string avatar_url { get; set; }
        public string online_status { get; set; }
        public string status { get; set; }
        public string gender { get; set; }
    }
}
