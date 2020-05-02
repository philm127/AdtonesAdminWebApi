
using AdtonesAdminWebApi.Enums;
using AdtonesAdminWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class AdvertiserUserResult
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NoOfactivecampaign { get; set; }
        public int NoOfunapprovedadverts { get; set; }
        public double Creditlimit { get; set; }
        public double Outstandinginvoice { get; set; }
        public int Activated { get; set; }
        public string fstatus => $"{(UserStatus)Activated}";
        public System.DateTime DateCreated { get; set; }
        public int TicketCount { get; set; }
        public string Role => $"{(UserRole)RoleId}";


    }
}