using Domain.ViewModels.CreateUpdateCampaign.ProfileModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels.CreateUpdateCampaign
{
    public class CampaignAuditFormModel
    {
        /// <summary>
        /// Gets or sets the campaign audit identifier.
        /// </summary>
        /// <value>The campaign audit identifier.</value>
        public int CampaignAuditId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile.
        /// </summary>
        /// <value>The campaign profile.</value>
        public CampaignProfileFormModel CampaignProfile { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the bid value.
        /// </summary>
        /// <value>The bid value.</value>
        // [Display(Name = "Bid Value")]
        public double BidValue { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>The start time.</value>
        // [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>The end time.</value>
        // [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the play length ticks.
        /// </summary>
        /// <value>The play length ticks.</value>
        // [Display(Name = "Play Length Ticks")]
        public Int64 PlayLengthTicks { get; set; }

        /// <summary>
        /// Gets or sets the length of the play.
        /// </summary>
        /// <value>The length of the play.</value>
        // [Display(Name = "Play Length")]
        public TimeSpan PlayLength
        {
            get { return TimeSpan.FromMilliseconds(PlayLengthTicks); }
            set { PlayLengthTicks = value.Ticks; }
        }

        /// <summary>
        /// Gets or sets the SMS.
        /// </summary>
        /// <value>The SMS.</value>
        public string SMS { get; set; }

        /// <summary>
        /// Gets or sets the SMS cost.
        /// </summary>
        /// <value>The SMS cost.</value>
        // [Display(Name = "SMS Cost")]
        public double SMSCost { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the email cost.
        /// </summary>
        /// <value>The email cost.</value>
        // [Display(Name = "Email Cost")]
        public double EmailCost { get; set; }

        /// <summary>
        /// Gets or sets the total cost.
        /// </summary>
        /// <value>The total cost.</value>
        // [Display(Name = "Total Cost")]
        public double TotalCost { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }

        /// <summary>
        /// Gets the addeddate.
        /// </summary>
        /// <value>The addeddate.</value>
        public DateTime? AddedDate { get; set; }
    }
}
