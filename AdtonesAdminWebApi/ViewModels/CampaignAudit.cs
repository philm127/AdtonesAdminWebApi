using System;
using AdtonesAdminWebApi.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class CampaignAudit
    {
        public int CampaignAuditId { get; set; }
        public CampaignProfile CampaignProfile { get; set; }
        public int CampaignProfileId { get; set; }
        public int UserProfileId { get; set; }
        public double BidValue { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Int64 PlayLengthTicks { get; set; }
        public string SMS { get; set; }
        public double SMSCost { get; set; }
        public string Email { get; set; }
        public double EmailCost { get; set; }
        public double TotalCost { get; set; }
        public string Status { get; set; }
        public bool Proceed { get; set; }
        public DateTime? AddedDate { get; set; }
        public bool IsMerticsUpdated { get; set; }
        public string CampaignAuditTableName { get; set; }
        public int? AdtoneServerCampaignProfileId { get; set; }
        public int? AdtoneServerUserProfileId { get; set; }
        public bool AdtoneServerDataTransfer { get; set; }
    }
}

