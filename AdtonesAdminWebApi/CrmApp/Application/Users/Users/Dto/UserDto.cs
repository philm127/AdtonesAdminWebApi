using AdtonesAdminWebApi.CrmApp.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.CrmApp.Application.Users.Users.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }

        public int OperatorId { get; set; }
        public string OperatorName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public string PasswordHash { get; set; }
        public string Token { get; set; }
        public System.DateTime DateCreated { get; set; }

        public string Organisation { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public int Status { get; set; }
        public string rStatus => $"{(UserStatus)Status}";
        public int RoleId { get; set; }
        public string Role => $"{(UserRole)RoleId}";
        public bool VerificationStatus { get; set; }

        public string Permissions { get; set; }

        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public int Outstandingdays { get; set; }

        public int? OrganisationTypeId { get; set; }

        public int? AdtoneServerUserId { get; set; }

        public bool IsSessionFlag { get; set; }
        public Nullable<System.DateTime> LockOutTime { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        // How many times user has tried to login.
        public int cntAttemps { get; set; }
    }
}
