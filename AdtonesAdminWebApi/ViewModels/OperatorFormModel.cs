using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class OperatorFormModel
    {
        public int OperatorId { get; set; }

        public string OperatorName { get; set; }

        public int CountryId { get; set; }
        public bool IsActive { get; set; }

        public decimal EmailCost { get; set; }

        public decimal SmsCost { get; set; }

        public int CurrencyId { get; set; }
        public string  CountryName { get; set; }
        public int? AdtoneServerOperatorId { get; set; }

    }
}