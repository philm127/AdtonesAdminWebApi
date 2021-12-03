using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class AdvertiserCreditFormModel
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CurrencyId { get; set; }
        public int CountryId { get; set; }

        public decimal AssignCredit { get; set; }
    }

    public class UsersCreditFormModel : AdvertiserCreditFormModel
    {
        public decimal AvailableCredit { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}