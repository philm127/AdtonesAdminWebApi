using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class AdvertCategoryResult
    {
        public int AdvertCategoryId { get; set; }
        public string CategoryName { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        public string CreatedDate { get; set; }

        public int? AdtoneServerAdvertCategoryId { get; set; }
    }


    public class CampaignCategoryResult
    {
        public int CampaignCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public bool Active { get; set; }

        public int? CountryId { get; set; }

        public int? AdtoneServerCampaignCategoryId { get; set; }
    }

    public class CampaignAdverts
    {
        public int CampaignAdvertId { get; set; }
        public int CampaignProfileId { get; set; }
        public int AdvertId { get; set; }
        public bool NextStatus { get; set; }
        public int AdtoneServerCampaignAdvertId { get; set; }
    }
}
