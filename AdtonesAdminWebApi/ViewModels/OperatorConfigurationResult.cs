using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class OperatorConfigurationResult
    {
        public int OperatorConfigurationId { get; set; }
        public int Days { get; set; }
        public bool IsActive { get; set; }
        public string Addeddate { get; set; }
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
    }
}
