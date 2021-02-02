using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Enums;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class ProfileMatchInfoService : IProfileMatchInfoService
    {

        private readonly IConfiguration _configuration;
        private readonly IProfileMatchInfoDAL _profDAL;
        ReturnResult result = new ReturnResult();
        private readonly ILoggingService _logServ;
        const string PageName = "ProfileMatchInfoService";


        public ProfileMatchInfoService(IConfiguration configuration, IProfileMatchInfoDAL profDAL, ILoggingService logServ)

        {
            _configuration = configuration;
            _profDAL = profDAL;
            _logServ = logServ;
        }


        public ReturnResult FillProfileType()
        {
            IEnumerable<Enums.ProfileType> profileTypes = Enum.GetValues(typeof(ProfileType))
                                                     .Cast<Enums.ProfileType>();
            result.body = profileTypes.Select(top => new
            {
                Text = top.ToString(),
                Value = top.ToString()
            }).ToList();
            return result;
        }


        /// <summary>
        /// Gets either a list of Profile Information or a single if passed model Id is not zero
        /// </summary>
        /// <param name="model">The id is used to select a single profile</param>
        /// <returns>Either a List or single ProfileInformationResult model</returns>
        public async Task<ReturnResult> LoadDataTable()
        {
            try
            {

                result.body = await _profDAL.LoadProfileResultSet();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "FillProfileMatchInformationResult";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Populates Form for editing
        /// </summary>
        /// <param name="model">The model.id uses the ProfileMatchInformations Id</param>
        /// <returns></returns>
        public async Task<ReturnResult> GetProfileInfo(int id)
        {
            try
            {
                var prof = new ProfileMatchInformationFormModel();
                IEnumerable<ProfileMatchLabelFormModel> label;
                prof = await _profDAL.GetProfileById(id);
                label = await _profDAL.GetProfileLabelById(prof.Id);

                prof.profileMatchLabelFormModels = label.ToList();
                result.body = prof;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetprofileInfo";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Updates Profile information and also adds or updates labels
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReturnResult> UpdateProfileInfo(ProfileMatchInformationFormModel model)
        {
            try
            {
                var profileMatches = model.profileMatchLabelFormModels;
                if (CheckLabelsUnique(profileMatches))
                {
                    var profileLabelCount = profileMatches.Count;
                    if (profileLabelCount > 0)
                    {
                        var x = await _profDAL.UpdateProfileInfo(model);

                        var y = await UpdateInsertProfileLabel(profileMatches, model.Id);
                        if (y == 0)
                        {
                            result.error = "failed to insert/update label(s)";
                            result.result = 0;
                            return result;
                        }
                    }
                    else
                    {
                        result.error = "Please add atleast one record.";
                        result.result = 0;
                        return result;
                    }
                }
                else
                {
                    result.error = " You have duplicate Match labels.";
                    result.result = 0;
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateProfileInfo";
                await _logServ.LogError();
                
                result.result = 0;
                result.error = "ProfileInfo was NOT added successfully";
            }
            return result;

        }

        /// <summary>
        /// Adds new profile information and also new labels
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReturnResult> AddProfileInfo(ProfileMatchInformationFormModel model)
        {
            if ((model.profileMatchLabelFormModels != null || model.profileMatchLabelFormModels.Count() > 0))
            {
                int profileId = 0;
                try
                {
                    if (await _profDAL.CheckIfProfileInfoExists(model))
                    {
                        result.error = model.ProfileName + " Record Exists.";
                        result.result = 0;
                        return result;
                    }
                    profileId = await _profDAL.AddProfileInfo(model);

                    if (!(profileId > 0))
                    {
                        result.result = 0;
                        result.error = "ProfileInfo was NOT added successfully";
                        return result;
                    }

                    // This could be passed to the UpdateInsertProfileLabel function but as took so few lines put it here.
                    foreach (var label in model.profileMatchLabelFormModels)
                    {
                        label.ProfileMatchInformationId = profileId;
                        var y = await _profDAL.AddProfileInfoLabel(label);
                    }
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "AddProfileInfo";
                    await _logServ.LogError();
                    
                    result.result = 0;
                    result.error = "ProfileInfo was NOT added successfully";
                }
                result.body = profileId;
                return result;
            }
            else
            {
                result.error = "Please add at least one profile label.";
                result.result = 0;
                return result;
            }
        }


        public async Task<ReturnResult> DeleteProfileLabel(int id)
        {
            try
            {
                var x = await _profDAL.DeleteProfileLabelById(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "DeleteProfileLabel";
                await _logServ.LogError();
                
                result.result = 0;
                return result;
            }
            return result;
        }


        /// <summary>
        /// This is used when the Edit/Update Profile is shown. Updates existing and adds any labels added.
        /// </summary>
        /// <param name="labellist"></param>
        /// <param name="profileId">This is to add the ProfilematchInfoId to any new labels</param>
        /// <returns></returns>
        private async Task<int> UpdateInsertProfileLabel(List<ProfileMatchLabelFormModel> labellist, int profileId)
        {
            try
            {
                var x = 0;
                string query = string.Empty;
                foreach (var label in labellist)
                {
                    if (label.Id == 0)
                    {
                        label.ProfileMatchInformationId = profileId;
                        x = await _profDAL.AddProfileInfoLabel(label);
                    }
                    else
                        x = await _profDAL.AddProfileInfoLabel(label);
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateInsertProfileLabel";
                await _logServ.LogError();
                
                return 0;
            }
            return 1;
        }


        /// <summary>
        /// Checks if the profileMatchLabel is unique
        /// </summary>
        /// <param name="label"> a list of Labels List<ProfileMatchLabelFormModel></param>
        /// <returns></returns>
        private bool CheckLabelsUnique(List<ProfileMatchLabelFormModel> label)
        {
            var names = new List<string>();
            foreach (var labelname in label)
            {
                names.Add(labelname.ProfileLabel.ToLower());
            }
            bool chk = names.Distinct().Count() == names.Count();
            return chk;
        }

    }
}
