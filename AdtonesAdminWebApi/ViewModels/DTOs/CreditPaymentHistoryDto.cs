using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.DTOs
{
    public class CreditPaymentHistoryDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
