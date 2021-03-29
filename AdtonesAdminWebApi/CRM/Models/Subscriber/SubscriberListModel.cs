using System;
using System.Collections.Generic;
using System.Text;

namespace AdtonesAdminWebApi.CRM.Models.Subscriber
{
    public class SubscriberListModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // CountryName
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string OperatorName { get; set; }
        public int OperatorId { get; set; }

        public System.DateTime DateCreated { get; set; }

        public string Organisation { get; set; }
        public int Activated { get; set; }
        
        public bool VerificationStatus { get; set; }

        public int? OrganisationTypeId { get; set; }

        public string MSISDN { get; set; }

    }
}
