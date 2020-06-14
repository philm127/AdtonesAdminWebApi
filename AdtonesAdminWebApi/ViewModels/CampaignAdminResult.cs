using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class CampaignAdminResult
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public int AdvertId { get; set; }
        public string AdvertName { get; set; }
        public int CampaignProfileId { get; set; }
        public string CampaignName { get; set; }

        public decimal TotalBudget { get; set; }
        public int finaltotalplays { get; set; }
        public decimal FundsAvailable { get; set; }
        public decimal AvgBidValue { get; set; }
        public decimal TotalSpend { get; set; }
        public int Status { get; set; }
        public string rStatus => $"{(Enums.CampaignStatus)Status}";
        public DateTime CreatedDate { get; set; }

        public int TicketCount { get; set; }
        public bool IsAdminApproval { get; set; }
        public int CountryId { get; set; }
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
        public string MobileNumber { get; set; }
        public string Organisation { get; set; }

    }
}