using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class CompanyDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string AdditionalAddress { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        public int CountryId { get; set; }
        public int? AdtoneServerCompanyDetailId { get; set; }
    }
}