using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels
{
    public class CampaignProfileSkizaFormModel : QuestionOptionsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileSkizaFormModel"/> class.
        /// </summary>

        public CampaignProfileSkizaFormModel()
        {
            // EFMVCDataContex db = new EFMVCDataContex();

            HustlersQuestion =
            CompileQuestions(new Dictionary<string, bool>
                   {{"Networked Youth", false}, {"Stable Hustler", false}, {"Savvy Loyalist", false}});

            YouthQuestion =
           CompileQuestions(new Dictionary<string, bool>
                  {{"Tween", false}, {"Hi-Pot students", true}, {"Prudent Young", false}});

            DiscerningProfessionalsQuestion =
           CompileQuestions(new Dictionary<string, bool>
                  {{"Young Flashers", false}, {"Mature trendies", true}, {"Settled Middle Mgmt", false},{"Affluent Influencers", false}});

            MassQuestion =
           CompileQuestions(new Dictionary<string, bool>
                  {{"Young cautious caller", true}, {"Toa Mpango", false},{"Young progressive worker", false}, {"Older Toa Mpango", true}, {"Progressive worker", false}});
        }

        //public CampaignProfileSkizaFormModel(int CountryId)
        //{
        //    // EFMVCDataContex db = new EFMVCDataContex();

        //    //Hustlers
        //    var hustlersProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Hustlers".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
        //    var hustlersProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == hustlersProfileMatchId).ToList();

        //    Dictionary<string, bool> hustlers = new Dictionary<string, bool>();
        //    List<Dictionary<string, bool>> hustlerslist = new List<Dictionary<string, bool>>();

        //    foreach (var item in hustlersProfileLabel)
        //    {
        //        hustlers = new Dictionary<string, bool> { { item.ProfileLabel, false } };
        //        hustlerslist.Add(hustlers);
        //    }
        //    HustlersQuestion = CompileQuestionsDynamic(hustlerslist);

        //    //Youth
        //    var youthProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Youth".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
        //    var youthProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == youthProfileMatchId).ToList();

        //    Dictionary<string, bool> youth = new Dictionary<string, bool>();
        //    List<Dictionary<string, bool>> youthlist = new List<Dictionary<string, bool>>();

        //    foreach (var item in youthProfileLabel)
        //    {
        //        youth = new Dictionary<string, bool> { { item.ProfileLabel, false } };
        //        youthlist.Add(youth);
        //    }
        //    YouthQuestion = CompileQuestionsDynamic(youthlist);

        //    //DiscerningProfessionals
        //    var discerningProfessionalsProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Discerning Professionals".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
        //    var discerningProfessionalsProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == discerningProfessionalsProfileMatchId).ToList();

        //    Dictionary<string, bool> discerningProfessionals = new Dictionary<string, bool>();
        //    List<Dictionary<string, bool>> discerningProfessionalslist = new List<Dictionary<string, bool>>();

        //    foreach (var item in discerningProfessionalsProfileLabel)
        //    {
        //        discerningProfessionals = new Dictionary<string, bool> { { item.ProfileLabel, false } };
        //        discerningProfessionalslist.Add(discerningProfessionals);
        //    }
        //    DiscerningProfessionalsQuestion = CompileQuestionsDynamic(discerningProfessionalslist);


        //    //Mass
        //    var massProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Mass".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
        //    var massProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == massProfileMatchId).ToList();

        //    Dictionary<string, bool> mass = new Dictionary<string, bool>();
        //    List<Dictionary<string, bool>> masslist = new List<Dictionary<string, bool>>();

        //    foreach (var item in massProfileLabel)
        //    {
        //        mass = new Dictionary<string, bool> { { item.ProfileLabel, false } };
        //        masslist.Add(mass);
        //    }
        //    MassQuestion = CompileQuestionsDynamic(masslist);
        //}

        /// <summary>
        /// Gets or sets the campaign profile adverts identifier.
        /// </summary>
        /// <value>The campaign profile adverts identifier.</value>
        public int CampaignProfileSKizaId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile.
        /// </summary>
        /// <value>The campaign profile.</value>
        public CampaignProfileFormModel CampaignProfile { get; set; }


        // [Display(Name = "Hustlers")]
        public List<QuestionOptionModel> HustlersQuestion { get; set; }

        // [Display(Name = "Hustlers")]
        public string Hustlers_AdType
        {
            get
            {
                if (HustlersQuestion == null)
                    HustlersQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(HustlersQuestion));
            }
            set
            {
                if (HustlersQuestion != null && HustlersQuestion.Count() > 0)
                {
                    if (value == null) return;
                    for (int i = 0; i < value.Length; i++)
                        HustlersQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
                }
                else
                {
                    return;
                }
            }
        }


        // [Display(Name = "Youth")]
        public List<QuestionOptionModel> YouthQuestion { get; set; }

        // [Display(Name = "Youth")]
        public string Youth_AdType
        {
            get
            {
                if (YouthQuestion == null)
                    YouthQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(YouthQuestion));
            }
            set
            {
                if (YouthQuestion != null && YouthQuestion.Count() > 0)
                {
                    if (value == null) return;
                    for (int i = 0; i < value.Length; i++)
                        YouthQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
                }
                else
                {
                    return;
                }
            }
        }

        // [Display(Name = "Discerning Professionals")]
        public List<QuestionOptionModel> DiscerningProfessionalsQuestion { get; set; }

        // [Display(Name = "Discerning Professionals")]
        public string DiscerningProfessionals_AdType
        {
            get
            {
                if (DiscerningProfessionalsQuestion == null)
                    DiscerningProfessionalsQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(DiscerningProfessionalsQuestion));
            }
            set
            {
                if (DiscerningProfessionalsQuestion != null && DiscerningProfessionalsQuestion.Count() > 0)
                {
                    if (value == null) return;
                    for (int i = 0; i < value.Length; i++)
                        DiscerningProfessionalsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
                }
                else
                {
                    return;
                }
            }
        }

        // [Display(Name = "Mass")]
        public List<QuestionOptionModel> MassQuestion { get; set; }

        // [Display(Name = "Mass")]
        public string Mass_AdType
        {
            get
            {
                if (MassQuestion == null)
                    MassQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(MassQuestion));
            }
            set
            {
                if (MassQuestion != null && MassQuestion.Count() > 0)
                {
                    if (value == null) return;
                    for (int i = 0; i < value.Length; i++)
                        MassQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
                }
                else
                {
                    return;
                }
            }
        }

    }
}