using AdtonesAdminWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class OperatorAdminFormModel : User
    {
        #region Contacts Data

        public int Id { get; set; }

        public int ContactUserId { get; set; }

        public string MobileNumber { get; set; }

        public string FixedLine { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public int CountryId { get; set; }

        public int? CurrencyId { get; set; }

        #endregion
    }
}
