using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class PromotionalCampaignResult
    {
        public int ID { get; set; }
        public int? OperatorID { get; set; }
        public int? CountryID { get; set; }
        public string CampaignName { get; set; }
        public string OperatorName { get; set; }
        public int BatchID { get; set; }
        public int MaxDaily { get; set; }
        public int MaxWeekly { get; set; }
        public string AdvertName { get; set; }
        public string AdvertLocation { get; set; }
        public int Status { get; set; }
        public string rStatus { get; set; }
        public IFormFile Files { get; set; }
    }
}
