using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class OperatorMaxAdvertsFormModel
    {
        public int OperatorMaxAdvertId { get; set; }

        public string KeyName { get; set; }

        public string KeyValue { get; set; }

        public DateTime Addeddate { get; set; }

        public Nullable<DateTime> Updateddate { get; set; }

        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
    }
}