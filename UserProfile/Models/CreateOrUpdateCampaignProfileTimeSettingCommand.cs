using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserProfile.Models
{
    public class CreateOrUpdateCampaignProfileTimeSettingCommand
    {
        /// <summary>
        /// Gets or sets the campaign profile time settings identifier.
        /// </summary>
        /// <value>The campaign profile time settings identifier.</value>
        public int CampaignProfileTimeSettingsId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the monday posted times.
        /// </summary>
        /// <value>The monday posted times.</value>
        public PostedTimes MondayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the tuesday posted times.
        /// </summary>
        /// <value>The tuesday posted times.</value>
        public PostedTimes TuesdayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the wednesday posted times.
        /// </summary>
        /// <value>The wednesday posted times.</value>
        public PostedTimes WednesdayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the thursday posted times.
        /// </summary>
        /// <value>The thursday posted times.</value>
        public PostedTimes ThursdayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the friday posted times.
        /// </summary>
        /// <value>The friday posted times.</value>
        public PostedTimes FridayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the saturday posted times.
        /// </summary>
        /// <value>The saturday posted times.</value>
        public PostedTimes SaturdayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the sunday posted times.
        /// </summary>
        /// <value>The sunday posted times.</value>
        public PostedTimes SundayPostedTimes { get; set; }
    }


    public class PostedTimes
    {
        /// <summary>
        /// Gets or sets the day ids.
        /// </summary>
        /// <value>The day ids.</value>
        public string[] DayIds { get; set; }
    }
}