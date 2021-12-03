using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    
    public class PromotionalCampaignBaseModel
    {
        public int ID { get; set; }
        public string BatchID { get; set; }
        public int OperatorId { get; set; }
        public int CountryId { get; set; }
        public IFormFile Files { get; set; }
    }

    public class PromotionalCampaignResult : PromotionalCampaignBaseModel
    {
        public string CampaignName { get; set; }
        public string OperatorName { get; set; }
        public int MaxDaily { get; set; }
        public int MaxWeekly { get; set; }
        public string AdvertName { get; set; }
        public string AdvertLocation { get; set; }
        public int Status { get; set; }
        public string rStatus => Status == 1 ? "Play" : "Stop";
        public int? AdtoneServerPromotionalCampaignId { get; set; }
    }


    public class PromotionalUserFormModel : PromotionalCampaignBaseModel
    {
        public string DeliveryServerConnectionString { get; set; }
        public string DeliveryServerIpAddress { get; set; }
    }

    public class PromotionalCampaignAdditionModel
    {
        public string BatchID { get; set; }
        public int OperatorId { get; set; }
        public IFormFile Files { get; set; }
        public string CampaignName { get; set; }
        public int MaxDaily { get; set; }
        public int MaxWeekly { get; set; }
        public string AdvertName { get; set; }
    }
}
