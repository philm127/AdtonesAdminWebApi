using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class AdvertCategoryResult
    {
        public int AdvertCategoryId { get; set; }
        public string CategoryName { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        public string CreatedDate { get; set; }
    }
}
