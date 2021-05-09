using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class CreateCheckSaveProfileModels : ICreateCheckSaveProfileModels
    {
        private readonly IHttpContextAccessor _httpAccessor;

        private readonly ICampaignDAL _campaignDAL;
        private readonly ICreateUpdateCampaignDAL _createDAL;
        private readonly IConnectionStringService _connService;
        private readonly IPrematchProcess _matchProcess;
        private readonly IUserMatchDAL _matchDAL;
        private readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        ReturnResult result = new ReturnResult();


        public CreateCheckSaveProfileModels(IHttpContextAccessor httpAccessor, ICampaignDAL campaignDAL, IPrematchProcess matchProcess,
                                            ICreateUpdateCampaignDAL createDAL, IConnectionStringService connService,
                                            IUserMatchDAL matchDAL, IMapper mapper, IConfiguration configuration)
        {
            _httpAccessor = httpAccessor;
            _campaignDAL = campaignDAL;
            _createDAL = createDAL;
            _connService = connService;
            _matchProcess = matchProcess;
            _matchDAL = matchDAL;
            _mapper = mapper;
            _configuration = configuration;
        }


        #region GeoGraphic


        /// <summary>
        /// Gets a blank Profile Model
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public CampaignProfileGeographicFormModel GetGeographicModel(int countryId, int profileId = 0)
        {
            var locationProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "location").Result;
            var locationProfileLabel = _matchDAL.GetProfileMatchLabels(locationProfileMatchId).Result;
            CampaignProfileGeographicFormModel CampaignProfileGeo = new CampaignProfileGeographicFormModel(countryId, locationProfileLabel.ToList()) { CampaignProfileId = 0 };

            return CampaignProfileGeo;
        }


        public async Task<CampaignProfileGeographicFormModel> GetGeographicData(int profileId, CampaignProfilePreference CampaignProfileGeograph)
        {
            var campaignProfile = await _campaignDAL.GetCampaignProfileDetail(profileId);
            CampaignProfileGeographicFormModel CampaignProfileGeo = GetGeographicModel(campaignProfile.CountryId.Value);

            int CountryId = campaignProfile.CountryId.Value;

            if (campaignProfile != null)
            {
                if (CampaignProfileGeograph.CountryId == 0)
                {
                    CampaignProfileGeograph.CountryId = CountryId;
                }

                if (CampaignProfileGeograph.Location_Demographics != null && CampaignProfileGeograph.Location_Demographics.Length > 0)
                {

                    for (int i = 0; i < CampaignProfileGeograph.Location_Demographics.Length; i++)
                        CampaignProfileGeo.LocationQuestion.Find(x => x.QuestionValue == CampaignProfileGeograph.Location_Demographics.Substring(i, 1)).Selected = true;
                }

                CampaignProfileGeo.CampaignProfileGeographicId = CampaignProfileGeograph.Id;
                CampaignProfileGeo.CampaignProfileId = profileId;
                CampaignProfileGeo.CountryId = CountryId;
            }

            return CampaignProfileGeo;
        }


        public async Task<bool> SaveGeographicWizard(CampaignProfileGeographicFormModel model, List<string> connString)
        {
            try
            {
                var command = new CreateOrUpdateCampaignProfileGeographicCommand();

                command.CampaignProfileId = model.CampaignProfileId;
                command.CountryId = model.CountryId;
                command.PostCode = model.PostCode;
                command.Location_Demographics = CompileAnswers(SortList(model.LocationQuestion));

                var result = await _matchDAL.UpdateGeographicProfile(command, connString);

                var result2 = await _matchDAL.UpdateMatchCampaignGeographic(command, connString);
            }
            catch
            {
                throw;
            }
            return true;
        }



        #endregion


        #region TimeSettings


        public CampaignProfileTimeSettingFormModel GetTimeSettingModel(int Id = 0)
        {
            var timeSettings = new CampaignProfileTimeSettingFormModel
            { CampaignProfileId = Id, 
                MondaySelectedTimes = GetTimes(),
                TuesdaySelectedTimes = GetTimes(),
                WednesdaySelectedTimes = GetTimes(),
                ThursdaySelectedTimes = GetTimes(),
                FridaySelectedTimes = GetTimes(),
                SaturdaySelectedTimes = GetTimes(),
                SundaySelectedTimes = GetTimes(),
            };
            
            return timeSettings;
        }


        public async Task<CampaignProfileTimeSettingFormModel> GetTimeSettingData(int campaignId)
        {
            var Timing = new CampaignProfileTimeSettingFormModel { CampaignProfileId = campaignId };
            CampaignProfileTimeSetting CampaignProfileTimeSettings = await _matchDAL.GetCampaignTimeSettings(campaignId);


            if (CampaignProfileTimeSettings != null)
            {
                Timing = new CampaignProfileTimeSettingFormModel
                {
                    CampaignProfileId = campaignId,
                    CampaignProfileTimeSettingsId =
                                      CampaignProfileTimeSettings.
                                      CampaignProfileTimeSettingsId
                };
                if (CampaignProfileTimeSettings.Monday != null)
                    Timing.MondaySelectedTimes =
                        ConvertTimesArrayToList(CampaignProfileTimeSettings.Monday.Split(",".ToCharArray()));
                else
                    Timing.MondaySelectedTimes = GetTimes();
                if (CampaignProfileTimeSettings.Tuesday != null)
                    Timing.TuesdaySelectedTimes =
                        ConvertTimesArrayToList(CampaignProfileTimeSettings.Tuesday.Split(",".ToCharArray()));
                else
                    Timing.TuesdaySelectedTimes = GetTimes();
                if (CampaignProfileTimeSettings.Wednesday != null)
                    Timing.WednesdaySelectedTimes =
                        ConvertTimesArrayToList(CampaignProfileTimeSettings.Wednesday.Split(",".ToCharArray()));
                else
                    Timing.WednesdaySelectedTimes = GetTimes();
                if (CampaignProfileTimeSettings.Thursday != null)
                    Timing.ThursdaySelectedTimes =
                        ConvertTimesArrayToList(CampaignProfileTimeSettings.Thursday.Split(",".ToCharArray()));
                else
                    Timing.ThursdaySelectedTimes = GetTimes();
                if (CampaignProfileTimeSettings.Friday != null)
                    Timing.FridaySelectedTimes =
                        ConvertTimesArrayToList(CampaignProfileTimeSettings.Friday.Split(",".ToCharArray()));
                else
                    Timing.FridaySelectedTimes = GetTimes();
                if (CampaignProfileTimeSettings.Saturday != null)
                    Timing.SaturdaySelectedTimes =
                        ConvertTimesArrayToList(CampaignProfileTimeSettings.Saturday.Split(",".ToCharArray()));
                else
                    Timing.SaturdaySelectedTimes = GetTimes();
                if (CampaignProfileTimeSettings.Sunday != null)
                    Timing.SundaySelectedTimes =
                        ConvertTimesArrayToList(CampaignProfileTimeSettings.Sunday.Split(",".ToCharArray()));
                else
                    Timing.SundaySelectedTimes = GetTimes();


            }
            else
            {
                Timing = GetTimeSettingModel();
            }

            return Timing;
        }


        public async Task<bool> SaveTimeSettingsWizard(CampaignProfileTimeSettingFormModel model, List<string> connString)
        {

            var timeSettings = new CampaignProfileTimeSetting();
            timeSettings = new CampaignProfileTimeSetting();
            timeSettings.Monday = null;
            foreach (var item in model.MondaySelectedTimes)
            {
                if (item.IsSelected)
                    timeSettings.Monday += item.Name + ",";
            }

            if (!string.IsNullOrEmpty(timeSettings.Monday))
                timeSettings.Monday = timeSettings.Monday.Substring(0, timeSettings.Monday.Length - 1);
            else
                timeSettings.Monday = string.Empty;

            timeSettings.Tuesday = null;
            foreach (var item in model.TuesdaySelectedTimes)
            {
                if (item.IsSelected)
                    timeSettings.Tuesday += item.Name + ",";
            }

            if (!string.IsNullOrEmpty(timeSettings.Tuesday))
                timeSettings.Tuesday = timeSettings.Tuesday.Substring(0, timeSettings.Tuesday.Length - 1);
            else
                timeSettings.Tuesday = string.Empty;

            timeSettings.Wednesday = null;
            foreach (var item in model.WednesdaySelectedTimes)
            {
                if (item.IsSelected)
                    timeSettings.Wednesday += item.Name + ",";
            }

            if (!string.IsNullOrEmpty(timeSettings.Wednesday))
                timeSettings.Wednesday = timeSettings.Wednesday.Substring(0, timeSettings.Wednesday.Length - 1);
            else
                timeSettings.Wednesday = string.Empty;

            timeSettings.Thursday = null;
            foreach (var item in model.ThursdaySelectedTimes)
            {
                if (item.IsSelected)
                    timeSettings.Thursday += item.Name + ",";
            }

            if (!string.IsNullOrEmpty(timeSettings.Thursday))
                timeSettings.Thursday = timeSettings.Thursday.Substring(0, timeSettings.Thursday.Length - 1);
            else
                timeSettings.Thursday = string.Empty;

            timeSettings.Friday = null;
            foreach (var item in model.FridaySelectedTimes)
            {
                if (item.IsSelected)
                    timeSettings.Friday += item.Name + ",";
            }

            if (!string.IsNullOrEmpty(timeSettings.Friday))
                timeSettings.Friday = timeSettings.Friday.Substring(0, timeSettings.Friday.Length - 1);
            else
                timeSettings.Friday = string.Empty;

            timeSettings.Saturday = null;
            foreach (var item in model.SaturdaySelectedTimes)
            {
                if (item.IsSelected)
                    timeSettings.Saturday += item.Name + ",";
            }

            if (!string.IsNullOrEmpty(timeSettings.Saturday))
                timeSettings.Saturday = timeSettings.Saturday.Substring(0, timeSettings.Saturday.Length - 1);
            else
                timeSettings.Saturday = string.Empty;

            timeSettings.Sunday = null;
            foreach (var item in model.SundaySelectedTimes)
            {
                if (item.IsSelected)
                    timeSettings.Sunday += item.Name + ",";
            }

            if (!string.IsNullOrEmpty(timeSettings.Sunday))
                timeSettings.Sunday = timeSettings.Sunday.Substring(0, timeSettings.Sunday.Length - 1);
            else
                timeSettings.Sunday = string.Empty;

            timeSettings.CampaignProfileId = model.CampaignProfileId;
            timeSettings.CampaignProfileTimeSettingsId = model.CampaignProfileTimeSettingsId;

            var x = await _matchDAL.InsertTimeSettingsProfile(timeSettings, connString);
            return true;
        }


        private IList<TimeOfDay> ConvertTimesArrayToList(string[] selectedTimes)
        {
            var timings = GetTimes();
            for(int i = 0; i < timings.Count; i++)
            {
                var sub = timings[i].Id;
                if (selectedTimes.Contains(sub))
                {
                    timings[i].IsSelected = true;
                }
            }

            return timings;
        }


        private IList<TimeOfDay> GetTimes()
        {
            IList<TimeOfDay> times = new List<TimeOfDay>();
            times.Add(new TimeOfDay { Id = "01:00", Name = "01:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "02:00", Name = "02:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "03:00", Name = "03:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "04:00", Name = "04:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "05:00", Name = "05:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "06:00", Name = "06:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "07:00", Name = "07:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "08:00", Name = "08:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "09:00", Name = "09:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "10:00", Name = "10:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "11:00", Name = "11:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "12:00", Name = "12:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "13:00", Name = "13:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "14:00", Name = "14:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "15:00", Name = "15:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "16:00", Name = "16:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "17:00", Name = "17:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "18:00", Name = "18:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "19:00", Name = "19:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "20:00", Name = "20:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "21:00", Name = "21:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "22:00", Name = "22:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "23:00", Name = "23:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "24:00", Name = "24:00", IsSelected = false });

            return times;
        }


        #endregion



        #region DemoGraphic

        /// <summary>
        /// Gets a blank Profile Model
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public CampaignProfileDemographicsFormModel GetDemographicModel(int countryId, int profileId = 0)
        {
            var ageProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "age").Result;
            var ageProfileLabel = _matchDAL.GetProfileMatchLabels(ageProfileMatchId).Result;

            var genderProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "gender").Result;
            var genderProfileLabel = _matchDAL.GetProfileMatchLabels(genderProfileMatchId).Result;

            var incomeProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "IncomeBracket".ToLower()).Result;
            var incomeProfileLabel = _matchDAL.GetProfileMatchLabels(incomeProfileMatchId).Result;

            var workingStatusProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Working Status".ToLower()).Result;
            var workingStatusProfileLabel = _matchDAL.GetProfileMatchLabels(workingStatusProfileMatchId).Result;

            var relationshipStatusProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Relationship Status".ToLower()).Result;
            var relationshipStatusProfileLabel = _matchDAL.GetProfileMatchLabels(relationshipStatusProfileMatchId).Result;

            var educationProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Education".ToLower()).Result;
            var educationProfileLabel = _matchDAL.GetProfileMatchLabels(educationProfileMatchId).Result;

            var householdStatusProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Household Status".ToLower()).Result;
            var householdStatusProfileLabel = _matchDAL.GetProfileMatchLabels(householdStatusProfileMatchId).Result;

            CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel = new CampaignProfileDemographicsFormModel(countryId, ageProfileLabel.ToList(),
                                                                        genderProfileLabel.ToList(), incomeProfileLabel.ToList(), workingStatusProfileLabel.ToList(), relationshipStatusProfileLabel.ToList(),
                                                                        educationProfileLabel.ToList(), householdStatusProfileLabel.ToList());

            // CampaignProfileDemographicsmodel = CampaignProfileDemographicMapping(countryId, CampaignProfileDemographicsmodel);


            return CampaignProfileDemographicsmodel;


        }

        public async Task<CampaignProfileDemographicsFormModel> GetDemographicData(int profileId, CampaignProfilePreference CampaignProfileDemograph)
        {
            var campaignProfile = await _campaignDAL.GetCampaignProfileDetail(profileId);
            CampaignProfileDemographicsFormModel model = GetDemographicModel(campaignProfile.CountryId.Value);

            int CountryId = campaignProfile.CountryId.Value;

            if (campaignProfile != null)
            {
                if (CampaignProfileDemograph.CountryId == 0)
                {
                    CampaignProfileDemograph.CountryId = CountryId;
                }

                if (CampaignProfileDemograph.Age_Demographics != null && CampaignProfileDemograph.Age_Demographics.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileDemograph.Age_Demographics.Length; i++)
                    {
                        model.AgeQuestion.Find(x => x.QuestionValue == CampaignProfileDemograph.Age_Demographics.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileDemograph.Gender_Demographics != null && CampaignProfileDemograph.Gender_Demographics.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileDemograph.Gender_Demographics.Length; i++)
                    {
                        model.GenderQuestion.Find(x => x.QuestionValue == CampaignProfileDemograph.Gender_Demographics.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileDemograph.IncomeBracket_Demographics != null && CampaignProfileDemograph.IncomeBracket_Demographics.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileDemograph.IncomeBracket_Demographics.Length; i++)
                    {
                        model.IncomeBracketQuestion.Find(x => x.QuestionValue == CampaignProfileDemograph.IncomeBracket_Demographics.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileDemograph.WorkingStatus_Demographics != null && CampaignProfileDemograph.WorkingStatus_Demographics.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileDemograph.WorkingStatus_Demographics.Length; i++)
                    {
                        model.WorkingStatusQuestion.Find(x => x.QuestionValue == CampaignProfileDemograph.WorkingStatus_Demographics.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileDemograph.RelationshipStatus_Demographics != null && CampaignProfileDemograph.RelationshipStatus_Demographics.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileDemograph.RelationshipStatus_Demographics.Length; i++)
                    {
                        model.RelationshipStatusQuestion.Find(x => x.QuestionValue == CampaignProfileDemograph.RelationshipStatus_Demographics.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileDemograph.Education_Demographics != null && CampaignProfileDemograph.Education_Demographics.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileDemograph.Education_Demographics.Length; i++)
                    {
                        model.EducationQuestion.Find(x => x.QuestionValue == CampaignProfileDemograph.Education_Demographics.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileDemograph.HouseholdStatus_Demographics != null && CampaignProfileDemograph.HouseholdStatus_Demographics.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileDemograph.HouseholdStatus_Demographics.Length; i++)
                    {
                        model.HouseholdStatusQuestion.Find(x => x.QuestionValue == CampaignProfileDemograph.HouseholdStatus_Demographics.Substring(i, 1)).Selected = true;
                    }
                }

                model.CampaignProfileDemographicsId = CampaignProfileDemograph.Id;
                model.CampaignProfileId = profileId;
            }

            return model;
        }


        private bool checkcampaignProfileDemographics(CampaignProfilePreference campaignProfileDemographics)
        {
            if (String.IsNullOrEmpty(campaignProfileDemographics.Age_Demographics) && String.IsNullOrEmpty(campaignProfileDemographics.Gender_Demographics) && String.IsNullOrEmpty(campaignProfileDemographics.IncomeBracket_Demographics) &&
                String.IsNullOrEmpty(campaignProfileDemographics.WorkingStatus_Demographics) && String.IsNullOrEmpty(campaignProfileDemographics.RelationshipStatus_Demographics) && String.IsNullOrEmpty(campaignProfileDemographics.Education_Demographics)
                    && String.IsNullOrEmpty(campaignProfileDemographics.HouseholdStatus_Demographics))// code commented on 29-03-2017 && String.IsNullOrEmpty(campaignProfileDemographics.Location_Demographics))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        // Update Demographics from Ads Profile Mapping
        public async Task<bool> SaveDemographicsWizard(CampaignProfileDemographicsFormModel model, List<string> connString)
        {
            try
            {
                int? id = model.CampaignProfileId;
                var command = new CreateOrUpdateCampaignProfileDemographicsCommand();

                command.CampaignProfileId = model.CampaignProfileId;

                command.Age_Demographics = CompileAnswers(SortList(model.AgeQuestion));
                command.Education_Demographics = CompileAnswers(SortList(model.EducationQuestion));
                command.Gender_Demographics = CompileAnswers(SortList(model.GenderQuestion));
                command.HouseholdStatus_Demographics = CompileAnswers(SortList(model.HouseholdStatusQuestion));
                // command.IncomeBracket_Demographics = CompileAnswers(SortList(model.IncomeBracketQuestion));
                command.RelationshipStatus_Demographics = CompileAnswers(SortList(model.RelationshipStatusQuestion));
                command.WorkingStatus_Demographics = CompileAnswers(SortList(model.WorkingStatusQuestion));

                var prefId = await _matchDAL.UpdateDemographicProfile(command, connString);
                var result2 = await _matchDAL.UpdateMatchCampaignDemographic(command, connString);
            }
            catch
            {
                throw;
            }
            return true;
        }


        #endregion



        #region Mobile

        public async Task<CampaignProfileMobileFormModel> GetMobileModel(int countryId, int profileId = 0)
        {
            var contractTypeProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Mobile plan".ToLower());
            var contractTypeProfileLabel = await _matchDAL.GetProfileMatchLabels(contractTypeProfileMatchId);

            var spendProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Average Monthly Spend".ToLower());
            var spendProfileLabel = await _matchDAL.GetProfileMatchLabels(spendProfileMatchId);


            CampaignProfileMobileFormModel mobileModel = new CampaignProfileMobileFormModel(countryId, contractTypeProfileLabel.ToList(),
                                                                        spendProfileLabel.ToList());


            return mobileModel;

        }


        public async Task<CampaignProfileMobileFormModel> GetMobileData(int profileId, CampaignProfilePreference CampaignProfileMobile)
        {
            var campaignProfile = await _campaignDAL.GetCampaignProfileDetail(profileId);
            CampaignProfileMobileFormModel model = await GetMobileModel(campaignProfile.CountryId.Value);

            int CountryId = campaignProfile.CountryId.Value;

            if (campaignProfile != null)
            {
                if (CampaignProfileMobile.CountryId == 0)
                {
                    CampaignProfileMobile.CountryId = CountryId;
                }

                if (CampaignProfileMobile.ContractType_Mobile != null && CampaignProfileMobile.ContractType_Mobile.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileMobile.ContractType_Mobile.Length; i++)
                    {
                        model.ContractTypeQuestion.Find(x => x.QuestionValue == CampaignProfileMobile.ContractType_Mobile.Substring(i, 1)).Selected = true;
                    }
                }


                if (CampaignProfileMobile.Spend_Mobile != null && CampaignProfileMobile.Spend_Mobile.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileMobile.Spend_Mobile.Length; i++)
                    {
                        model.SpendQuestion.Find(x => x.QuestionValue == CampaignProfileMobile.Spend_Mobile.Substring(i, 1)).Selected = true;
                    }
                }


                model.CampaignProfileMobileId = CampaignProfileMobile.Id;
                model.CampaignProfileId = profileId;

            }


            return model;
        }


        public async Task<bool> SaveMobileWizard(CampaignProfileMobileFormModel model, List<string> connString)
        {


            var command = new CreateOrUpdateCampaignProfileMobileCommand();

            command.CampaignProfileId = model.CampaignProfileId;

            command.ContractType_Mobile = CompileAnswers(SortList(model.ContractTypeQuestion));
            command.Spend_Mobile = CompileAnswers(SortList(model.SpendQuestion));

            var prefId = await _matchDAL.UpdateMobileProfile(command, connString);

            var result2 = await _matchDAL.UpdateMatchCampaignMobile(command, connString);

            return true;   
        }



        #endregion



        #region Questionnaire


        public async Task<CampaignProfileSkizaFormModel> GetQuestionnaireModel(int countryId, int profileId = 0)
        {
            var hustlersProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Hustlers".ToLower());
            var hustlersProfileLabel = await _matchDAL.GetProfileMatchLabels(hustlersProfileMatchId);

            var youthProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Youth".ToLower());
            var youthProfileLabel = await _matchDAL.GetProfileMatchLabels(youthProfileMatchId);

            var discerningProfessionalsProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Discerning Professionals".ToLower());
            var discerningProfessionalsProfileLabel = await _matchDAL.GetProfileMatchLabels(discerningProfessionalsProfileMatchId);

            var massProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Mass".ToLower());
            var massProfileLabel = await _matchDAL.GetProfileMatchLabels(massProfileMatchId);


            CampaignProfileSkizaFormModel skizaModel = new CampaignProfileSkizaFormModel(countryId, hustlersProfileLabel.ToList(),
                                                                        youthProfileLabel.ToList(), discerningProfessionalsProfileLabel.ToList(),
                                                                        massProfileLabel.ToList());

            return skizaModel;
        }


        public async Task<CampaignProfileSkizaFormModel> GetQuestionnaireData(int profileId, CampaignProfilePreference CampaignProfileSkiza)
        {
            var campaignProfile = await _campaignDAL.GetCampaignProfileDetail(profileId);
            CampaignProfileSkizaFormModel model = await GetQuestionnaireModel(campaignProfile.CountryId.Value);

            int CountryId = campaignProfile.CountryId.Value;

            if (campaignProfile != null)
            {
                if (CampaignProfileSkiza.CountryId == 0)
                {
                    CampaignProfileSkiza.CountryId = CountryId;
                }

                if (CampaignProfileSkiza.Hustlers_AdType != null && CampaignProfileSkiza.Hustlers_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileSkiza.Hustlers_AdType.Length; i++)
                    {
                        model.HustlersQuestion.Find(x => x.QuestionValue == CampaignProfileSkiza.Hustlers_AdType.Substring(i, 1)).Selected = true;
                    }
                }


                if (CampaignProfileSkiza.Youth_AdType != null && CampaignProfileSkiza.Youth_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileSkiza.Youth_AdType.Length; i++)
                    {
                        model.YouthQuestion.Find(x => x.QuestionValue == CampaignProfileSkiza.Youth_AdType.Substring(i, 1)).Selected = true;
                    }
                }


                if (CampaignProfileSkiza.DiscerningProfessionals_AdType != null && CampaignProfileSkiza.DiscerningProfessionals_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileSkiza.DiscerningProfessionals_AdType.Length; i++)
                    {
                        model.DiscerningProfessionalsQuestion.Find(x => x.QuestionValue == CampaignProfileSkiza.DiscerningProfessionals_AdType.Substring(i, 1)).Selected = true;
                    }
                }


                if (CampaignProfileSkiza.Mass_AdType != null && CampaignProfileSkiza.Mass_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileSkiza.Mass_AdType.Length; i++)
                    {
                        model.MassQuestion.Find(x => x.QuestionValue == CampaignProfileSkiza.Mass_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                model.CampaignProfileSKizaId = CampaignProfileSkiza.Id;
                model.CampaignProfileId = profileId;
            }

            return model;
        }


        public async Task<bool> SaveQuestionnaireWizard(CampaignProfileSkizaFormModel model, List<string> connString)
        {
            var command = new CreateOrUpdateCampaignProfileSkizaCommand();

            command.CampaignProfileId = model.CampaignProfileId;

            command.DiscerningProfessionals_AdType = CompileAnswers(SortList(model.DiscerningProfessionalsQuestion));
            command.Hustlers_AdType = CompileAnswers(SortList(model.HustlersQuestion));
            command.Mass_AdType = CompileAnswers(SortList(model.MassQuestion));
            command.Youth_AdType = CompileAnswers(SortList(model.YouthQuestion));

            var prefId = await _matchDAL.UpdateQuestionnaireProfile(command, connString);

            var result2 = await _matchDAL.UpdateMatchCampaignQuestionnaire(command, connString);

            return true;
        }


        #endregion



        #region AdvertProfile


        public async Task<CampaignProfileAdvertFormModel> GetAdvertProfileModel(int countryId, int profileId = 0)
        {
            var foodProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Food".ToLower());
            var foodProfileLabel = await _matchDAL.GetProfileMatchLabels(foodProfileMatchId);

            var sweetSaltySnacksProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Sweets/Snacks".ToLower());
            var sweetSaltySnacksProfileLabel = await _matchDAL.GetProfileMatchLabels(sweetSaltySnacksProfileMatchId);

            var alcoholicDrinksProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Alcoholic Drinks".ToLower());
            var alcoholicDrinksProfileLabel = await _matchDAL.GetProfileMatchLabels(alcoholicDrinksProfileMatchId);

            var nonAlcoholicDrinksProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Non-Alcoholic Drinks".ToLower());
            var nonAlcoholicDrinksProfileLabel = await _matchDAL.GetProfileMatchLabels(nonAlcoholicDrinksProfileMatchId);

            var householdproductsProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Household Appliances/Products".ToLower());
            var householdproductsProfileLabel = await _matchDAL.GetProfileMatchLabels(householdproductsProfileMatchId);

            var toiletriesCosmeticsProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Toiletries/Cosmetics".ToLower());
            var toiletriesCosmeticsProfileLabel = await _matchDAL.GetProfileMatchLabels(toiletriesCosmeticsProfileMatchId);

            var pharmaceuticalChemistsProductsProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Pharmaceutical/Chemists Products".ToLower());
            var pharmaceuticalChemistsProductsProfileLabel = await _matchDAL.GetProfileMatchLabels(pharmaceuticalChemistsProductsProfileMatchId);

            var tobaccoProductsProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Tobacco Products".ToLower());
            var tobaccoProductsProfileLabel = await _matchDAL.GetProfileMatchLabels(tobaccoProductsProfileMatchId);

            var petsPetFoodProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Pets".ToLower());
            var petsPetFoodProfileLabel = await _matchDAL.GetProfileMatchLabels(petsPetFoodProfileMatchId);

            var shoppingRetailClothingProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Clothing/Fashion".ToLower());
            var shoppingRetailClothingProfileLabel = await _matchDAL.GetProfileMatchLabels(shoppingRetailClothingProfileMatchId);

            var dIYGardeningProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "DIY/Gardening".ToLower());
            var dIYGardeningProfileLabel = await _matchDAL.GetProfileMatchLabels(dIYGardeningProfileMatchId);

            var electronicsOtherPersonalItemsProfileMatchId = await _matchDAL.GetProfileMatchInformationId(countryId, "Electronics/Other Personal Items".ToLower());
            var electronicsOtherPersonalItemsProfileLabel = await _matchDAL.GetProfileMatchLabels(electronicsOtherPersonalItemsProfileMatchId);

            var communicationsInternetProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Communications/Internet Telecom".ToLower()).Result;
            var communicationsInternetProfileLabel = _matchDAL.GetProfileMatchLabels(communicationsInternetProfileMatchId).Result;

            var financialServicesProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Financial Services".ToLower()).Result;
            var financialServicesProfileLabel = _matchDAL.GetProfileMatchLabels(financialServicesProfileMatchId).Result;

            var holidaysTravelProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Holidays/Travel Tourism".ToLower()).Result;
            var holidaysTravelProfileLabel = _matchDAL.GetProfileMatchLabels(holidaysTravelProfileMatchId).Result;

            var sportsLeisureProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Sports/Leisure".ToLower()).Result;
            var sportsLeisureProfileLabel = _matchDAL.GetProfileMatchLabels(sportsLeisureProfileMatchId).Result;

            var motoringProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Motoring/Automotive".ToLower()).Result;
            var motoringProfileLabel = _matchDAL.GetProfileMatchLabels(motoringProfileMatchId).Result;

            var newspapersProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Newspapers/Magazines".ToLower()).Result;
            var newspapersProfileLabel = _matchDAL.GetProfileMatchLabels(newspapersProfileMatchId).Result;

            var tVProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "TV/Video/ Radio".ToLower()).Result;
            var tVProfileLabel = _matchDAL.GetProfileMatchLabels(tVProfileMatchId).Result;

            var cinemaProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "cinema").Result;
            var cinemaProfileLabel = _matchDAL.GetProfileMatchLabels(cinemaProfileMatchId).Result;

            var socialNetworkingProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Social Networking".ToLower()).Result;
            var socialNetworkingProfileLabel = _matchDAL.GetProfileMatchLabels(socialNetworkingProfileMatchId).Result;

            var shoppingProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Shopping(retail gen merc)".ToLower()).Result;
            var shoppingProfileLabel = _matchDAL.GetProfileMatchLabels(shoppingProfileMatchId).Result;

            var fitnessProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Fitness".ToLower()).Result;
            var fitnessProfileLabel = _matchDAL.GetProfileMatchLabels(fitnessProfileMatchId).Result;

            var environmentProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Environment".ToLower()).Result;
            var environmentProfileLabel = _matchDAL.GetProfileMatchLabels(environmentProfileMatchId).Result;

            var goingOutProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Going Out/Entertainment".ToLower()).Result;
            var goingOutProfileLabel = _matchDAL.GetProfileMatchLabels(goingOutProfileMatchId).Result;

            var religionProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Religion".ToLower()).Result;
            var religionProfileLabel = _matchDAL.GetProfileMatchLabels(religionProfileMatchId).Result;

            var musicProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Music".ToLower()).Result;
            var musicProfileLabel = _matchDAL.GetProfileMatchLabels(musicProfileMatchId).Result;

            var businessOrOpportunitiesProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Business/opportunities".ToLower()).Result;
            var businessOrOpportunitiesProfileLabel = _matchDAL.GetProfileMatchLabels(businessOrOpportunitiesProfileMatchId).Result;

            var gamblingProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Over 18/Gambling".ToLower()).Result;
            var gamblingProfileLabel = _matchDAL.GetProfileMatchLabels(gamblingProfileMatchId).Result;

            var restaurantsProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Restaurants".ToLower()).Result;
            var restaurantsProfileLabel = _matchDAL.GetProfileMatchLabels(restaurantsProfileMatchId).Result;

            var insuranceProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Insurance".ToLower()).Result;
            var insuranceProfileLabel = _matchDAL.GetProfileMatchLabels(insuranceProfileMatchId).Result;

            var furnitureProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Furniture".ToLower()).Result;
            var furnitureProfileLabel = _matchDAL.GetProfileMatchLabels(furnitureProfileMatchId).Result;

            var informationTechnologyProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Information technology".ToLower()).Result;
            var informationTechnologyProfileLabel = _matchDAL.GetProfileMatchLabels(informationTechnologyProfileMatchId).Result;

            var energyProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "energy").Result;
            var energyProfileLabel = _matchDAL.GetProfileMatchLabels(energyProfileMatchId).Result;

            var supermarketsProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "supermarkets").Result;
            var supermarketsProfileLabel = _matchDAL.GetProfileMatchLabels(supermarketsProfileMatchId).Result;

            var healthcareProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Healthcare".ToLower()).Result;
            var healthcareProfileLabel = _matchDAL.GetProfileMatchLabels(healthcareProfileMatchId).Result;

            var jobsAndEducationProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Jobs and Education".ToLower()).Result;
            var jobsAndEducationProfileLabel = _matchDAL.GetProfileMatchLabels(jobsAndEducationProfileMatchId).Result;

            var giftsProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Gifts".ToLower()).Result;
            var giftsProfileLabel = _matchDAL.GetProfileMatchLabels(giftsProfileMatchId).Result;

            var advocacyOrLegalProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Advocacy/Legal".ToLower()).Result;
            var advocacyOrLegalProfileLabel = _matchDAL.GetProfileMatchLabels(advocacyOrLegalProfileMatchId).Result;

            var datingAndPersonalProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Dating & Personal".ToLower()).Result;
            var datingAndPersonalProfileLabel = _matchDAL.GetProfileMatchLabels(datingAndPersonalProfileMatchId).Result;

            var realEstateProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Real Estate / Property".ToLower()).Result;
            var realEstateProfileLabel = _matchDAL.GetProfileMatchLabels(realEstateProfileMatchId).Result;

            var gamesProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "Games".ToLower()).Result;
            var gamesProfileLabel = _matchDAL.GetProfileMatchLabels(gamesProfileMatchId).Result;


            CampaignProfileAdvertFormModel adModel = new CampaignProfileAdvertFormModel(countryId, foodProfileLabel.ToList(), sweetSaltySnacksProfileLabel.ToList(),
                                                                                          alcoholicDrinksProfileLabel.ToList(), nonAlcoholicDrinksProfileLabel.ToList(),
                                                                                          householdproductsProfileLabel.ToList(), toiletriesCosmeticsProfileLabel.ToList(),
                                                                                          pharmaceuticalChemistsProductsProfileLabel.ToList(), tobaccoProductsProfileLabel.ToList(),
                                                                                          petsPetFoodProfileLabel.ToList(), shoppingRetailClothingProfileLabel.ToList(),
                                                                                          dIYGardeningProfileLabel.ToList(), electronicsOtherPersonalItemsProfileLabel.ToList(),
                                                                                          communicationsInternetProfileLabel.ToList(), financialServicesProfileLabel.ToList(),
                                                                                          holidaysTravelProfileLabel.ToList(), sportsLeisureProfileLabel.ToList(),
                                                                                          motoringProfileLabel.ToList(), newspapersProfileLabel.ToList(),
                                                                                          tVProfileLabel.ToList(), cinemaProfileLabel.ToList(),
                                                                                          socialNetworkingProfileLabel.ToList(), shoppingProfileLabel.ToList(),
                                                                                          fitnessProfileLabel.ToList(), environmentProfileLabel.ToList(),
                                                                                          goingOutProfileLabel.ToList(), religionProfileLabel.ToList(),
                                                                                          musicProfileLabel.ToList(), businessOrOpportunitiesProfileLabel.ToList(),
                                                                                          gamblingProfileLabel.ToList(), restaurantsProfileLabel.ToList(),
                                                                                          insuranceProfileLabel.ToList(), furnitureProfileLabel.ToList(),
                                                                                          informationTechnologyProfileLabel.ToList(), energyProfileLabel.ToList(),
                                                                                          supermarketsProfileLabel.ToList(), healthcareProfileLabel.ToList(),
                                                                                          jobsAndEducationProfileLabel.ToList(), giftsProfileLabel.ToList(),
                                                                                          advocacyOrLegalProfileLabel.ToList(), datingAndPersonalProfileLabel.ToList(),
                                                                                          realEstateProfileLabel.ToList(), gamesProfileLabel.ToList());



            return adModel;
        }


        public async Task<CampaignProfileAdvertFormModel> GetAdvertProfileData(int profileId, CampaignProfilePreference CampaignProfileAdvert)
        {
            var campaignProfile = await _campaignDAL.GetCampaignProfileDetail(profileId);
            CampaignProfileAdvertFormModel model = await GetAdvertProfileModel(campaignProfile.CountryId.Value);

            int CountryId = campaignProfile.CountryId.Value;

            if (campaignProfile != null)
            {
                if (CampaignProfileAdvert.CountryId == 0)
                {
                    CampaignProfileAdvert.CountryId = CountryId;
                }

                if (CampaignProfileAdvert.Food_Advert != null && CampaignProfileAdvert.Food_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Food_Advert.Length; i++)
                    {
                        model.FoodQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Food_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.SweetSaltySnacks_Advert != null && CampaignProfileAdvert.SweetSaltySnacks_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.SweetSaltySnacks_Advert.Length; i++)
                    {
                        model.SweetSaltySnacksQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.SweetSaltySnacks_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.AlcoholicDrinks_Advert != null && CampaignProfileAdvert.AlcoholicDrinks_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.AlcoholicDrinks_Advert.Length; i++)
                    {
                        model.AlcoholicDrinksQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.AlcoholicDrinks_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.NonAlcoholicDrinks_Advert != null && CampaignProfileAdvert.NonAlcoholicDrinks_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.NonAlcoholicDrinks_Advert.Length; i++)
                    {
                        model.NonAlcoholicDrinksQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.NonAlcoholicDrinks_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Householdproducts_Advert != null && CampaignProfileAdvert.Householdproducts_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Householdproducts_Advert.Length; i++)
                    {
                        model.HouseholdproductsQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Householdproducts_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.ToiletriesCosmetics_Advert != null && CampaignProfileAdvert.ToiletriesCosmetics_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.ToiletriesCosmetics_Advert.Length; i++)
                    {
                        model.ToiletriesCosmeticsQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.ToiletriesCosmetics_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.PharmaceuticalChemistsProducts_Advert != null && CampaignProfileAdvert.PharmaceuticalChemistsProducts_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.PharmaceuticalChemistsProducts_Advert.Length; i++)
                    {
                        model.PharmaceuticalChemistsProductsQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.PharmaceuticalChemistsProducts_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.TobaccoProducts_Advert != null && CampaignProfileAdvert.TobaccoProducts_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.TobaccoProducts_Advert.Length; i++)
                    {
                        model.TobaccoProductsQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.TobaccoProducts_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.PetsPetFood_Advert != null && CampaignProfileAdvert.PetsPetFood_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.PetsPetFood_Advert.Length; i++)
                    {
                        model.PetsPetFoodQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.PetsPetFood_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.ShoppingRetailClothing_Advert != null && CampaignProfileAdvert.ShoppingRetailClothing_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.ShoppingRetailClothing_Advert.Length; i++)
                    {
                        model.ShoppingRetailClothingQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.ShoppingRetailClothing_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.DIYGardening_Advert != null && CampaignProfileAdvert.DIYGardening_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.DIYGardening_Advert.Length; i++)
                    {
                        model.DIYGardeningQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.DIYGardening_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.ElectronicsOtherPersonalItems_Advert != null && CampaignProfileAdvert.ElectronicsOtherPersonalItems_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.ElectronicsOtherPersonalItems_Advert.Length; i++)
                    {
                        model.ElectronicsOtherPersonalItemsQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.ElectronicsOtherPersonalItems_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.CommunicationsInternet_Advert != null && CampaignProfileAdvert.CommunicationsInternet_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.CommunicationsInternet_Advert.Length; i++)
                    {
                        model.CommunicationsInternetQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.CommunicationsInternet_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.FinancialServices_Advert != null && CampaignProfileAdvert.FinancialServices_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.FinancialServices_Advert.Length; i++)
                    {
                        model.FinancialServicesQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.FinancialServices_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.HolidaysTravel_Advert != null && CampaignProfileAdvert.HolidaysTravel_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.HolidaysTravel_Advert.Length; i++)
                    {
                        model.HolidaysTravelQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.HolidaysTravel_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.SportsLeisure_Advert != null && CampaignProfileAdvert.SportsLeisure_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.SportsLeisure_Advert.Length; i++)
                    {
                        model.SportsLeisureQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.SportsLeisure_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Motoring_Advert != null && CampaignProfileAdvert.Motoring_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Motoring_Advert.Length; i++)
                    {
                        model.MotoringQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Motoring_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Newspapers_Advert != null && CampaignProfileAdvert.Newspapers_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Newspapers_Advert.Length; i++)
                    {
                        model.NewspapersQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Newspapers_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.TV_Advert != null && CampaignProfileAdvert.TV_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.TV_Advert.Length; i++)
                    {
                        model.TVQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.TV_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Cinema_Advert != null && CampaignProfileAdvert.Cinema_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Cinema_Advert.Length; i++)
                    {
                        model.CinemaQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Cinema_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.SocialNetworking_Advert != null && CampaignProfileAdvert.SocialNetworking_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.SocialNetworking_Advert.Length; i++)
                    {
                        model.SocialNetworkingQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.SocialNetworking_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Shopping_Advert != null && CampaignProfileAdvert.Shopping_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Shopping_Advert.Length; i++)
                    {
                        model.ShoppingQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Shopping_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Fitness_Advert != null && CampaignProfileAdvert.Fitness_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Fitness_Advert.Length; i++)
                    {
                        model.FitnessQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Fitness_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Environment_Advert != null && CampaignProfileAdvert.Environment_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Environment_Advert.Length; i++)
                    {
                        model.EnvironmentQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Environment_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.GoingOut_Advert != null && CampaignProfileAdvert.GoingOut_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.GoingOut_Advert.Length; i++)
                    {
                        model.GoingOutQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.GoingOut_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Religion_Advert != null && CampaignProfileAdvert.Religion_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Religion_Advert.Length; i++)
                    {
                        model.ReligionQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Religion_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Music_Advert != null && CampaignProfileAdvert.Music_Advert.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Music_Advert.Length; i++)
                    {
                        model.MusicQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Music_Advert.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.BusinessOrOpportunities_AdType != null && CampaignProfileAdvert.BusinessOrOpportunities_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.BusinessOrOpportunities_AdType.Length; i++)
                    {
                        model.BusinessOrOpportunitiesQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.BusinessOrOpportunities_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Gambling_AdType != null && CampaignProfileAdvert.Gambling_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Gambling_AdType.Length; i++)
                    {
                        model.GamblingQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Gambling_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Restaurants_AdType != null && CampaignProfileAdvert.Restaurants_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Restaurants_AdType.Length; i++)
                    {
                        model.RestaurantsQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Restaurants_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Insurance_AdType != null && CampaignProfileAdvert.Insurance_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Insurance_AdType.Length; i++)
                    {
                        model.InsuranceQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Insurance_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Furniture_AdType != null && CampaignProfileAdvert.Furniture_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Furniture_AdType.Length; i++)
                    {
                        model.FurnitureQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Furniture_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.InformationTechnology_AdType != null && CampaignProfileAdvert.InformationTechnology_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.InformationTechnology_AdType.Length; i++)
                    {
                        model.InformationTechnologyQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.InformationTechnology_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Energy_AdType != null && CampaignProfileAdvert.Energy_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Energy_AdType.Length; i++)
                    {
                        model.EnergyQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Energy_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Supermarkets_AdType != null && CampaignProfileAdvert.Supermarkets_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Supermarkets_AdType.Length; i++)
                    {
                        model.SupermarketsQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Supermarkets_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Healthcare_AdType != null && CampaignProfileAdvert.Healthcare_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Healthcare_AdType.Length; i++)
                    {
                        model.HealthcareQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Healthcare_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.JobsAndEducation_AdType != null && CampaignProfileAdvert.JobsAndEducation_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.JobsAndEducation_AdType.Length; i++)
                    {
                        model.JobsAndEducationQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.JobsAndEducation_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.Gifts_AdType != null && CampaignProfileAdvert.Gifts_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Gifts_AdType.Length; i++)
                    {
                        model.GiftsQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Gifts_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.AdvocacyOrLegal_AdType != null && CampaignProfileAdvert.AdvocacyOrLegal_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.AdvocacyOrLegal_AdType.Length; i++)
                    {
                        model.AdvocacyOrLegalQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.AdvocacyOrLegal_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                if (CampaignProfileAdvert.DatingAndPersonal_AdType != null && CampaignProfileAdvert.DatingAndPersonal_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.DatingAndPersonal_AdType.Length; i++)
                    {
                        model.DatingAndPersonalQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.DatingAndPersonal_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                //if (CampaignProfileAdvert.RealEstate_AdType != null && CampaignProfileAdvert.RealEstate_AdType.Length > 0)
                //{
                //    for (int i = 0; i < CampaignProfileAdvert.RealEstate_AdType.Length; i++)
                //    {
                //        model.RealEstateQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.RealEstate_AdType.Substring(i, 1)).Selected = true;
                //    }
                //}

                if (CampaignProfileAdvert.Games_AdType != null && CampaignProfileAdvert.Games_AdType.Length > 0)
                {
                    for (int i = 0; i < CampaignProfileAdvert.Games_AdType.Length; i++)
                    {
                        model.GamesQuestion.Find(x => x.QuestionValue == CampaignProfileAdvert.Games_AdType.Substring(i, 1)).Selected = true;
                    }
                }

                

                model.CampaignProfileAdvertsId = CampaignProfileAdvert.Id;
                model.CampaignProfileId = profileId;
            }

            return model;
        }


        public async Task<bool> SaveAdvertsWizard(CampaignProfileAdvertFormModel model, List<string> connString)
        {
            var command = new CreateOrUpdateCampaignProfileAdvertCommand();
            command.CampaignProfileId = model.CampaignProfileId;
            command.AdvocacyOrLegal_AdType = CompileAnswers(SortList(model.AdvocacyOrLegalQuestion));
            command.AlcoholicDrinks_Advert = CompileAnswers(SortList(model.AlcoholicDrinksQuestion));
            command.BusinessOrOpportunities_AdType = CompileAnswers(SortList(model.BusinessOrOpportunitiesQuestion));
            command.Cinema_Advert = CompileAnswers(SortList(model.CinemaQuestion));
            command.CommunicationsInternet_Advert = CompileAnswers(SortList(model.CommunicationsInternetQuestion));
            command.DatingAndPersonal_AdType = CompileAnswers(SortList(model.DatingAndPersonalQuestion));
            // command.DiscerningProfessionals_AdType = CompileAnswers(SortList(model.DiscerningProfessionalsQuestion));
            command.DIYGardening_Advert = CompileAnswers(SortList(model.DIYGardeningQuestion));
            command.ElectronicsOtherPersonalItems_Advert = CompileAnswers(SortList(model.ElectronicsOtherPersonalItemsQuestion));
            command.Energy_AdType = CompileAnswers(SortList(model.EnergyQuestion));
            command.Environment_Advert = CompileAnswers(SortList(model.EnvironmentQuestion));
            command.FinancialServices_Advert = CompileAnswers(SortList(model.FinancialServicesQuestion));
            command.Fitness_Advert = CompileAnswers(SortList(model.FitnessQuestion));
            command.Food_Advert = CompileAnswers(SortList(model.FoodQuestion));
            command.Furniture_AdType = CompileAnswers(SortList(model.FurnitureQuestion));
            command.Gambling_AdType = CompileAnswers(SortList(model.GamblingQuestion));
            command.Games_AdType = CompileAnswers(SortList(model.GamesQuestion));
            command.Gifts_AdType = CompileAnswers(SortList(model.GiftsQuestion));
            command.GoingOut_Advert = CompileAnswers(SortList(model.GoingOutQuestion));
            command.Healthcare_AdType = CompileAnswers(SortList(model.HealthcareQuestion));
            command.HolidaysTravel_Advert = CompileAnswers(SortList(model.HolidaysTravelQuestion));
            command.Householdproducts_Advert = CompileAnswers(SortList(model.HouseholdproductsQuestion));
            command.InformationTechnology_AdType = CompileAnswers(SortList(model.InformationTechnologyQuestion));
            command.Insurance_AdType = CompileAnswers(SortList(model.InsuranceQuestion));
            command.JobsAndEducation_AdType = CompileAnswers(SortList(model.JobsAndEducationQuestion));
            command.Motoring_Advert = CompileAnswers(SortList(model.MotoringQuestion));
            command.Music_Advert = CompileAnswers(SortList(model.MusicQuestion));
            command.Newspapers_Advert = CompileAnswers(SortList(model.NewspapersQuestion));
            command.NonAlcoholicDrinks_Advert = CompileAnswers(SortList(model.NonAlcoholicDrinksQuestion));
            command.PetsPetFood_Advert = CompileAnswers(SortList(model.PetsPetFoodQuestion));
            command.PharmaceuticalChemistsProducts_Advert = CompileAnswers(SortList(model.PharmaceuticalChemistsProductsQuestion));
            command.RealEstate_AdType = CompileAnswers(SortList(model.RealEstateQuestion));
            command.Religion_Advert = CompileAnswers(SortList(model.ReligionQuestion));
            command.Restaurants_AdType = CompileAnswers(SortList(model.RestaurantsQuestion));
            command.ShoppingRetailClothing_Advert = CompileAnswers(SortList(model.ShoppingRetailClothingQuestion));
            command.Shopping_Advert = CompileAnswers(SortList(model.ShoppingQuestion));
            command.SocialNetworking_Advert = CompileAnswers(SortList(model.SocialNetworkingQuestion));
            command.SportsLeisure_Advert = CompileAnswers(SortList(model.SportsLeisureQuestion));
            command.Supermarkets_AdType = CompileAnswers(SortList(model.SupermarketsQuestion));
            command.SweetSaltySnacks_Advert = CompileAnswers(SortList(model.SweetSaltySnacksQuestion));
            command.TobaccoProducts_Advert = CompileAnswers(SortList(model.TobaccoProductsQuestion));
            command.ToiletriesCosmetics_Advert = CompileAnswers(SortList(model.ToiletriesCosmeticsQuestion));
            command.TV_Advert = CompileAnswers(SortList(model.TVQuestion));
            // command.Youth_AdType = CompileAnswers(SortList(model.YouthQuestion));

            var prefId = await _matchDAL.UpdateAdvertProfile(command, connString);

            var result2 = await _matchDAL.UpdateMatchCampaignAdvert(command, connString);

            return true;
        }



        #endregion


        /// <summary>
        /// Sorts the list.
        /// </summary>
        /// <param name="questionOptions">The question options.</param>
        /// <returns>IEnumerable&lt;QuestionOptionModel&gt;.</returns>
        internal IEnumerable<QuestionOptionModel> SortList(List<QuestionOptionModel> questionOptions)
        {
            questionOptions.Sort((x, y) => String.Compare(x.QuestionValue, y.QuestionValue, StringComparison.Ordinal));
            return questionOptions;
        }

        /// <summary>
        /// Compiles the answers.
        /// </summary>
        /// <param name="questionOptions">The question options.</param>
        /// <returns>System.String.</returns>
        internal string CompileAnswers(IEnumerable<QuestionOptionModel> questionOptions)
        {
            IEnumerable<QuestionOptionModel> questionOptionModels = questionOptions as QuestionOptionModel[] ??
                                                                    questionOptions.ToArray();

            string answers = questionOptionModels.Where(q => q.Selected).Aggregate(
                string.Empty,
                (current, q) => current + q.QuestionValue
                );

            if (string.IsNullOrEmpty(answers))
            {
                foreach (
                    QuestionOptionModel questionOptionModel in
                        questionOptionModels.Where(questionOptionModel => questionOptionModel.DefaultAnswer))
                    answers = questionOptionModel.QuestionValue;
            }

            return answers;
        }

    }
}
