using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels
{
    public class CreateOrUpdateCampaignProfileGeographicCommand
    {
        public int CampaignProfileGeographicId { get; set; }
        public int CampaignProfileId { get; set; }
        public string PostCode { get; set; }
        public int CountryId { get; set; }
        public string Location_Demographics { get; set; }
    }
}
