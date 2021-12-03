using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels.CreateUpdateCampaign.ProfileModels
{
    public class CampaignProfileMobileFormModel : QuestionOptionsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileMobileFormModel"/> class.
        /// </summary>
        public CampaignProfileMobileFormModel()
        {
            //ContractTypeQuestion =
            //    CompileQuestions(new Dictionary<string, bool>
            //                         {{"Don't Know", true}, {"Pay As You Go", false}, {"Monthly Contract", false}});
            //SpendQuestion =
            //    CompileQuestions(new Dictionary<string, bool>
            //                         {
            //                             {"Don't Know", true},
            //                             {"0-9", true},
            //                             {"10-19", false},
            //                             {"20-29", false},
            //                             {"30-40", false},
            //                             {"40-49", false},
            //                             {"50+", false}
            //                         });
        }

        public CampaignProfileMobileFormModel(int CountryId, List<string> contractTypeProfileLabel, List<string> spendProfileLabel)
        {

            //ContractType
            
            Dictionary<string, bool> contractType = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> contractTypelist = new List<Dictionary<string, bool>>();

            foreach (var item in contractTypeProfileLabel)
            {
                contractType = new Dictionary<string, bool> { { item, false } };
                contractTypelist.Add(contractType);
            }
            ContractTypeQuestion = CompileQuestionsDynamic(contractTypelist);
            foreach (var item in ContractTypeQuestion)
            {
                if (item.QuestionName == "Don't Know")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Spend
            Dictionary<string, bool> spend = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> spendlist = new List<Dictionary<string, bool>>();

            foreach (var item in spendProfileLabel)
            {
                spend = new Dictionary<string, bool> { { item, false } };
                spendlist.Add(spend);
            }
            SpendQuestion = CompileQuestionsDynamic(spendlist);
            foreach (var item in SpendQuestion)
            {
                if (item.QuestionName == "Don't Know")
                {
                    item.DefaultAnswer = true;
                }
                if (item.QuestionName == "0-9")
                {
                    item.DefaultAnswer = true;
                }
            }

        }

        /// <summary>
        /// Gets or sets the campaign profile mobile identifier.
        /// </summary>
        /// <value>The campaign profile mobile identifier.</value>
        public int CampaignProfileMobileId { get; set; }

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
        /// Gets or sets the contract type question.
        /// </summary>
        /// <value>The contract type question.</value>
        //// [Display(Name = "ContractType")]
        // [Display(Name = "Mobile plan")]
        public List<QuestionOptionModel> ContractTypeQuestion { get; set; }

        /// <summary>
        /// Gets or sets the type of the contract.
        /// </summary>
        /// <value>The type of the contract.</value>
        //// [Display(Name = "ContractType")]
        // [Display(Name = "Mobile plan")]
        public string ContractType_Mobile { get; set; }
        //{
        //    get
        //    {
        //        if (ContractTypeQuestion == null)
        //            ContractTypeQuestion = new List<QuestionOptionModel>();
        //        return CompileAnswers(SortList(ContractTypeQuestion));
        //    }
        //    set
        //    {
        //        if (ContractTypeQuestion != null && ContractTypeQuestion.Count() > 0)
        //        {
        //            if (value == null) return;
        //            for (int i = 0; i < value.Length; i++)
        //                ContractTypeQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}


        /// <summary>
        /// Gets or sets the spend question.
        /// </summary>
        /// <value>The spend question.</value>
        // [Display(Name = "Average Monthly Spend")]
        public List<QuestionOptionModel> SpendQuestion { get; set; }

        /// <summary>
        /// Gets or sets the spend.
        /// </summary>
        /// <value>The spend.</value>
        // [Display(Name = "Average Monthly Spend")]
        public string Spend_Mobile { get; set; }
        //{
        //    get
        //    {
        //        if (SpendQuestion == null)
        //            SpendQuestion = new List<QuestionOptionModel>();
        //        return CompileAnswers(SortList(SpendQuestion));
        //    }
        //    set
        //    {
        //        if (SpendQuestion != null && SpendQuestion.Count() > 0)
        //        {
        //            if (value == null) return;
        //            for (int i = 0; i < value.Length; i++)
        //                SpendQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}

        public bool ContractType { get; set; }
        public bool AverageMonthlySpend { get; set; }
    }
}
