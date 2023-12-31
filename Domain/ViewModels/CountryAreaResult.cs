﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    
    public class CountryResult
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string ShortName { get; set; }
        public string CountryCode { get; set; }
        public decimal TaxPercentage { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int Status { get; set; }

        public int? UserId { get; set; }
        // [Required(ErrorMessage = "The Term & Condition field is required.")]
        public string TermAndConditionFileName { get; set; }
        public IFormFile file { get; set; }
        public decimal MinBid { get; set; }

        public int? AdtoneServeCountryId { get; set; }
    }


    public class AreaResult
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public bool IsActive { get; set; }
    }


}
