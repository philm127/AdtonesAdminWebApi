using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserProfile.Models
{
    public class CampaignProfileDateSettingsFormModel
    {
        /// <summary>
        /// Gets or sets the campaign date settings identifier.
        /// </summary>
        /// <value>The campaign date settings identifier.</value>
        public int CampaignDateSettingsId { get; set; }

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
        /// Gets or sets the campaign date.
        /// </summary>
        /// <value>The campaign date.</value>
        public DateTime CampaignDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CampaignProfileDateSettingsFormModel"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }
    }
}