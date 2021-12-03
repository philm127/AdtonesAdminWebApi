using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.DTOs.UserProfile
{
    public class UserProfileDto
    {
        public int UserProfileId { get; set; }
        public int UserId { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string IncomeBracket { get; set; }
        public string WorkingStatus { get; set; }
        public string RelationshipStatus { get; set; }
        public string Education { get; set; }
        public string HouseholdStatus { get; set; }
        public string Location { get; set; }
        public string MSISDN { get; set; }
        public int AdtoneServerUserProfileId { get; set; }

        public UserProfilePreferenceDto UserProfilePreferences { get; set; }

    }
}
