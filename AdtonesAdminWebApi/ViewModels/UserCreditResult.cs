using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class UserCreditResult
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
    }
}
