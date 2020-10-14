using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class AdvertiserCreditFormModel
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CurrencyId { get; set; }
        public int CountryId { get; set; }

        public decimal AssignCredit { get; set; }
        public decimal AvailableCredit { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int BillingId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int CampaignProfileId { get; set; }
        public int Status { get; set; } = 0;
    }


    public class AdvertiserCreditDetailModel
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CurrencyId { get; set; }
        public int CountryId { get; set; }

        public decimal AssignCredit { get; set; }
        public decimal AvailableCredit { get; set; }

        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Amount { get; set; }

        public List<CreditPaymentHistory> PaymentHistory { get; set; }
    }


    public class CreditPaymentHistory
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }


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


    public class AdvertiserCreditResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Email { get; set; }
        public string FullName { get; set; }

        public string Organisation { get; set; }
        public decimal Credit { get; set; }
        public decimal AvailableCredit { get; set; }
        public decimal TotalUsed { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RemainingAmount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
        public string SalesExec { get; set; }
        public int SUserId { get; set; }
    }


    public class AdvertiserCreditSharedListsModel
    {
        public IEnumerable<SharedSelectListViewModel> users { get; set; }
        public IEnumerable<SharedSelectListViewModel> country { get; set; }
        public IEnumerable<SharedSelectListViewModel> currency { get; set; }
        public IEnumerable<SharedSelectListViewModel> addUser { get; set; }
    }


    public class CampaignCreditResult
    {
        public int? CampaignCreditPeriodId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int CampaignProfileId { get; set; }
        public string CampaignName { get; set; }
        public int CreditPeriod { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? AdtoneServerCampaignCreditPeriodId { get; set; }
        public string SalesExec { get; set; }
    }


}
