using AdtonesAdminWebApi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Model
{
    public class UsersModelXXX
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public int status { get; set; }
        public string fstatus => $"{(UserStatus)status}";
        public DateTime? DateCreated { get; set; }
        public string Role => $"{(UserRole)RoleId}";
        public string Organisation { get; set; }
        public int Outstandingdays { get; set; }
    }
}
