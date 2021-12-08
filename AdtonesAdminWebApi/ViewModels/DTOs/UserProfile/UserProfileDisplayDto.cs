using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.DTOs.UserProfile
{
    public class UserProfileDisplayDto
    {
        public SkizaProfileDto SkizaProfileDto { get; set; }
        public UserProfileDemographicsDto UserProfileDemographicsDto { get; set; }
    }

    public class UserProfileDemographicsDto
    {
        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>The age.</value>
        public string Age_Demographics { get; set; }
        public string Age { get; set; }
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        public string Gender_Demographics { get; set; }

        /// <summary>
        /// Gets or sets the income bracket.
        /// </summary>
        /// <value>The income bracket.</value>
        public string IncomeBracket_Demographics { get; set; }

        /// <summary>
        /// Gets or sets the working status.
        /// </summary>
        /// <value>The working status.</value>
        public string WorkingStatus_Demographics { get; set; }


        /// <summary>
        /// Gets or sets the education.
        /// </summary>
        /// <value>The education.</value>
        public string Education_Demographics { get; set; }

        /// <summary>
        /// Gets or sets the household status.
        /// </summary>
        /// <value>The household status.</value>
        public string HouseholdStatus_Demographics { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public string Location_Demographics { get; set; }

    }


    public class SkizaProfileDto
    {
        public string Hustlers_AdType { get; set; }
        public string Mass_AdType { get; set; }
        public string Youth_AdType { get; set; }
        public string DiscerningProfessionals_AdType { get; set; }
        public int CountryId { get; set; }
    }
}
