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
        public DateTime CreatedDate { get; set; }
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
        public string CountryName { get; set; }
        public int? AdtoneServerOperatorConfigurationId { get; set; }
    }


    public class OperatorFormModel
    {
        public int OperatorId { get; set; }

        public string OperatorName { get; set; }

        public int CountryId { get; set; }
        public bool IsActive { get; set; }

        public decimal EmailCost { get; set; }

        public decimal SmsCost { get; set; }

        public int CurrencyId { get; set; }
        public string CountryName { get; set; }
        public int? AdtoneServerOperatorId { get; set; }

    }

    public class OperatorMaxAdvertsFormModel
    {
        public int OperatorMaxAdvertId { get; set; }

        public string KeyName { get; set; }

        public string KeyValue { get; set; }

        public DateTime CreatedDate { get; set; }

        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
        public int? AdtoneServerOperatorMaxAdvertId { get; set; }
    }

    public class OperatorResult
    {
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        public string IsActive { get; set; }
        public decimal EmailCost { get; set; }
        public decimal SmsCost { get; set; }
        public string Currency { get; set; }
        public int CurrencyId { get; set; }
    }
}
