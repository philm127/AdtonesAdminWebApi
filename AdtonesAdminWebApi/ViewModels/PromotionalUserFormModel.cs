using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class PromotionalUserFormModel
    {
        public int ID { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public int OperatorId { get; set; }

        public int BatchID { get; set; }

        public string DeliveryServerConnectionString { get; set; }

        public string DeliveryServerIpAddress { get; set; }

        public IFormFile Files { get; set; }
    }
}