using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Enums;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.DTOs.UserProfile;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class UserProfileService : IUserProfileService
    {
        private ILoggingService _logServ;
        private readonly ICampaignDAL _campService;
        private readonly IProfileMatchInfoDAL _profDAL;
        private readonly IUserManagementDAL _userDAL;
        private readonly IUserProfileDAL _uPrileDAL;
        private readonly IUserMatchDAL _matchDAL;
        ReturnResult result = new ReturnResult();
        const string PageName = "UserProfileService";

        public UserProfileService(ICampaignDAL campService, ILoggingService logServ, IProfileMatchInfoDAL profDAL,
                                    IUserManagementDAL userDAL, IUserProfileDAL uPrileDAL, IUserMatchDAL matchDAL)
        {
            _logServ = logServ;
            _campService = campService;
            _profDAL = profDAL;
            _userDAL = userDAL;
            _uPrileDAL = uPrileDAL;
            _matchDAL = matchDAL;
        }

        public async Task<ReturnResult> GetUserProfile(int userId)
        {
            try
            {
                result.body = await _uPrileDAL.GetUserProfileByUserId(userId);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetUserProfile";
                await _logServ.LogError();

                result.result = 0;
                result.body = ex.Message.ToString();
                return result;
            }
            return result;
        }

        public async Task<ReturnResult> GetUserProfilePreference(int id, int? campaignId)
        {
            var campaignProfile = await _campService.GetCampaignProfileDetail(campaignId.Value);

            var profileMatchInformation = await _profDAL.GetProfileMatchInformation(campaignProfile.CountryId.Value);
            var _profileMatchInformation = profileMatchInformation.AsQueryable();
            var countryId = campaignProfile.CountryId.Value;
            var operatorId = await _userDAL.GetOperatorIdByUserId(id);

            UserProfileDto userProfile = await _uPrileDAL.GetUserProfileByUserId(id);
            userProfile.UserProfilePreferences = await _uPrileDAL.GetUserProfilePreferenceByUserProfileId(userProfile.UserProfileId);
            var campPreferences = await _matchDAL.GetCampaignProfilePreferenceDetailsByCampaignId(campaignId.Value);
            var profileMatchValue = new ProfileMatchRetrivalService();
            var profileDisplay = new UserProfileDisplayDto();
            var demographicDisplay = new UserProfileDemographicsDto();

            var userid = Convert.ToInt32(id);

            if (userProfile != null)
            {
                bool isQuestionnaire = false;
                if (operatorId == (int)OperatorTableId.Safaricom)
                {
                    isQuestionnaire = true;
                }

                if (userProfile.UserProfilePreferences != null)
                {

                    //Skiza Profile
                    if (isQuestionnaire)
                        profileDisplay.SkizaProfileDto = GetSkizaData(_profileMatchInformation, campPreferences);
                    demographicDisplay.Gender_Demographics = GetGenderData(userProfile, campPreferences, _profileMatchInformation);
                    demographicDisplay.Location_Demographics = GetLocationData(userProfile, campPreferences, _profileMatchInformation);

                    int age = 0;
                    if (userProfile.DOB.ToString() != null)
                    {
                        DateTime dob = (DateTime)userProfile.DOB;
                        DateTime now = DateTime.Today;
                        age = now.Year - dob.Year;
                    }
                    demographicDisplay.Age = GetAgeData(campPreferences, age).ToString();
                    demographicDisplay.Age_Demographics = demographicDisplay.Age;

                    profileDisplay.UserProfileDemographicsDto = demographicDisplay;

                    result.body = profileDisplay;
                    return result;
                }
                else
                    result.result = 0;
            }
            else
                result.result = 0;
            return result;
        }


        private string GetLabelString(string letter, string matchDescription, IQueryable<ProfileMatchInformationFormModel> matchInfo)
        {
            var profileLabelService = new ProfileMatchRetrivalService();
            var profileMatchId = matchInfo.Where(top => top.ProfileName.ToLower().Equals(matchDescription.ToLower())).Select(top => top.Id).FirstOrDefault();
            if (profileMatchId == 0)
            {
                return "-";
            }
            else
            {
                int labelId = profileLabelService.GetProfileMatchValue(letter);
                var profileLabelList = _profDAL.GetListProfileLabelById(profileMatchId).Result.AsQueryable();
                var profileLabel = profileLabelList.Skip(labelId).Take(1).FirstOrDefault();
                return profileLabel.ProfileLabel;
            }
        }

        public string GetLocationData(UserProfileDto userProfileDemographic, CampaignProfilePreference campPreferences, IQueryable<ProfileMatchInformationFormModel> matchInfo)
        {
            string letter = string.Empty;
            var advertData = campPreferences.Location_Demographics;
            var userLocation = userProfileDemographic.UserProfilePreferences.Location_Demographics;

            if (userLocation == null || advertData.IndexOf(userLocation) == -1)
                letter = advertData.Substring(0, 1);
            else
                letter = userLocation;

            return GetLabelString(letter,"Location", matchInfo);
        }

        private string GetGenderData(UserProfileDto userProfileDemographic, CampaignProfilePreference campPreferences, IQueryable<ProfileMatchInformationFormModel> matchInfo)
        {
            var letter = string.Empty;
            var advertData = campPreferences.Gender_Demographics;
            var userGender = userProfileDemographic.UserProfilePreferences.Gender_Demographics;
            if ((advertData == null || advertData.Length == 0) && userGender == null)
                letter = "A";
            else if (advertData == null || advertData.Length == 0)
                letter = userGender;
            else if (userGender == null || advertData.IndexOf(userGender) == -1)
                letter = advertData.Substring(0, 1);
            else
                letter = userGender;

            return GetLabelString(letter, "Gender", matchInfo);
        }

        private int GetAgeData(CampaignProfilePreference campPreferences, int age)
        {
            var advertData = campPreferences.Age_Demographics;
            var ageArray = advertData.Select(x => new string(x, 1)).ToArray();
            if (ageArray.Count() > 7)
                return age;
            else if (advertData == null || ageArray.Count() == 0)
                return age;
            else
                return checkAge(age, ageArray);
        }

        private int checkAge(int age, string[] ranges)
        {
            var rnd = new Random();
            var letter = "Z";
            if (age >= 1 && age < 18)
                letter = "A";
            else if (age >= 18 && age < 25)
                letter = "B";
            else if (age >= 25 && age < 35)
                letter = "C";
            else if (age >= 35 && age < 45)
                letter = "D";
            else if (age >= 45 && age < 55)
                letter = "E";
            else if (age >= 55 && age < 65)
                letter = "F";
            else if (age >= 65)
                letter = "G";
            else
                letter = "H";

            if (ranges == null || ranges.Count() == 0)
                return age;
            else if (ranges.Contains(letter))
                return age;
            else if (ranges.Contains("B") && ranges.Contains("C"))
                return rnd.Next(18, 36);
            else if (ranges.Contains("B") && ranges.Contains("C"))
                return rnd.Next(18, 36);
            else if (ranges.Contains("C"))
                return rnd.Next(25, 36);
            else if (ranges.Contains("H"))
                return 0;
            else return 28;
        }

        #region Skiza Data
        private SkizaProfileDto GetSkizaData(IQueryable<ProfileMatchInformationFormModel> matchInfo, CampaignProfilePreference campPreferences)
        {
            var model = new SkizaProfileDto();

            model.Hustlers_AdType = GetHustlersData(campPreferences, matchInfo);
            model.Mass_AdType = GetMassData(campPreferences, matchInfo);
            model.Youth_AdType = GetYouthData(campPreferences, matchInfo);
            model.DiscerningProfessionals_AdType = GetDiscerningData(campPreferences, matchInfo);
            return model;
        }

        public string GetHustlersData(CampaignProfilePreference campPreferences, IQueryable<ProfileMatchInformationFormModel> matchInfo)
        {
            var letter = string.Empty;
            var advertData = campPreferences.Hustlers_AdType;
            if (advertData == null || advertData.Length == 0)
                letter = "B";
            else
                letter = advertData;

            return GetLabelString(letter, "Hustlers", matchInfo);
        }

        public string GetYouthData(CampaignProfilePreference campPreferences, IQueryable<ProfileMatchInformationFormModel> matchInfo)
        {
            var letter = string.Empty;
            var advertData = campPreferences.Youth_AdType;
            if (advertData == null || advertData.Length == 0)
                letter = "C";
            else
                letter = advertData;

            return GetLabelString(letter, "Youth", matchInfo);
        }

        public string GetDiscerningData(CampaignProfilePreference campPreferences, IQueryable<ProfileMatchInformationFormModel> matchInfo)
        {
            var letter = string.Empty;
            var advertData = campPreferences.DiscerningProfessionals_AdType;
            if (advertData == null || advertData.Length == 0)
                letter = "A";
            else
                letter = advertData;

            return GetLabelString(letter, "Discerning Professionals", matchInfo);
        }

        public string GetMassData(CampaignProfilePreference campPreferences, IQueryable<ProfileMatchInformationFormModel> matchInfo)
        {
            var letter = string.Empty;
            var advertData = campPreferences.Mass_AdType;
            if (advertData == null || advertData.Length == 0)
                letter = "E";
            else
                letter = advertData;

            return GetLabelString(letter, "Mass", matchInfo);
        }

        #endregion
    }
}
