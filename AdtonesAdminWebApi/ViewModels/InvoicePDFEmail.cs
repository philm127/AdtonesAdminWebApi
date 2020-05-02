using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class InvoicePDFEmailModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime SettledDate { get; set; }
        public int CountryId { get; set; }
        public decimal Amount { get; set; }
    }
}
