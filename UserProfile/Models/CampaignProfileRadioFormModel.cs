using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserProfile.Models
{
    public class CampaignProfileRadioFormModel : QuestionOptionsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileRadioFormModel"/> class.
        /// </summary>
        public CampaignProfileRadioFormModel()
        {
            NationalQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            LocalQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            MusicQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            SportQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            TalkQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
        }

        /// <summary>
        /// Gets or sets the campaign profile radio identifier.
        /// </summary>
        /// <value>The campaign profile radio identifier.</value>
        public int CampaignProfileRadioId { get; set; }

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
        /// Gets or sets the national question.
        /// </summary>
        /// <value>The national question.</value>
        // [Display(Name = "National")]
        public List<QuestionOptionModel> NationalQuestion { get; set; }

        /// <summary>
        /// Gets or sets the national.
        /// </summary>
        /// <value>The national.</value>
        // [Display(Name = "National")]
        public string National_Radio
        {
            get
            {
                if (NationalQuestion == null)
                    NationalQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(NationalQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    NationalQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the local question.
        /// </summary>
        /// <value>The local question.</value>
        // [Display(Name = "Local")]
        public List<QuestionOptionModel> LocalQuestion { get; set; }

        /// <summary>
        /// Gets or sets the local.
        /// </summary>
        /// <value>The local.</value>
        // [Display(Name = "Local")]
        public string Local_Radio
        {
            get
            {
                if (LocalQuestion == null)
                    LocalQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(LocalQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    LocalQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
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
        public string Music_Radio
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


        /// <summary>
        /// Gets or sets the sport question.
        /// </summary>
        /// <value>The sport question.</value>
        // [Display(Name = "Sport")]
        public List<QuestionOptionModel> SportQuestion { get; set; }

        /// <summary>
        /// Gets or sets the sport.
        /// </summary>
        /// <value>The sport.</value>
        // [Display(Name = "Sport")]
        public string Sport_Radio
        {
            get
            {
                if (SportQuestion == null)
                    SportQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(SportQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    SportQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the talk question.
        /// </summary>
        /// <value>The talk question.</value>
        // [Display(Name = "Talk")]
        public List<QuestionOptionModel> TalkQuestion { get; set; }

        /// <summary>
        /// Gets or sets the talk.
        /// </summary>
        /// <value>The talk.</value>
        // [Display(Name = "Talk")]
        public string Talk_Radio
        {
            get
            {
                if (TalkQuestion == null)
                    TalkQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(TalkQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    TalkQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }
    }
}