using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserProfile.Models
{
    public class CampaignProfileCinemaFormModel : QuestionOptionsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileCinemaFormModel"/> class.
        /// </summary>
        public CampaignProfileCinemaFormModel()
        {
            CinemaQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
        }

        /// <summary>
        /// Gets or sets the campaign profile cinema identifier.
        /// </summary>
        /// <value>The campaign profile cinema identifier.</value>
        public int CampaignProfileCinemaId { get; set; }

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
        /// Gets or sets the cinema question.
        /// </summary>
        /// <value>The cinema question.</value>
        // [Display(Name = "Cinema")]
        public List<QuestionOptionModel> CinemaQuestion { get; set; }

        /// <summary>
        /// Gets or sets the cinema.
        /// </summary>
        /// <value>The cinema.</value>
        // [Display(Name = "Cinema")]
        public string Cinema_Cinema
        {
            get
            {
                if (CinemaQuestion == null)
                    CinemaQuestion = new List<QuestionOptionModel>();

                return CompileAnswers(SortList(CinemaQuestion));
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                // remove the default selection
                foreach (QuestionOptionModel questionOptionModel in CinemaQuestion)
                    questionOptionModel.Selected = false;

                // select the values that are present in the database
                for (int i = 0; i < value.Length; i++)
                    CinemaQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }
    }
}