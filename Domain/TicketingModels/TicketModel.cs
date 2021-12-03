using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.TicketingModels
{
    public class TicketModel
    {
        public string id { get; set; }
        public string owner_contactid { get; set; }
        public string departmentid { get; set; }
        public string status { get; set; }
        public string code { get; set; }
        public string date_created { get; set; }
        public string public_access_urlcode { get; set; }
        public string subject { get; set; }
        public List<object> custom_fields { get; set; }
    }

    public class TicketUpdatable
    {
        public string agentid { get; set; }
        public List<CustomField> custom_fields { get; set; }
        public string departmentid { get; set; }
        public string owner_contactid { get; set; }
        public string status { get; set; }
        public List<string> tags { get; set; }
    }

    public class CustomField
    {
        public string code { get; set; }
        public string value { get; set; }
    }
}
