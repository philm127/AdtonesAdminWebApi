using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels
{
    public class CreateOrUpdateCampaignProfileMobileCommand
    {
        public int CampaignProfileMobileId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the type of the contract.
        /// </summary>
        /// <value>The type of the contract.</value>
        public string ContractType_Mobile { get; set; }

        /// <summary>
        /// Gets or sets the spend.
        /// </summary>
        /// <value>The spend.</value>
        public string Spend_Mobile { get; set; }

        public bool NextStatus { get; set; }
    }
}
