using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class CountryResult
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string ShortName { get; set; }
        public string CountryCode { get; set; }
        public decimal TaxPercentage { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int Status { get; set; }
    }
}
