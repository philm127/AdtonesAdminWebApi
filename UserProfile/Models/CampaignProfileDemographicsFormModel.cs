using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserProfile.Models
{
    public class CampaignProfileDemographicsFormModel : QuestionOptionsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileDemographicsFormModel"/> class.
        /// </summary>

        public CampaignProfileDemographicsFormModel(){}

        public CampaignProfileDemographicsFormModel(int CountryId, List<string> ageProfileLabel, List<string>  genderProfileLabel,
                                                    List<string>  incomeProfileLabel, List<string> workingStatusProfileLabel, List<string> relationshipStatusProfileLabel,
                                                    List<string> educationProfileLabel, List<string> householdStatusProfileLabel)
        {
            // EFMVCDataContex db = new EFMVCDataContex();

            //Age

            Dictionary<string, bool> age = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> agelist = new List<Dictionary<string, bool>>();

            foreach (var item in ageProfileLabel)
            {
                age = new Dictionary<string, bool> { { item, false } };
                agelist.Add(age);
            }
            AgeQuestion = CompileQuestionsDynamic(agelist);
            foreach (var item in AgeQuestion)
            {
                if (item.QuestionName == "Prefer not to answer")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Gender
            Dictionary<string, bool> gender = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> genderlist = new List<Dictionary<string, bool>>();

            foreach (var item in genderProfileLabel)
            {
                gender = new Dictionary<string, bool> { { item, false } };
                genderlist.Add(gender);
            }
            GenderQuestion = CompileQuestionsDynamic(genderlist);
            foreach (var item in GenderQuestion)
            {
                if (item.QuestionName == "Prefer not to answer")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Income

            Dictionary<string, bool> income = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> incomelist = new List<Dictionary<string, bool>>();

            foreach (var item in incomeProfileLabel)
            {
                income = new Dictionary<string, bool> { { item, false } };
                incomelist.Add(income);
            }
            IncomeBracketQuestion = CompileQuestionsDynamic(incomelist);
            foreach (var item in IncomeBracketQuestion)
            {
                if (item.QuestionName == "Prefer not to answer")
                {
                    item.DefaultAnswer = true;
                }
            }

            //WorkingStatus
            
            Dictionary<string, bool> workingStatus = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> workingStatuslist = new List<Dictionary<string, bool>>();

            foreach (var item in workingStatusProfileLabel)
            {
                workingStatus = new Dictionary<string, bool> { { item, false } };
                workingStatuslist.Add(workingStatus);
            }
            WorkingStatusQuestion = CompileQuestionsDynamic(workingStatuslist);
            foreach (var item in WorkingStatusQuestion)
            {
                if (item.QuestionName == "Prefer not to answer")
                {
                    item.DefaultAnswer = true;
                }
            }

            //RelationshipStatus
            
            Dictionary<string, bool> relationshipStatus = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> relationshipStatuslist = new List<Dictionary<string, bool>>();

            foreach (var item in relationshipStatusProfileLabel)
            {
                relationshipStatus = new Dictionary<string, bool> { { item, false } };
                relationshipStatuslist.Add(relationshipStatus);
            }
            RelationshipStatusQuestion = CompileQuestionsDynamic(relationshipStatuslist);
            foreach (var item in RelationshipStatusQuestion)
            {
                if (item.QuestionName == "Prefer not to answer")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Education
            
            Dictionary<string, bool> education = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> educationlist = new List<Dictionary<string, bool>>();

            foreach (var item in educationProfileLabel)
            {
                education = new Dictionary<string, bool> { { item, false } };
                educationlist.Add(education);
            }
            EducationQuestion = CompileQuestionsDynamic(educationlist);
            foreach (var item in EducationQuestion)
            {
                if (item.QuestionName == "Prefer not to answer")
                {
                    item.DefaultAnswer = true;
                }
            }

            //HouseholdStatus
            
            Dictionary<string, bool> householdStatus = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> householdStatuslist = new List<Dictionary<string, bool>>();

            foreach (var item in householdStatusProfileLabel)
            {
                householdStatus = new Dictionary<string, bool> { { item, false } };
                householdStatuslist.Add(householdStatus);
            }
            HouseholdStatusQuestion = CompileQuestionsDynamic(householdStatuslist);
            foreach (var item in HouseholdStatusQuestion)
            {
                if (item.QuestionName == "Prefer not to answer")
                {
                    item.DefaultAnswer = true;
                }
            }

            
        }

        /// <summary>
        /// Gets or sets the campaign profile demographics identifier.
        /// </summary>
        /// <value>The campaign profile demographics identifier.</value>
        public int CampaignProfileDemographicsId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the dob start.
        /// </summary>
        /// <value>The dob start.</value>
        // [Display(Name = "DOB Start")]
        public DateTime? DOBStart_Demographics { get; set; }

        /// <summary>
        /// Gets or sets the dob end.
        /// </summary>
        /// <value>The dob end.</value>
        // [Display(Name = "DOB End")]
        public DateTime? DOBEnd_Demographics { get; set; }

        /// <summary>
        /// Gets or sets the age question.
        /// </summary>
        /// <value>The age question.</value>
        // [Display(Name = "Age")]
        public List<QuestionOptionModel> AgeQuestion { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>The age.</value>
        // [Display(Name = "Age")]
        public string Age_Demographics { get; set; }
        //{
        //    get
        //    {
        //        if (AgeQuestion == null)
        //            AgeQuestion = new List<QuestionOptionModel>();
        //        return CompileAnswers(SortList(AgeQuestion));
        //    }
        //    set
        //    {
        //        if (AgeQuestion != null && AgeQuestion.Count() > 0)
        //        {
        //            if (value == null) return;
        //            for (int i = 0; i < value.Length; i++)
        //                AgeQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}

        /// <summary>
        /// Gets or sets the gender question.
        /// </summary>
        /// <value>The gender question.</value>
        // [Display(Name = "Gender")]
        public List<QuestionOptionModel> GenderQuestion { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        // [Display(Name = "Gender")]
        public string Gender_Demographics { get; set; }
        //{
        //    get
        //    {
        //        if (GenderQuestion == null)
        //            GenderQuestion = new List<QuestionOptionModel>();
        //        return CompileAnswers(SortList(GenderQuestion));
        //    }
        //    set
        //    {
        //        if (GenderQuestion != null && GenderQuestion.Count() > 0)
        //        {
        //            if (value == null) return;
        //            for (int i = 0; i < value.Length; i++)
        //                GenderQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}


        /// <summary>
        /// Gets or sets the income bracket question.
        /// </summary>
        /// <value>The income bracket question.</value>
        // [Display(Name = "Income Bracket")]
        public List<QuestionOptionModel> IncomeBracketQuestion { get; set; }

        /// <summary>
        /// Gets or sets the income bracket.
        /// </summary>
        /// <value>The income bracket.</value>
        // [Display(Name = "Income Bracket")]
        public string IncomeBracket_Demographics { get; set; }
        //{
        //    get
        //    {
        //        if (IncomeBracketQuestion == null)
        //            IncomeBracketQuestion = new List<QuestionOptionModel>();
        //        return CompileAnswers(SortList(IncomeBracketQuestion));
        //    }
        //    set
        //    {
        //        if (IncomeBracketQuestion != null && IncomeBracketQuestion.Count() > 0)
        //        {
        //            if (value == null) return;
        //            for (int i = 0; i < value.Length; i++)
        //                IncomeBracketQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}


        /// <summary>
        /// Gets or sets the working status question.
        /// </summary>
        /// <value>The working status question.</value>
        // [Display(Name = "Working Status")]
        public List<QuestionOptionModel> WorkingStatusQuestion { get; set; }

        /// <summary>
        /// Gets or sets the working status.
        /// </summary>
        /// <value>The working status.</value>
        // [Display(Name = "Working Status")]
        public string WorkingStatus_Demographics { get; set; }
        //{
        //    get
        //    {
        //        if (WorkingStatusQuestion == null)
        //            WorkingStatusQuestion = new List<QuestionOptionModel>();
        //        return CompileAnswers(SortList(WorkingStatusQuestion));
        //    }
        //    set
        //    {
        //        if (WorkingStatusQuestion != null && WorkingStatusQuestion.Count() > 0)
        //        {
        //            if (value == null) return;
        //            for (int i = 0; i < value.Length; i++)
        //                WorkingStatusQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}


        /// <summary>
        /// Gets or sets the relationship status question.
        /// </summary>
        /// <value>The relationship status question.</value>
        // [Display(Name = "Relationship Status")]
        public List<QuestionOptionModel> RelationshipStatusQuestion { get; set; }

        /// <summary>
        /// Gets or sets the relationship status.
        /// </summary>
        /// <value>The relationship status.</value>
        // [Display(Name = "Relationship Status")]
        public string RelationshipStatus_Demographics { get; set; }
        //{
        //    get
        //    {
        //        if (RelationshipStatusQuestion == null)
        //            RelationshipStatusQuestion = new List<QuestionOptionModel>();
        //        return CompileAnswers(SortList(RelationshipStatusQuestion));
        //    }
        //    set
        //    {
        //        if (RelationshipStatusQuestion != null && RelationshipStatusQuestion.Count() > 0)
        //        {
        //            if (value == null) return;
        //            for (int i = 0; i < value.Length; i++)
        //                RelationshipStatusQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}


        /// <summary>
        /// Gets or sets the education question.
        /// </summary>
        /// <value>The education question.</value>
        // [Display(Name = "Education")]
        public List<QuestionOptionModel> EducationQuestion { get; set; }

        /// <summary>
        /// Gets or sets the education.
        /// </summary>
        /// <value>The education.</value>
        // [Display(Name = "Education")]
        public string Education_Demographics { get; set; }
        //{
        //    get
        //    {
        //        if (EducationQuestion == null)
        //            EducationQuestion = new List<QuestionOptionModel>();
        //        return CompileAnswers(SortList(EducationQuestion));
        //    }
        //    set
        //    {
        //        if (EducationQuestion != null && EducationQuestion.Count() > 0)
        //        {
        //            if (value == null) return;
        //            for (int i = 0; i < value.Length; i++)
        //                EducationQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}


        /// <summary>
        /// Gets or sets the household status question.
        /// </summary>
        /// <value>The household status question.</value>
        // [Display(Name = "Household Status")]
        public List<QuestionOptionModel> HouseholdStatusQuestion { get; set; }

        /// <summary>
        /// Gets or sets the household status.
        /// </summary>
        /// <value>The household status.</value>
        // [Display(Name = "Household Status")]
        public string HouseholdStatus_Demographics { get; set; }
        //{
        //    get
        //    {
        //        if (HouseholdStatusQuestion == null)
        //            HouseholdStatusQuestion = new List<QuestionOptionModel>();
        //        return CompileAnswers(SortList(HouseholdStatusQuestion));
        //    }
        //    set
        //    {
        //        if (HouseholdStatusQuestion != null && HouseholdStatusQuestion.Count() > 0)
        //        {
        //            if (value == null) return;
        //            for (int i = 0; i < value.Length; i++)
        //                HouseholdStatusQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}


        /// <summary>
        /// Gets or sets the msisdn.
        /// </summary>
        /// <value>The msisdn.</value>
        // [Display(Name = "MSISDN")]
        public string MSISDN { get; set; }

        public bool Age { get; set; }
        public bool Gender { get; set; }
        public bool HouseholdStatus { get; set; }
        public bool WorkingStatus { get; set; }
        public bool RelationshipStatus { get; set; }
        public bool Education { get; set; }
    }
}