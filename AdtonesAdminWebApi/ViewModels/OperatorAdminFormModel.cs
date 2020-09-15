using AdtonesAdminWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class OperatorAdminFormModel
    {
        #region Contacts Data

        public int Id { get; set; }
        public int UserId { get; set; }

        public int ContactUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Organisation { get; set; }
        public string Email { get; set; }

        public int CountryId { get; set; }
        public int? OperatorId { get; set; }
        public int RoleId { get; set; }

        public string MobileNumber { get; set; }

        public string FixedLine { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public int? CurrencyId { get; set; }

        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string Permissions { get; set; }

        public int Activated { get; set; } = 1;

        #endregion
    }


    public class UserAddFormModel
    {
        #region Contacts Data

        public int Id { get; set; }
        public int UserId { get; set; }

        public int ContactUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Organisation { get; set; }
        public string Email { get; set; }

        public int CountryId { get; set; }
        public int? OperatorId { get; set; }
        public int RoleId { get; set; }

        public string MobileNumber { get; set; }

        public string FixedLine { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public int? CurrencyId { get; set; }

        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string Permissions { get; set; }

        public int Activated { get; set; } = 1;

        public bool VerificationStatus { get; set; }

        public int Outstandingdays { get; set; }

        public bool IsMsisdnMatch { get; set; }
        public bool IsEmailVerfication { get; set; }
        public string PhoneticAlphabet { get; set; }
        public bool IsMobileVerfication { get; set; }

        public int? OrganisationTypeId { get; set; }

        public string UserMatchTableName { get; set; }

        public int AdtoneServerUserId { get; set; }

        #endregion
    }
}
