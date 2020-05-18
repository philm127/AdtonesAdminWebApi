using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Enums;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
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
        ReturnResult result = new ReturnResult();


        public ProfileMatchInfoService(IConfiguration configuration)

        {
            _configuration = configuration;
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
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                        await connection.OpenAsync();
                        result.body = await connection.QueryAsync<ProfileMatchInformationFormModel>(@"SELECT prof.Id,prof.ProfileName,
                                                                                        prof.ProfileType,prof.CountryId, c.Name,prof.IsActive 
                                                                                        FROM ProfileMatchInformations AS prof 
                                                                                        LEFT JOIN Country AS c ON prof.CountryId=c.Id");
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "ProfileMatchInfoService",
                    ProcedureName = "FillProfileMatchInformationResult"
                };
                _logging.LogError();
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
                using(var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    prof = await connection.QueryFirstOrDefaultAsync<ProfileMatchInformationFormModel>(@"SELECT p.Id, p.ProfileName,p.IsActive,
                                                                                                    c.Name,p.ProfileType
                                                                                                    FROM ProfileMatchInformations p
                                                                                                    LEFT JOIN Country c ON p.CountryId=c.Id
                                                                                                    WHERE p.Id=@id", new { id });
                }

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    label = await connection.QueryAsync<ProfileMatchLabelFormModel>(@"SELECT Id,ProfileLabel,ProfileMatchInformationId,CreatedDate
                                                                                                    FROM ProfileMatchLabels
                                                                                                    WHERE ProfileMatchInformationId=@id", new { prof.Id });
                }

                prof.profileMatchLabelFormModels = label.ToList();
                result.body = prof;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "ProfileMatchInfoService",
                    ProcedureName = "GetprofileInfo"
                };
                _logging.LogError();
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
                    if (CheckIfProfileExists(model))
                    {
                        result.error = model.ProfileName + " Record Exists.";
                        result.result = 0;
                        return result;
                    }

                    var profileLabelCount = profileMatches.Count;
                    if (profileLabelCount > 0)
                    {
                        string insert_query = @"UPDATE ProfileMatchInformations SET IsActive=@IsActive,UpdatedDate=GETDATE(),ProfileType=@ProfileType
                                                                                                                    WHERE Id=@Id;";

                        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                        {
                            await connection.OpenAsync();
                            var x = await connection.QueryAsync<int>(insert_query, model);
                        }

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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "ProfileMatchInfoService",
                    ProcedureName = "UpdateProfileInfo"
                };
                _logging.LogError();
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
                    if (CheckIfProfileExists(model))
                    {
                        result.error = model.ProfileName + " Record Exists.";
                        result.result = 0;
                        return result;
                    }

                    
                    string insert_query = @"INSERT INTO ProfileMatchInformations(ProfileName,IsActive,CountryId,CreatedDate,UpdatedDate,
                                                                                                            ProfileType)
                                                            VALUES(@ProfileName,@IsActive,@CountryId,GETDATE(),GETDATE(),@ProfileType);
                                                            SELECT CAST(SCOPE_IDENTITY() AS INT);";


                    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        await connection.OpenAsync();
                        profileId = await connection.ExecuteScalarAsync<int>(insert_query, model);

                        if (!(profileId > 0))
                        {
                            result.result = 0;
                            result.error = "ProfileInfo was NOT added successfully";
                            return result;
                        }
                    }

                    // This could be passed to the UpdateInsertProfileLabel function but as took so few lines put it here.
                    string new_insert = @"INSERT INTO ProfileMatchLabels(ProfileMatchInformationId, ProfileLabel, CreatedDate, UpdatedDate)
                                            VALUES(@ProfileMatchInformationId, @ProfileLabel, GETDATE(), GETDATE());";
                    foreach (var label in model.profileMatchLabelFormModels)
                    {
                        label.ProfileMatchInformationId = profileId;
                        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                        {
                            await connection.OpenAsync();
                            var y = await connection.ExecuteScalarAsync<string>(new_insert, label);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var _logging = new ErrorLogging()
                    {
                        ErrorMessage = ex.Message.ToString(),
                        StackTrace = ex.StackTrace.ToString(),
                        PageName = "ProfileMatchInfoService",
                        ProcedureName = "AddProfileInfo"
                    };
                    _logging.LogError();
                    result.result = 0;
                    result.error = "ProfileInfo was NOT added successfully";
                }
                result.body = profileId;
                return result;
            }
            else
            {
                result.error = "Please add at least one record.";
                result.result = 0;
                return result;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">Uses the IdCollectionViewModel id as the Id</param>
        /// <returns></returns>
        public async Task<ReturnResult> DeleteProfileLabel(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    await connection.ExecuteAsync("DELETE FROM ProfileMatchLabels WHERE Id=@id", new { id = id });
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "ProfileMatchInfoService",
                    ProcedureName = "DeleteProfileLabel"
                };
                _logging.LogError();
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
        private async Task<int> UpdateInsertProfileLabel(List<ProfileMatchLabelFormModel> labellist,int profileId)
        {
            try
            {
                string update_query = @"UPDATE ProfileMatchLabels SET ProfileLabel=@ProfileLabel,UpdatedDate=GETDATE() WHERE Id=@Id";
                string insert_query = @"INSERT INTO ProfileMatchLabels(ProfileLabel,ProfileMatchInformationId,CreatedDate,UpdatedDate) 
                                                VALUES(@ProfileLabel,@ProfileMatchInformationId,GETDATE(),GETDATE());";
                string query = string.Empty;
                foreach (var label in labellist)
                {
                    if (label.Id == 0)
                    {
                        label.ProfileMatchInformationId = profileId;
                        query = insert_query;
                    }
                    else
                        query = update_query;

                    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        await connection.OpenAsync();
                        var x = await connection.QueryAsync<int>(query, label);
                    }
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "ProfileMatchInfoService",
                    ProcedureName = "UpdateInsertProfileLabel"
                };
                _logging.LogError();
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
            foreach(var labelname in label)
            {
                names.Add(labelname.ProfileLabel.ToLower());
            }
            bool chk = names.Distinct().Count() == names.Count();
            return chk;
        }


        private bool CheckIfProfileExists(ProfileMatchInformationFormModel model)
        {
            bool profileLabelExist = false;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                profileLabelExist = connection.ExecuteScalar<bool>(@"SELECT COUNT(1) FROM ProfileMatchInformations 
                                                                    WHERE LOWER(ProfileName) = @profilename
                                                                    AND CountryId=@countryId AND LOWER(ProfileType)=@profileType",
                                                              new { profilename = model.ProfileName.Trim().ToLower(), countryId = model.CountryId, profileType = model.ProfileType.ToLower() });
            }
            return profileLabelExist;
        }


    }
}
