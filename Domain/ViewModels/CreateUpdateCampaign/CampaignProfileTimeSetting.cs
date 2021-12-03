using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels.CreateUpdateCampaign
{
    public class CampaignProfileTimeSetting
    {
        public int CampaignProfileTimeSettingsId { get; set; }
        public int CampaignProfileId { get; set; }
        public CampaignProfile CampaignProfile { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
        public int? AdtoneServerCampaignProfileTimeId { get; set; }
    }
}
