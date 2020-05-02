using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class UserCreditFormModel
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CurrencyId { get; set; }

        public decimal AssignCredit { get; set; }
    }

    public class UsersCreditFormModel : UserCreditFormModel
    {
        public decimal AvailableCredit { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}