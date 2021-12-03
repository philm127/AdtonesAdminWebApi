using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class CampaignAuditOperatorTable
    {
        public int AuditId { get; set; }
        public double TotalCost { get; set; }
        public double PlayCost { get; set; }
        public double EmailCost { get; set; }
        public double SMSCost { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double PlayLength { get; set; }
        public int UserId { get; set; }
        public string CurrencyCode { get; set; }
        public string AdvertName { get; set; }
        public string EmailMsg { get; set; }
        public string SMS { get; set; }
        public string MSISDN { get; set; }
        public string DTMFKey { get; set; }
    }

    public class PromoCampaignPlaylist
    {
        public string MSISDN { get; set; }
        public string DTMFKey { get; set; }
        public double PlayLength { get; set; }
        public DateTime StartTime { get; set; }
    }
}
