using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class CountryFormModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string CountryCode { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public decimal TaxPercentage { get; set; }

        public int Status { get; set; }

        // [Required(ErrorMessage = "The Term & Condition field is required.")]
        public string TermAndConditionFileName { get; set; }

        public IFormFile file { get; set; }
    }
}
