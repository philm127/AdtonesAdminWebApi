using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Model
{
    public class Currency
    {
        public int CurrencyId { get; set; }

        public string CurrencyCode { get; set; }

        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
