using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Model
{
    public class Contacts
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string MobileNumber { get; set; }

        public string FixedLine { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public int? CountryId { get; set; }

        public int? CurrencyId { get; set; }

        public int? AdtoneServerContactId { get; set; }

        public int? RoleId { get; set; }
    }
}