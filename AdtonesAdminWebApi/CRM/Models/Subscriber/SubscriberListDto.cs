using AdtonesAdminWebApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.CRM.Models.Subscriber
{
    public class SubscriberListDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string CountryName { get; set; }
        public int CountryId { get; set; }
        public string OperatorName { get; set; }
        public int OperatorId { get; set; }
        public System.DateTime DateCreated { get; set; }

        public string Organisation { get; set; }
        public int Status { get; set; }

        public bool VerificationStatus { get; set; }

        public string Permissions { get; set; }

        public int? OrganisationTypeId { get; set; }

        public string rStatus => $"{(UserStatus)Status}";
    }
}
