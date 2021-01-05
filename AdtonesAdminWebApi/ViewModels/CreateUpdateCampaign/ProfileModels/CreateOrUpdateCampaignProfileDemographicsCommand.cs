using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels
{
    public class CreateOrUpdateCampaignProfileDemographicsCommand
    {
        /// <summary>
        /// Gets or sets the campaign profile demographics identifier.
        /// </summary>
        /// <value>The campaign profile demographics identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the dob start.
        /// </summary>
        /// <value>The dob start.</value>
        public DateTime? DOBStart_Demographics { get; set; }

        /// <summary>
        /// Gets or sets the dob end.
        /// </summary>
        /// <value>The dob end.</value>
        public DateTime? DOBEnd_Demographics { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>The age.</value>
        public string Age_Demographics { get; set; }

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
        /// Gets or sets the relationship status.
        /// </summary>
        /// <value>The relationship status.</value>
        public string RelationshipStatus_Demographics { get; set; }

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

        public bool NextStatus { get; set; }
    }
}
