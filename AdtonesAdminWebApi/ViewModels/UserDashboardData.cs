
using AdtonesAdminWebApi.Enums;
using AdtonesAdminWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    
    public class UserDashboardData
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Activated { get; set; }
        public string rStatus => $"{(UserStatus)Activated}";
        public System.DateTime DateCreated { get; set; }
        public string Role => $"{(UserRole)RoleId}";
    }

    public class AdminDashboardResult : UserDashboardData
    {

        public string MobileNumber { get; set; }

        public string CountryName { get; set; }

        public string Organisation { get; set; }


    }

    public class AdvertiserDashboardResult : UserDashboardData
    {
        
        public int NoOfactivecampaign { get; set; }
        public int NoOfunapprovedadverts { get; set; }
        public double Creditlimit { get; set; }
        public double Outstandinginvoice { get; set; }
        public string MobileNumber { get; set; }

        public string CountryName { get; set; }

        public int TicketCount { get; set; }
        
    }


    public class OperatorDashboardResult : UserDashboardData
    {
        public string Organisation { get; set; }

        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
    }

    public class SubscriberDashboardResult : UserDashboardData
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
        public string MSISDN { get; set; }
    }
}