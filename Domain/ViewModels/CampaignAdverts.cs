using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class CampaignAdverts
    {
        public int CampaignAdvertId { get; set; }
        public int CampaignProfileId { get; set; }
        public int AdvertId { get; set; }
        public bool NextStatus { get; set; }
        public int AdtoneServerCampaignAdvertId { get; set; }
    }
}
