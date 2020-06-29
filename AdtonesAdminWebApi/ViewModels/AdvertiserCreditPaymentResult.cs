using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class AdvertiserCreditPaymentResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }

        public string Organisation { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public int CampaignProfileId { get; set; }
        public string CampaignName { get; set; }
        public decimal AssignCredit { get; set; }
        public decimal AvailableCredit { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }

    }
}