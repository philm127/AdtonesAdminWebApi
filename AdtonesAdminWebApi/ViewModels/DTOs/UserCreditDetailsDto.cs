using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.DTOs
{
    public class UserCreditDetailsDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; }

        public int CurrencyId { get; set; }
        public int CountryId { get; set; }

        public decimal AssignCredit { get; set; }
        public decimal AvailableCredit { get; set; }

        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Amount { get; set; }

        public List<CreditPaymentHistoryDto> PaymentHistory { get; set; }
    }
}
