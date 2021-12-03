using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.TicketingModels
{
    public class Ticket
    {
        public string subject { get; set; }
        public string departmentid { get; set; }
        public string message { get; set; }
        public string recipient { get; set; }
        public string useridentifier { get; set; }
        public string status { get; set; }
    }
}
