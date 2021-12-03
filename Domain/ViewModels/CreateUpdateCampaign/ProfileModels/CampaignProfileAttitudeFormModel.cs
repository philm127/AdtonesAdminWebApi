using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels.CreateUpdateCampaign.ProfileModels
{
    public class CampaignProfileAttitudeFormModel : QuestionOptionsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileAttitudeFormModel"/> class.
        /// </summary>
        public CampaignProfileAttitudeFormModel()
        {
            FitnessQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
            HolidaysQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
            EnvironmentQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
            GoingOutQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
            FinancialStabiityQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
            ReligionQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
            FashionQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
            MusicQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
        }

        /// <summary>
        /// Gets or sets the campaign profile attitude identifier.
        /// </summary>
        /// <value>The campaign profile attitude identifier.</value>
        public int CampaignProfileAttitudeId { get; set; }

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
        /// Gets or sets the fitness question.
        /// </summary>
        /// <value>The fitness question.</value>
        // [Display(Name = "Fitness")]
        public List<QuestionOptionModel> FitnessQuestion { get; set; }

        /// <summary>
        /// Gets or sets the fitness.
        /// </summary>
        /// <value>The fitness.</value>
        // [Display(Name = "Fitness")]
        public string Fitness_Attitude
        {
            get
            {
                if (FitnessQuestion == null)
                    FitnessQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(FitnessQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    FitnessQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the holidays question.
        /// </summary>
        /// <value>The holidays question.</value>
        // [Display(Name = "Holidays")]
        public List<QuestionOptionModel> HolidaysQuestion { get; set; }

        /// <summary>
        /// Gets or sets the holidays.
        /// </summary>
        /// <value>The holidays.</value>
        // [Display(Name = "Holidays")]
        public string Holidays_Attitude
        {
            get
            {
                if (HolidaysQuestion == null)
                    HolidaysQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(HolidaysQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    HolidaysQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the environment question.
        /// </summary>
        /// <value>The environment question.</value>
        // [Display(Name = "Environment")]
        public List<QuestionOptionModel> EnvironmentQuestion { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        // [Display(Name = "Environment")]
        public string Environment_Attitude
        {
            get
            {
                if (EnvironmentQuestion == null)
                    EnvironmentQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(EnvironmentQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    EnvironmentQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the going out question.
        /// </summary>
        /// <value>The going out question.</value>
        // [Display(Name = "Going Out")]
        public List<QuestionOptionModel> GoingOutQuestion { get; set; }

        /// <summary>
        /// Gets or sets the going out.
        /// </summary>
        /// <value>The going out.</value>
        // [Display(Name = "Going Out")]
        public string GoingOut_Attitude
        {
            get
            {
                if (GoingOutQuestion == null)
                    GoingOutQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(GoingOutQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    GoingOutQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the financial stabiity question.
        /// </summary>
        /// <value>The financial stabiity question.</value>
        // [Display(Name = "Financial Stabiity")]
        public List<QuestionOptionModel> FinancialStabiityQuestion { get; set; }

        /// <summary>
        /// Gets or sets the financial stabiity.
        /// </summary>
        /// <value>The financial stabiity.</value>
        // [Display(Name = "Financial Stabiity")]
        public string FinancialStabiity_Attitude
        {
            get
            {
                if (FinancialStabiityQuestion == null)
                    FinancialStabiityQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(FinancialStabiityQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    FinancialStabiityQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the religion question.
        /// </summary>
        /// <value>The religion question.</value>
        // [Display(Name = "Religion")]
        public List<QuestionOptionModel> ReligionQuestion { get; set; }

        /// <summary>
        /// Gets or sets the religion.
        /// </summary>
        /// <value>The religion.</value>
        // [Display(Name = "Religion")]
        public string Religion_Attitude
        {
            get
            {
                if (ReligionQuestion == null)
                    ReligionQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(ReligionQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    ReligionQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the fashion question.
        /// </summary>
        /// <value>The fashion question.</value>
        // [Display(Name = "Fashion")]
        public List<QuestionOptionModel> FashionQuestion { get; set; }

        /// <summary>
        /// Gets or sets the fashion.
        /// </summary>
        /// <value>The fashion.</value>
        // [Display(Name = "Fashion")]
        public string Fashion_Attitude
        {
            get
            {
                if (FashionQuestion == null)
                    FashionQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(FashionQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    FashionQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the music question.
        /// </summary>
        /// <value>The music question.</value>
        // [Display(Name = "Music")]
        public List<QuestionOptionModel> MusicQuestion { get; set; }

        /// <summary>
        /// Gets or sets the music.
        /// </summary>
        /// <value>The music.</value>
        // [Display(Name = "Music")]
        public string Music_Attitude
        {
            get
            {
                if (MusicQuestion == null)
                    MusicQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(MusicQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    MusicQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }
    }
}