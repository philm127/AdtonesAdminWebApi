using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class AreaResult
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public bool IsActive { get; set; }
    }
}