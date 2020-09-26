//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace AdtonesAdminWebApi.ViewModels.AdProfilrModels
//{
//    public class CampaignProfileDemographicsFormModel : ArtharFormModel
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="CampaignProfileDemographicsFormModel"/> class.
//        /// </summary>

//        public CampaignProfileDemographicsFormModel()
//        {
//            AgeQuestion =
//               CompileQuestions(new Dictionary<string, bool>
//                                    {
//                                         {"Under 18", false},
//                                         {"18-24", false},
//                                         {"25-34", false},
//                                         {"35-44", false},
//                                         {"45-54", false},
//                                         {"55-64", false},
//                                         {"65+", false},
//                                         {"Prefer not to answer", true}
//                                    });

//            GenderQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Male", false}, {"Female", false}, {"Prefer not to answer", true}});
//            IncomeBracketQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {
//                                         {"£0 to £14,999", false},
//                                         {"£15,000 to £24,999", false},
//                                         {"£25,000 to £39,999", false},
//                                         {"£40,000 to £74,999", false},
//                                         {"£75,000 to £99,999", false},
//                                         {"£100,000+", false},
//                                         {"Prefer not to answer", true}
//                                     });
//            WorkingStatusQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {
//                                         {"Employed", false},
//                                         {"Self-Employed", false},
//                                         {"Housewife/Househusband", false},
//                                         {"Retired", false},
//                                         {"Unpaid Carer", false},
//                                         {"Full or Part-time Education", false},
//                                         {"Not Working", false},
//                                         {"None of these", false},
//                                         {"Prefer not to answer", true}
//                                     });
//            RelationshipStatusQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {
//                                         {"Divorced", false},
//                                         {"Living with another", false},
//                                         {"Married", false},
//                                         {"Separated", false},
//                                         {"Single", false},
//                                         {"Widowed", false},
//                                         {"Prefer not to answer", true}
//                                     });
//            EducationQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {
//                                         {"Secondary", false},
//                                         {"College", false},
//                                         {"University", false},
//                                         {"Post Graduate", false},
//                                         {"Prefer not to answer", true}
//                                     });
//            HouseholdStatusQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {
//                                         {"Rent", false},
//                                         {"Owner", false},
//                                         {"Live with someone", false},
//                                         {"Prefer not to answer", true}
//                                     });
//        }

//        public CampaignProfileDemographicsFormModel(int CountryId)
//        {
//            EFMVCDataContex db = new EFMVCDataContex();

//            //Age
//            var ageProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Age".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var ageProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == ageProfileMatchId).ToList();

//            Dictionary<string, bool> age = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> agelist = new List<Dictionary<string, bool>>();

//            foreach (var item in ageProfileLabel)
//            {
//                age = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                agelist.Add(age);
//            }
//            AgeQuestion = CompileQuestionsDynamic(agelist);
//            foreach (var item in AgeQuestion)
//            {
//                if (item.QuestionName == "Prefer not to answer")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }
//            //AgeQuestion = (from top in AgeQuestion where top.QuestionName == "Prefer not to answer" select top).ToList()
//            //               .ForEach(a => a.DefaultAnswer = true);

//            //var test = AgeQuestion.Where(p => p.QuestionName == "Prefer not to answer").Select(u => { u.DefaultAnswer = true; return u; }).ToList();
//            //var query = (from agequestion in AgeQuestion
//            //             where agequestion.QuestionName == "Prefer not to answer"
//            //             select agequestion).SingleOrDefault()
//            //             .Update(st => { st.Standard = "0"; st.Status = "X"; });
//            //Gender
//            var genderProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Gender".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var genderProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == genderProfileMatchId).ToList();

//            Dictionary<string, bool> gender = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> genderlist = new List<Dictionary<string, bool>>();

//            foreach (var item in genderProfileLabel)
//            {
//                gender = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                genderlist.Add(gender);
//            }
//            GenderQuestion = CompileQuestionsDynamic(genderlist);
//            foreach (var item in GenderQuestion)
//            {
//                if (item.QuestionName == "Prefer not to answer")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Income
//            var incomeProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("IncomeBracket".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var incomeProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == incomeProfileMatchId).ToList();

//            Dictionary<string, bool> income = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> incomelist = new List<Dictionary<string, bool>>();

//            foreach (var item in incomeProfileLabel)
//            {
//                income = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                incomelist.Add(income);
//            }
//            IncomeBracketQuestion = CompileQuestionsDynamic(incomelist);
//            foreach (var item in IncomeBracketQuestion)
//            {
//                if (item.QuestionName == "Prefer not to answer")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //WorkingStatus
//            var workingStatusProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Working Status".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var workingStatusProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == workingStatusProfileMatchId).ToList();

//            Dictionary<string, bool> workingStatus = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> workingStatuslist = new List<Dictionary<string, bool>>();

//            foreach (var item in workingStatusProfileLabel)
//            {
//                workingStatus = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                workingStatuslist.Add(workingStatus);
//            }
//            WorkingStatusQuestion = CompileQuestionsDynamic(workingStatuslist);
//            foreach (var item in WorkingStatusQuestion)
//            {
//                if (item.QuestionName == "Prefer not to answer")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //RelationshipStatus
//            var relationshipStatusProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Relationship Status".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var relationshipStatusProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == relationshipStatusProfileMatchId).ToList();

//            Dictionary<string, bool> relationshipStatus = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> relationshipStatuslist = new List<Dictionary<string, bool>>();

//            foreach (var item in relationshipStatusProfileLabel)
//            {
//                relationshipStatus = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                relationshipStatuslist.Add(relationshipStatus);
//            }
//            RelationshipStatusQuestion = CompileQuestionsDynamic(relationshipStatuslist);
//            foreach (var item in RelationshipStatusQuestion)
//            {
//                if (item.QuestionName == "Prefer not to answer")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Education
//            var educationProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Education".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var educationProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == educationProfileMatchId).ToList();

//            Dictionary<string, bool> education = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> educationlist = new List<Dictionary<string, bool>>();

//            foreach (var item in educationProfileLabel)
//            {
//                education = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                educationlist.Add(education);
//            }
//            EducationQuestion = CompileQuestionsDynamic(educationlist);
//            foreach (var item in EducationQuestion)
//            {
//                if (item.QuestionName == "Prefer not to answer")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //HouseholdStatus
//            var householdStatusProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Household Status".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var householdStatusProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == householdStatusProfileMatchId).ToList();

//            Dictionary<string, bool> householdStatus = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> householdStatuslist = new List<Dictionary<string, bool>>();

//            foreach (var item in householdStatusProfileLabel)
//            {
//                householdStatus = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                householdStatuslist.Add(householdStatus);
//            }
//            HouseholdStatusQuestion = CompileQuestionsDynamic(householdStatuslist);
//            foreach (var item in HouseholdStatusQuestion)
//            {
//                if (item.QuestionName == "Prefer not to answer")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //AgeQuestion =
//            //   CompileQuestions(new Dictionary<string, bool>
//            //                        {
//            //                             {"Under 18", false},
//            //                             {"18-24", false},
//            //                             {"25-34", false},
//            //                             {"35-44", false},
//            //                             {"45-54", false},
//            //                             {"55-64", false},
//            //                             {"65+", false},
//            //                             {"Prefer not to answer", true}
//            //                        });

//            //GenderQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Male", false}, {"Female", false}, {"Prefer not to answer", true}});
//            //IncomeBracketQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {
//            //                             {"£0 to £14,999", false},
//            //                             {"£15,000 to £24,999", false},
//            //                             {"£25,000 to £39,999", false},
//            //                             {"£40,000 to £74,999", false},
//            //                             {"£75,000 to £99,999", false},
//            //                             {"£100,000+", false},
//            //                             {"Prefer not to answer", true}
//            //                         });
//            //WorkingStatusQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {
//            //                             {"Employed", false},
//            //                             {"Self-Employed", false},
//            //                             {"Housewife/Househusband", false},
//            //                             {"Retired", false},
//            //                             {"Unpaid Carer", false},
//            //                             {"Full or Part-time Education", false},
//            //                             {"Not Working", false},
//            //                             {"None of these", false},
//            //                             {"Prefer not to answer", true}
//            //                         });
//            //RelationshipStatusQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {
//            //                             {"Divorced", false},
//            //                             {"Living with another", false},
//            //                             {"Married", false},
//            //                             {"Separated", false},
//            //                             {"Single", false},
//            //                             {"Widowed", false},
//            //                             {"Prefer not to answer", true}
//            //                         });
//            //EducationQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {
//            //                             {"Secondary", false},
//            //                             {"College", false},
//            //                             {"University", false},
//            //                             {"Post Graduate", false},
//            //                             {"Prefer not to answer", true}
//            //                         });
//            //HouseholdStatusQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {
//            //                             {"Rent", false},
//            //                             {"Owner", false},
//            //                             {"Live with someone", false},
//            //                             {"Prefer not to answer", true}
//            //                         });

//            //code commented on 29-03-2017
//            //LocationQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {
//            //                             {"London", false},
//            //                             {"South East (excl. London)", false},
//            //                             {"South West", false},
//            //                             {"East Anglia", false},
//            //                             {"East Midlands", false},
//            //                             {"West Midlands", false},
//            //                             {"Wales", false},
//            //                             {"North West", false},
//            //                             {"North East", false},
//            //                             {"Scotland", false},
//            //                             {"Northern Ireland", true},
//            //                             {"Prefer not to answer", true}
//            //                         });
//        }

//        /// <summary>
//        /// Gets or sets the campaign profile demographics identifier.
//        /// </summary>
//        /// <value>The campaign profile demographics identifier.</value>
//        [Key]
//        public int CampaignProfileDemographicsId { get; set; }

//        /// <summary>
//        /// Gets or sets the campaign profile identifier.
//        /// </summary>
//        /// <value>The campaign profile identifier.</value>
//        public int CampaignProfileId { get; set; }

//        /// <summary>
//        /// Gets or sets the dob start.
//        /// </summary>
//        /// <value>The dob start.</value>
//        [Display(Name = "DOB Start")]
//        public DateTime? DOBStart_Demographics { get; set; }

//        /// <summary>
//        /// Gets or sets the dob end.
//        /// </summary>
//        /// <value>The dob end.</value>
//        [Display(Name = "DOB End")]
//        public DateTime? DOBEnd_Demographics { get; set; }

//        /// <summary>
//        /// Gets or sets the age question.
//        /// </summary>
//        /// <value>The age question.</value>
//        [Display(Name = "Age")]
//        public List<QuestionOptionModel> AgeQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the age.
//        /// </summary>
//        /// <value>The age.</value>
//        [Display(Name = "Age")]
//        public string Age_Demographics
//        {
//            get
//            {
//                if (AgeQuestion == null)
//                    AgeQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(AgeQuestion));
//            }
//            set
//            {
//                if (AgeQuestion != null && AgeQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        AgeQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets or sets the gender question.
//        /// </summary>
//        /// <value>The gender question.</value>
//        [Display(Name = "Gender")]
//        public List<QuestionOptionModel> GenderQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the gender.
//        /// </summary>
//        /// <value>The gender.</value>
//        [Display(Name = "Gender")]
//        public string Gender_Demographics
//        {
//            get
//            {
//                if (GenderQuestion == null)
//                    GenderQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(GenderQuestion));
//            }
//            set
//            {
//                if (GenderQuestion != null && GenderQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        GenderQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the income bracket question.
//        /// </summary>
//        /// <value>The income bracket question.</value>
//        [Display(Name = "Income Bracket")]
//        public List<QuestionOptionModel> IncomeBracketQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the income bracket.
//        /// </summary>
//        /// <value>The income bracket.</value>
//        [Display(Name = "Income Bracket")]
//        public string IncomeBracket_Demographics
//        {
//            get
//            {
//                if (IncomeBracketQuestion == null)
//                    IncomeBracketQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(IncomeBracketQuestion));
//            }
//            set
//            {
//                if (IncomeBracketQuestion != null && IncomeBracketQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        IncomeBracketQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the working status question.
//        /// </summary>
//        /// <value>The working status question.</value>
//        [Display(Name = "Working Status")]
//        public List<QuestionOptionModel> WorkingStatusQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the working status.
//        /// </summary>
//        /// <value>The working status.</value>
//        [Display(Name = "Working Status")]
//        public string WorkingStatus_Demographics
//        {
//            get
//            {
//                if (WorkingStatusQuestion == null)
//                    WorkingStatusQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(WorkingStatusQuestion));
//            }
//            set
//            {
//                if (WorkingStatusQuestion != null && WorkingStatusQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        WorkingStatusQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the relationship status question.
//        /// </summary>
//        /// <value>The relationship status question.</value>
//        [Display(Name = "Relationship Status")]
//        public List<QuestionOptionModel> RelationshipStatusQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the relationship status.
//        /// </summary>
//        /// <value>The relationship status.</value>
//        [Display(Name = "Relationship Status")]
//        public string RelationshipStatus_Demographics
//        {
//            get
//            {
//                if (RelationshipStatusQuestion == null)
//                    RelationshipStatusQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(RelationshipStatusQuestion));
//            }
//            set
//            {
//                if (RelationshipStatusQuestion != null && RelationshipStatusQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        RelationshipStatusQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the education question.
//        /// </summary>
//        /// <value>The education question.</value>
//        // [Display(Name = "Education")]
//        public List<QuestionOptionModel> EducationQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the education.
//        /// </summary>
//        /// <value>The education.</value>
//        [Display(Name = "Education")]
//        public string Education_Demographics
//        {
//            get
//            {
//                if (EducationQuestion == null)
//                    EducationQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(EducationQuestion));
//            }
//            set
//            {
//                if (EducationQuestion != null && EducationQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        EducationQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the household status question.
//        /// </summary>
//        /// <value>The household status question.</value>
//        // [Display(Name = "Household Status")]
//        public List<QuestionOptionModel> HouseholdStatusQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the household status.
//        /// </summary>
//        /// <value>The household status.</value>
//        // [Display(Name = "Household Status")]
//        public string HouseholdStatus_Demographics
//        {
//            get
//            {
//                if (HouseholdStatusQuestion == null)
//                    HouseholdStatusQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(HouseholdStatusQuestion));
//            }
//            set
//            {
//                if (HouseholdStatusQuestion != null && HouseholdStatusQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        HouseholdStatusQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }



//        public string MSISDN { get; set; }

//        public bool Age { get; set; }
//        public bool Gender { get; set; }
//        public bool HouseholdStatus { get; set; }
//        public bool WorkingStatus { get; set; }
//        public bool RelationshipStatus { get; set; }
//        public bool Education { get; set; }
//    }

