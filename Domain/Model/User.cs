using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public int OperatorId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public string PasswordHash { get; set; }
        public string Token { get; set; }
        public System.DateTime DateCreated { get; set; }

        public string Organisation { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public int Activated { get; set; }
        public string rStatus => $"{(UserStatus)Activated}";
        public int RoleId { get; set; }
        public string Role => $"{(UserRole)RoleId}";
        public bool VerificationStatus { get; set; }

        public string Permissions { get; set; }

        public int CountryId { get; set; }

        public int Outstandingdays { get; set; }
        public bool IsMsisdnMatch { get; set; }
        public bool IsEmailVerfication { get; set; }
        public string PhoneticAlphabet { get; set; }
        public bool IsMobileVerfication { get; set; }

        public int? OrganisationTypeId { get; set; }

        public string UserMatchTableName { get; set; }

        public int? AdtoneServerUserId { get; set; }
        public string TibcoMessageId { get; set; }

        public bool IsSessionFlag { get; set; }
        public Nullable<System.DateTime> LockOutTime { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        // How many times user has tried to login.
        public int cntAttemps { get; set; }

    }
}
