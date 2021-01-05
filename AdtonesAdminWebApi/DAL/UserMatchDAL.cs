using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class UserMatchDAL : IUserMatchDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;
        private readonly IConnectionStringService _connService;
        private readonly ICampaignDAL _campDAL;

        public UserMatchDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, ICampaignDAL campDAL)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _connService = connService;
            _campDAL = campDAL;
        }


        public async Task<int> UpdateMediaLocation(string conn, string media, int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(UserMatchQuery.UpdateMediaLocation);
            try
            {
                builder.AddParameters(new { media = media });
                builder.AddParameters(new { campaignProfileId = id });

                return await _executers.ExecuteCommand(conn,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Takes original CampaignId from MAIN and transforms it
        /// updating both instances of the DB
        /// </summary>
        /// <param name="campaignId">Original campaignId</param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public async Task PrematchProcessForCampaign(int campaignId, string conn)
        {
            try
            {
                using (var connection = new SqlConnection(_connStr))
                {
                    await connection.OpenAsync();
                    await connection.ExecuteAsync("CampaignUserMatchSpByCampaignId",
                                                                        new { CampaignProfileId = campaignId },
                                                                        commandType: CommandType.StoredProcedure);
                }

                var campId = await _connService.GetCampaignProfileIdFromAdtoneIdByConn(campaignId, conn);

                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();
                    await connection.ExecuteAsync("CampaignUserMatchSpByCampaignId",
                                                                        new { CampaignProfileId = campId },
                                                                        commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserMatchDAL",
                    ProcedureName = "PrematchProcessForCampaign"
                };
                _logging.LogError();
            }

        }


        public async Task<int> GetProfileMatchInformationId(int countryId, string profileName)
        {
            using (var connection = new SqlConnection(_connStr))
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<int>(UserMatchQuery.GetProfileMatchInformationId, 
                                                                                        new { Id = countryId, profileName = profileName } ));
            }

        }


        public async Task<IEnumerable<string>> GetProfileMatchLabels(int infoId)
        {
            using (var connection = new SqlConnection(_connStr))
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<string>(UserMatchQuery.GetProfileMatchLabels,new { Id = infoId }));
            }

        }


        public async Task<CampaignBudgetModel> GetBudgetAmounts(int campaignId, string conn)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(UserMatchQuery.GetBudgetUpdateAmount);
            try
            {
                builder.AddParameters(new { Id = campaignId });

                return await _executers.ExecuteCommand(conn,
                                    conn => conn.QueryFirstOrDefault<CampaignBudgetModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateBucketCount(int campaignId, string conn, int bucketCount)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(UserMatchQuery.UpdateBucketAmount);
            try
            {
                builder.AddParameters(new { Id = campaignId });
                builder.AddParameters(new { BucketCount = bucketCount });

                return await _executers.ExecuteCommand(conn,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> AddCampaignData(NewCampaignProfileFormModel model, string conn)
        {
            try
            {

                return await _executers.ExecuteCommand(conn,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertNewCampaignData, model));
            }
            catch
            {
                throw;
            }
        }


        public async Task PrematchUserProcess(int campaignId, string conn)
        {
            using (var connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync("CampaignUserMatchSpByCampaignId",
                                                                    new { CampaignProfileId = campaignId },
                                                                    commandType: CommandType.StoredProcedure);
            }

        }


        public async Task<CampaignProfilePreference> GetCampaignProfilePreference()
        {
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<CampaignProfilePreference>(UserMatchQuery.GetCampaignProfilePreference));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> GetCampaignProfilePreferenceId(int campaignId)
        {
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<int>(UserMatchQuery.GetCampaignProfilePreferenceId, new { Id = campaignId }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<CampaignProfilePreference> GetCampaignProfilePreferenceDetailsByCampaignId(int campaignId)
        {
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<CampaignProfilePreference>(
                                        UserMatchQuery.GetCampaignProfilePreferenceDetailsByCampaignId, 
                                    new { Id = campaignId }));
            }
            catch
            {
                throw;
            }
        }


        #region GeographicProfile

        public async Task<int> InsertGeographicProfile(CreateOrUpdateCampaignProfileGeographicCommand model)
        {

            var prefs = await GetCampaignProfilePreferenceId(model.CampaignProfileId);
            int preferenceId = 0;
            if (prefs == 0)
            {
                string adpref = null;
                try
                {
                    preferenceId = await _executers.ExecuteCommand(_connStr,
                                            conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertGeographicProfile, new
                                            {
                                                CountryId = model.CountryId,
                                                CampaignProfileId = model.CampaignProfileId,
                                                PostCode = model.PostCode,
                                                Location_Demographics = model.Location_Demographics,
                                                AdtoneServerCampaignProfilePrefId = adpref
                                            }));

                    var lst = await _connService.GetConnectionStringsByCountry(model.CountryId);
                    List<string> conns = lst.ToList();

                    foreach (string constr in conns)
                    {
                        var countryId = await _connService.GetCountryIdFromAdtoneId(model.CountryId, constr);
                        var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                        preferenceId = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertGeographicProfile, new
                                        {
                                            CountryId = countryId,
                                            CampaignProfileId = campaignId,
                                            PostCode = model.PostCode,
                                            Location_Demographics = model.Location_Demographics,
                                            AdtoneServerCampaignProfilePrefId = preferenceId
                                        }));
                    }
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                model.CampaignProfileGeographicId = prefs;
                preferenceId = await UpdateGeographicProfile(model);
            }
            return preferenceId;
        }


        public async Task<int> UpdateGeographicProfile(CreateOrUpdateCampaignProfileGeographicCommand model)
        {
            int preferenceId = 0;
            var x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateGeographicProfile, new
                                        {
                                            PostCode = model.PostCode,
                                            Location_Demographics = model.Location_Demographics,
                                            Id = model.CampaignProfileGeographicId,
                                            CountryId = model.CountryId
                                        }));

                var lst = await _connService.GetConnectionStringsByCountry(model.CountryId);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    var countryId = await _connService.GetCountryIdFromAdtoneId(model.CountryId, constr);
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);
                    preferenceId = await _connService.GetCampaignProfilePreferenceIdFromAdtoneId(model.CampaignProfileGeographicId, constr);

                    x += await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateGeographicProfile, new
                                    {
                                        CountryId = countryId,
                                        PostCode = model.PostCode,
                                        Location_Demographics = model.Location_Demographics,
                                        Id = preferenceId
                                    }));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<int> InsertMatchCampaignGeographic(CampaignProfileGeographicFormModel model)
        {
            int x = 0;
            try
            {
                var lst = await _connService.GetConnectionStringsByCountry(model.CountryId);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);
                     
                    var matchDetails = await _executers.ExecuteCommand(constr,
                                    conn => conn.Query<NewCampaignProfileFormModel>(UserMatchQuery.GetCampaignMatchesDetailsById, new
                                    {
                                        Id = campaignId
                                    }));

                    if (matchDetails == null)
                        x = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertMatchCampaignGeographic, new
                                        {
                                            Id = campaignId,
                                            Location_Demographics = model.Location_Demographics,
                                        }));
                    else
                        x = await UpdateMatchCampaignGeographic(model);
                }
            }
            catch
            {
                throw;
            }
            return x;
        }

        public async Task<int> UpdateMatchCampaignGeographic(CampaignProfileGeographicFormModel model)
        {
            int x = 0;
            try
            {
                var lst = await _connService.GetConnectionStringsByCountry(model.CountryId);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateMatchCampaignGeographic, new
                                    {
                                        Id = campaignId,
                                        Location_Demographics = model.Location_Demographics,
                                    }));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }

        #endregion


        #region DemographicProfile

        public async Task<int> InsertDemographicProfile(CreateOrUpdateCampaignProfileDemographicsCommand model)
        {
            var prefs = await GetCampaignProfilePreferenceId(model.CampaignProfileId);
            int preferenceId = 0;
            if (prefs == 0)
            {
                string adpref = null;
                try
                {
                    preferenceId = await _executers.ExecuteCommand(_connStr,
                                            conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertDemographicProfile, new
                                            {
                                                CampaignProfileId = model.CampaignProfileId,
                                                DOBEnd_Demographics = model.DOBEnd_Demographics,
                                                DOBStart_Demographics = model.DOBStart_Demographics,
                                                Age_Demographics = model.Age_Demographics,
                                                Education_Demographics = model.Education_Demographics,
                                                Gender_Demographics = model.Gender_Demographics,
                                                HouseholdStatus_Demographics = model.HouseholdStatus_Demographics,
                                                IncomeBracket_Demographics = model.IncomeBracket_Demographics,
                                                RelationshipStatus_Demographics = model.RelationshipStatus_Demographics,
                                                WorkingStatus_Demographics = model.WorkingStatus_Demographics,
                                                AdtoneServerCampaignProfilePrefId = adpref
                                            }));

                    var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                    var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                    List<string> conns = lst.ToList();

                    foreach (string constr in conns)
                    {
                        var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                        preferenceId = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertDemographicProfile, new
                                        {
                                            CampaignProfileId = campaignId,
                                            AdtoneServerCampaignProfilePrefId = preferenceId,
                                            DOBEnd_Demographics = model.DOBEnd_Demographics,
                                            DOBStart_Demographics = model.DOBStart_Demographics,
                                            Age_Demographics = model.Age_Demographics,
                                            Education_Demographics = model.Education_Demographics,
                                            Gender_Demographics = model.Gender_Demographics,
                                            HouseholdStatus_Demographics = model.HouseholdStatus_Demographics,
                                            IncomeBracket_Demographics = model.IncomeBracket_Demographics,
                                            RelationshipStatus_Demographics = model.RelationshipStatus_Demographics,
                                            WorkingStatus_Demographics = model.WorkingStatus_Demographics
                                        }));
                    }
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                model.Id = prefs;
                preferenceId = await UpdateDemographicProfile(model);
            }
            return preferenceId;
        }


        public async Task<int> UpdateDemographicProfile(CreateOrUpdateCampaignProfileDemographicsCommand model)
        {
            int preferenceId = 0;
            var x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateDemographicProfile, new
                                        {
                                            DOBEnd_Demographics = model.DOBEnd_Demographics,
                                            DOBStart_Demographics = model.DOBStart_Demographics,
                                            Age_Demographics = model.Age_Demographics,
                                            Education_Demographics = model.Education_Demographics,
                                            Gender_Demographics = model.Gender_Demographics,
                                            HouseholdStatus_Demographics = model.HouseholdStatus_Demographics,
                                            IncomeBracket_Demographics = model.IncomeBracket_Demographics,
                                            RelationshipStatus_Demographics = model.RelationshipStatus_Demographics,
                                            WorkingStatus_Demographics = model.WorkingStatus_Demographics,
                                            Id = model.Id
                                        }));

                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);
                    preferenceId = await _connService.GetCampaignProfilePreferenceIdFromAdtoneId(model.Id, constr);

                    x += await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateDemographicProfile, new
                                    {
                                        DOBEnd_Demographics = model.DOBEnd_Demographics,
                                        DOBStart_Demographics = model.DOBStart_Demographics,
                                        Age_Demographics = model.Age_Demographics,
                                        Education_Demographics = model.Education_Demographics,
                                        Gender_Demographics = model.Gender_Demographics,
                                        HouseholdStatus_Demographics = model.HouseholdStatus_Demographics,
                                        IncomeBracket_Demographics = model.IncomeBracket_Demographics,
                                        RelationshipStatus_Demographics = model.RelationshipStatus_Demographics,
                                        WorkingStatus_Demographics = model.WorkingStatus_Demographics,
                                        Id = preferenceId
                                    }));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<int> InsertMatchCampaignDemographic(CampaignProfileDemographicsFormModel model)
        {
            int x = 0;
            try
            {
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                    var matchDetails = await _executers.ExecuteCommand(constr,
                                    conn => conn.Query<NewCampaignProfileFormModel>(UserMatchQuery.GetCampaignMatchesDetailsById, new
                                    {
                                        Id = campaignId
                                    }));

                    if (matchDetails == null)
                        x = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertMatchCampaignDemographic, new
                                        {
                                            Id = campaignId,
                                            Age_Demographics = model.Age_Demographics,
                                            Education_Demographics = model.Education_Demographics,
                                            Gender_Demographics = model.Gender_Demographics,
                                            HouseholdStatus_Demographics = model.HouseholdStatus_Demographics,
                                            IncomeBracket_Demographics = model.IncomeBracket_Demographics,
                                            RelationshipStatus_Demographics = model.RelationshipStatus_Demographics,
                                            WorkingStatus_Demographics = model.WorkingStatus_Demographics,
                                        }));
                    else
                        x = await UpdateMatchCampaignDemographic(model);
                }
            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<int> UpdateMatchCampaignDemographic(CampaignProfileDemographicsFormModel model)
        {
            int x = 0;
            try
            {
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateMatchCampaignDemographic, new
                                    {
                                        Id = campaignId,
                                        Age_Demographics = model.Age_Demographics,
                                        Education_Demographics = model.Education_Demographics,
                                        Gender_Demographics = model.Gender_Demographics,
                                        HouseholdStatus_Demographics = model.HouseholdStatus_Demographics,
                                        IncomeBracket_Demographics = model.IncomeBracket_Demographics,
                                        RelationshipStatus_Demographics = model.RelationshipStatus_Demographics,
                                        WorkingStatus_Demographics = model.WorkingStatus_Demographics
                                    }));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }



        #endregion


        #region TimeSettingProfile


        public async Task<int> InsertTimeSettingsProfile(CampaignProfileTimeSetting timeSettings)
        {

            var prefs = await GetCampaignTimeSettings(timeSettings.CampaignProfileId);
            int preferenceId = 0;
            if (prefs == null)
            {
                string adpref = null;
                try
                {
                    preferenceId = await _executers.ExecuteCommand(_connStr,
                                            conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertTimeSettingsProfile, new
                                            {
                                                Monday = timeSettings.Monday,
                    Tuesday = timeSettings.Tuesday,
                    Wednesday = timeSettings.Wednesday,
                    Thursday = timeSettings.Thursday,
                    Friday = timeSettings.Friday,
                    Saturday = timeSettings.Saturday,
                    Sunday = timeSettings.Sunday,
                    CampaignProfileId = timeSettings.CampaignProfileId,
                    AdtoneServerCampaignProfileTimeId = adpref
                }));

                    var campaignDetails = await _campDAL.GetCampaignProfileDetail(timeSettings.CampaignProfileId);

                    var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                    List<string> conns = lst.ToList();

                    foreach (string constr in conns)
                    {
                        var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(timeSettings.CampaignProfileId, constr);

                        var x = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertTimeSettingsProfile, new
                                        {
                                            Monday = timeSettings.Monday,
                                            Tuesday = timeSettings.Tuesday,
                                            Wednesday = timeSettings.Wednesday,
                                            Thursday = timeSettings.Thursday,
                                            Friday = timeSettings.Friday,
                                            Saturday = timeSettings.Saturday,
                                            Sunday = timeSettings.Sunday,
                                            CampaignProfileId = campaignId,
                                            AdtoneServerCampaignProfileTimeId = preferenceId
                                        }));
                    }
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                timeSettings.CampaignProfileTimeSettingsId = prefs.CampaignProfileTimeSettingsId;
                preferenceId = await UpdateTimeSettingsProfile(timeSettings);
            }
            return preferenceId;
        }


        public async Task<int> UpdateTimeSettingsProfile(CampaignProfileTimeSetting timeSettings)
        {
            var x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateTimeSettingsProfile, new
                                        {
                                            Monday = timeSettings.Monday,
                                            Tuesday = timeSettings.Tuesday,
                                            Wednesday = timeSettings.Wednesday,
                                            Thursday = timeSettings.Thursday,
                                            Friday = timeSettings.Friday,
                                            Saturday = timeSettings.Saturday,
                                            Sunday = timeSettings.Sunday,
                                            CampaignProfileId = timeSettings.CampaignProfileId
                                        }));

                var campaignDetails = await _campDAL.GetCampaignProfileDetail(timeSettings.CampaignProfileId);

                var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(timeSettings.CampaignProfileId, constr);

                    x += await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateTimeSettingsProfile, new
                                    {
                                        Monday = timeSettings.Monday,
                                        Tuesday = timeSettings.Tuesday,
                                        Wednesday = timeSettings.Wednesday,
                                        Thursday = timeSettings.Thursday,
                                        Friday = timeSettings.Friday,
                                        Saturday = timeSettings.Saturday,
                                        Sunday = timeSettings.Sunday,
                                        CampaignProfileId = timeSettings.CampaignProfileId
                                    }));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }



        public async Task<CampaignProfileTimeSetting> GetCampaignTimeSettings(int campaignId)
        {
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<CampaignProfileTimeSetting>(UserMatchQuery.GetCampaignTimeSettings,
                                                                                        new { Id = campaignId }));
            }
            catch
            {
                throw;
            }
        }



        #endregion


        #region MobileProfile


        public async Task<int> InsertMobileProfile(CreateOrUpdateCampaignProfileMobileCommand model)
        {

            var prefs = await GetCampaignProfilePreferenceId(model.CampaignProfileId);
            int preferenceId = 0;
            if (prefs == 0)
            {
                string adpref = null;
                try
                {
                    preferenceId = await _executers.ExecuteCommand(_connStr,
                                            conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertMobileProfile, new
                                            {
                                                CampaignProfileId = model.CampaignProfileId,
                                                ContractType_Mobile = model.ContractType_Mobile,
                                                Spend_Mobile = model.Spend_Mobile,
                                                AdtoneServerCampaignProfilePrefId = adpref
                                            }));

                    var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                    var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                    List<string> conns = lst.ToList();

                    foreach (string constr in conns)
                    {
                        var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                        preferenceId = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertMobileProfile, new
                                        {
                                            CampaignProfileId = campaignId,
                                            ContractType_Mobile = model.ContractType_Mobile,
                                            Spend_Mobile = model.Spend_Mobile,
                                            AdtoneServerCampaignProfilePrefId = preferenceId
                                        }));
                    }
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                model.CampaignProfileMobileId = prefs;
                preferenceId = await UpdateMobileProfile(model);
            }
            return preferenceId;
        }


        public async Task<int> UpdateMobileProfile(CreateOrUpdateCampaignProfileMobileCommand model)
        {
            var x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateMobileProfile, new
                                        {
                                            Id = model.CampaignProfileId,
                                            ContractType_Mobile = model.ContractType_Mobile,
                                            Spend_Mobile = model.Spend_Mobile
                                        }));

                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                    x += await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateMobileProfile, new
                                    {
                                        Id = campaignId,
                                        ContractType_Mobile = model.ContractType_Mobile,
                                        Spend_Mobile = model.Spend_Mobile
                                    }));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<int> UpdateMatchCampaignMobile(CampaignProfileMobileFormModel model)
        {
            int x = 0;
            try
            {
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateMatchCampaignMobile, new
                                    {
                                        Id = campaignId,
                                        ContractType_Mobile = model.ContractType_Mobile,
                                        Spend_Mobile = model.Spend_Mobile
                                    }));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }


        #endregion


        #region Questionnaire

        public async Task<int> InsertQuestionnaireProfile(CampaignProfileSkizaFormModel model)
        {

            var prefs = await GetCampaignProfilePreferenceId(model.CampaignProfileId);
            int preferenceId = 0;
            if (prefs == 0)
            {
                string adpref = null;
                try
                {
                    preferenceId = await _executers.ExecuteCommand(_connStr,
                                            conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertQuestionnaireProfile, new
                                            {
                                                CampaignProfileId = model.CampaignProfileId,
                                                CountryId = model.CountryId,
                                                DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType,
                                                MassQuestion = model.MassQuestion,
                                                Hustlers_AdType = model.Hustlers_AdType,
                                                Youth_AdType = model.Youth_AdType,
                                                AdtoneServerCampaignProfilePrefId = adpref
                                            }));

                    var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                    var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                    List<string> conns = lst.ToList();

                    foreach (string constr in conns)
                    {
                        var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);
                        var countryId = await _connService.GetCountryIdFromAdtoneId(model.CountryId, constr);

                        var x = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertQuestionnaireProfile, new
                                        {
                                            CampaignProfileId = campaignId,
                                            CountryId = countryId,
                                            DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType,
                                            MassQuestion = model.MassQuestion,
                                            Hustlers_AdType = model.Hustlers_AdType,
                                            Youth_AdType = model.Youth_AdType,
                                            AdtoneServerCampaignProfilePrefId = preferenceId
                                        }));
                    }
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                model.CampaignProfileSKizaId = prefs;
                preferenceId = await UpdateQuestionnaireProfile(model);
            }
            return preferenceId;
        }


        public async Task<int> UpdateQuestionnaireProfile(CampaignProfileSkizaFormModel model)
        {
            var x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateQuestionnaireProfile, new
                                        {
                                            Id = model.CampaignProfileId,
                                            CountryId = model.CountryId,
                                            DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType,
                                            MassQuestion = model.MassQuestion,
                                            Hustlers_AdType = model.Hustlers_AdType,
                                            Youth_AdType = model.Youth_AdType
                                        }));

                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);
                    var countryId = await _connService.GetCountryIdFromAdtoneId(model.CountryId, constr);

                    x += await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateQuestionnaireProfile, new
                                    {
                                        Id = campaignId,
                                        CountryId = countryId,
                                        DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType,
                                        MassQuestion = model.MassQuestion,
                                        Hustlers_AdType = model.Hustlers_AdType,
                                        Youth_AdType = model.Youth_AdType
                                    }));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<int> UpdateMatchCampaignQuestionnaire(CampaignProfileSkizaFormModel model)
        {
            int x = 0;
            try
            {
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateMatchCampaignQuestionnaire, new
                                    {
                                        Id = campaignId,
                                        DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType,
                                        MassQuestion = model.MassQuestion,
                                        Hustlers_AdType = model.Hustlers_AdType,
                                        Youth_AdType = model.Youth_AdType
                                    }));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }



        #endregion


        #region AdvertProfile


        public async Task<int> InsertAdvertProfile(CampaignProfileAdvertFormModel model)
        {

            var prefs = await GetCampaignProfilePreferenceId(model.CampaignProfileId);
            int preferenceId = 0;
            if (prefs == 0)
            {
                try
                {
                    preferenceId = await _executers.ExecuteCommand(_connStr,
                                            conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertAdvertProfile, model));

                    var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                    var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                    List<string> conns = lst.ToList();

                    foreach (string constr in conns)
                    {
                        var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                        model.CampaignProfileId = campaignId;

                        var x = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertAdvertProfile, model));
                    }
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                model.CampaignProfileAdvertsId = prefs;
                preferenceId = await UpdateAdvertProfile(model);
            }
            return preferenceId;
        }


        public async Task<int> UpdateAdvertProfile(CampaignProfileAdvertFormModel model)
        {
            var x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateAdvertProfile, model));

                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                    model.CampaignProfileId = campaignId;

                    x += await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateAdvertProfile, model));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<int> UpdateMatchCampaignAdvert(CampaignProfileAdvertFormModel model)
        {
            int x = 0;
            try
            {
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);
                    model.CampaignProfileId = campaignId;

                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateMatchCampaignAdvert, model));
                }
            }
            catch
            {
                throw;
            }
            return x;
        }



        #endregion
    }
}
