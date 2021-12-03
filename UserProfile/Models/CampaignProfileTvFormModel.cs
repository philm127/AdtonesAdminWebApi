using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserProfile.Models
{
    public class CampaignProfileTvFormModel : QuestionOptionsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileTvFormModel"/> class.
        /// </summary>
        public CampaignProfileTvFormModel()
        {
            SatalliteQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            CableQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            TerrestrialQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            InternetQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
        }

        /// <summary>
        /// Gets or sets the campaign profile tv identifier.
        /// </summary>
        /// <value>The campaign profile tv identifier.</value>
        public int CampaignProfileTvId { get; set; }

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
        /// Gets or sets the satallite question.
        /// </summary>
        /// <value>The satallite question.</value>
        // [Display(Name = "Satellite")]
        public List<QuestionOptionModel> SatalliteQuestion { get; set; }

        /// <summary>
        /// Gets or sets the satallite.
        /// </summary>
        /// <value>The satallite.</value>
        // [Display(Name = "Satellite")]
        public string Satallite_TV
        {
            get
            {
                if (SatalliteQuestion == null)
                    SatalliteQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(SatalliteQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    SatalliteQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the cable question.
        /// </summary>
        /// <value>The cable question.</value>
        // [Display(Name = "Cable")]
        public List<QuestionOptionModel> CableQuestion { get; set; }

        /// <summary>
        /// Gets or sets the cable.
        /// </summary>
        /// <value>The cable.</value>
        // [Display(Name = "Cable")]
        public string Cable_TV
        {
            get
            {
                if (CableQuestion == null)
                    CableQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(CableQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    CableQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the terrestrial question.
        /// </summary>
        /// <value>The terrestrial question.</value>
        // [Display(Name = "Terrestrial")]
        public List<QuestionOptionModel> TerrestrialQuestion { get; set; }

        /// <summary>
        /// Gets or sets the terrestrial.
        /// </summary>
        /// <value>The terrestrial.</value>
        // [Display(Name = "Terrestrial")]
        public string Terrestrial_TV
        {
            get
            {
                if (TerrestrialQuestion == null)
                    TerrestrialQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(TerrestrialQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    TerrestrialQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the internet question.
        /// </summary>
        /// <value>The internet question.</value>
        // [Display(Name = "Internet")]
        public List<QuestionOptionModel> InternetQuestion { get; set; }

        /// <summary>
        /// Gets or sets the internet.
        /// </summary>
        /// <value>The internet.</value>
        // [Display(Name = "Internet")]
        public string Internet_TV
        {
            get
            {
                if (InternetQuestion == null)
                    InternetQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(InternetQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    InternetQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }
    }
}