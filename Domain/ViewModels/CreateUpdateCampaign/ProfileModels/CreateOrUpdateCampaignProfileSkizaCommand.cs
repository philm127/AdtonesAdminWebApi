using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels.CreateUpdateCampaign.ProfileModels
{
    public class CreateOrUpdateCampaignProfileSkizaCommand
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


        public List<QuestionOptionModel> HustlersQuestion { get; set; }

        // [Display(Name = "Hustlers")]
        public string Hustlers_AdType { get; set; }



        // [Display(Name = "Youth")]
        public List<QuestionOptionModel> YouthQuestion { get; set; }

        // [Display(Name = "Youth")]
        public string Youth_AdType { get; set; }


        // [Display(Name = "Discerning Professionals")]
        public List<QuestionOptionModel> DiscerningProfessionalsQuestion { get; set; }

        // [Display(Name = "Discerning Professionals")]
        public string DiscerningProfessionals_AdType { get; set; }


        // [Display(Name = "Mass")]
        public List<QuestionOptionModel> MassQuestion { get; set; }

        // [Display(Name = "Mass")]
        public string Mass_AdType { get; set; }
    }
}
