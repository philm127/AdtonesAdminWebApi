using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.TicketingModels
{
    public class TicketMailAccount
    {
        public string mailaccount_id { get; set; }
        public string fetch_type { get; set; }
        public string email { get; set; }
        public string department_id { get; set; }
        public string status { get; set; }
        public string provider { get; set; }
        public string last_mail_date { get; set; }
        public string last_fetch_date { get; set; }
    }
}
