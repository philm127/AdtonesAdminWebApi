﻿using BusinessServices.Interfaces.Repository;
using Data.Repositories.Queries;
using AdtonesAdminWebApi.Services;
using Domain.ViewModels;
using Domain.ViewModels.CreateUpdateCampaign;
using Domain.ViewModels.CreateUpdateCampaign.ProfileModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CampaignMatchesRepository : ICampaignMatchesRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IConnectionStringService _connService;
        private readonly ICampaignDAL _campDAL;
        private readonly ILoggingService _logServ;
        const string PageName = "UserMatchDAL";

        public CampaignMatchesRepository(IConfiguration configuration, IConnectionStringService connService, ICampaignDAL campDAL,
                            ILoggingService logServ)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _connService = connService;
            _campDAL = campDAL;
            _logServ = logServ;
        }


        public async Task<int> UpdateMediaLocation(string conn, string media, int id)
        {
            var sqlUpdate = "UPDATE CampaignMatches SET MEDIA_URL=@media WHERE MSCampaignProfileId=@campaignProfileId";
            try
            {
                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();
                    return await connection.ExecuteScalarAsync<int>(sqlUpdate, new { media = media, campaignProfileId = id });
                }
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> AddCampaignMatchData(NewCampaignProfileFormModel model, string conn)
        {
            var sqlInsert = @"INSERT INTO CampaignMatches(UserId,ClientId,CampaignName,CampaignDescription,TotalBudget,
                                                        MaxDailyBudget,MaxBid,MaxMonthBudget,MaxWeeklyBudget,MaxHourlyBudget,TotalCredit,
                                                        EmailFileLocation,Active,
                                                        EmailSubject,EmailBody,EmailSenderAddress,SmsOriginator,SmsBody,SMSFileLocation,CreatedDateTime,
                                                        UpdatedDateTime,Status,StartDate,EndDate,NumberInBatch,CountryId,
                                                        NextStatus,MSCampaignProfileId,EMAIL_MESSAGE,SMS_MESSAGE,ORIGINATOR)
                                                    VALUES(@UserId,@ClientId,@CampaignName,@CampaignDescription,@TotalBudget,@MaxDailyBudget,
                                                        @MaxBid,@MaxMonthBudget,@MaxWeeklyBudget,@MaxHourlyBudget,@TotalCredit,
                                                        @EmailFileLocation,@Active,@EmailSubject,@EmailBody,@EmailSenderAddress,@SmsOriginator,@SmsBody,
                                                        @SMSFileLocation,GETDATE(),GETDATE(),@Status,@StartDate,@EndDate,@NumberInBatch,
                                                        @CountryId,1,
                                                        @CampaignProfileId,@EmailBody,@SmsBody,@SmsOriginator);
                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
            try
            {

                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();
                    return await connection.ExecuteScalarAsync<int>(sqlInsert, model));
                }
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateCampaignMatchData(NewCampaignProfileFormModel model, string conn)
        {
            string UpdateCampaignMatchData = @"UPDATE CampaignMatches SET CampaignDescription=@CampaignDescription,
                                                    MaxDailyBudget=@MaxDailyBudget,MaxBid=@MaxBid,MaxMonthBudget=@MaxMonthBudget,
                                                    MaxWeeklyBudget=@MaxWeeklyBudget,MaxHourlyBudget=@MaxHourlyBudget,
                                                    EmailSubject=@EmailSubject,EmailBody=@EmailBody,SmsOriginator=@SmsOriginator,SmsBody=@SmsBody,
                                                    UpdatedDateTime=GETDATE(),StartDate=@StartDate,EndDate=@EndDate
                                                    WHERE MSCampaignProfileId=@CampaignProfileId ";
            var campId = model.CampaignProfileId;
            var x = 0;
            try
            {
                model.CampaignProfileId = await _connService.GetCampaignProfileIdFromAdtoneIdByConn(model.CampaignProfileId, conn);

                using (var connection = new SqlConnection(conn))
                {
                    await connection.OpenAsync();
                    x = await connection.ExecuteScalarAsync<int>(UpdateCampaignMatchData, model);
                }
            }
            catch
            {
                throw;
            }
            model.CampaignProfileId = campId;
            return x;
        }


        public async Task<int> UpdateCampaignMatchesCredit(BillingPaymentModel model, string constr)
        {
            string InsertMatchFinancial = @"UPDATE CampaignMatches SET TotalBudget=@TotalBudget,TotalCredit=@TotalCredit,
                                                        AvailableCredit=@AvailableCredit,Status=@Status WHERE MSCampaignProfileId=@Id";
            var campModel = await GetCampaignProfileDetail(model.CampaignProfileId);
            var available = await _invDAL.GetAvailableCredit(model.AdvertiserId);
            int x = 0;
            try
            {

                if (constr != null && constr.Length > 10)
                {
                    var campId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                    using (var connection = new SqlConnection(constr))
                    {
                        await connection.OpenAsync();
                        x = await connection.ExecuteScalarAsync<int>(InsertMatchFinancial, new
                        {
                            Id = campId,
                            Status = (int)Domain.Enums.CampaignStatus.Play,
                            TotalBudget = (campModel.TotalBudget + model.Fundamount),
                            TotalCredit = (campModel.TotalCredit + model.TotalAmount),
                            AvailableCredit = available
                        });
                    }
                }
            }
            catch
            {
                throw;
            }

            return x;
        }


        // XXXXXXXXXXXXXXX END OF CampaignMatches XXXXXXXXXXXXXXXXXXXXXXXXXX //

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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "PrematchProcessForCampaign";
                await _logServ.LogError();
                throw;
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


        public async Task<int> InsertProfilePreference(NewAdProfileMappingFormModel model)
        {
            var prefs = await GetCampaignProfilePreferenceId(model.CampaignProfileId);
            int preferenceId = 0;
            if (prefs == 0)
            {
                string adpref = null;
                try
                {
                    preferenceId = await _executers.ExecuteCommand(_connStr,
                        conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertProfilePreference, new
                        {
                            CountryId = model.CountryId,
                            CampaignProfileId = model.CampaignProfileId,
                            AdtoneServerCampaignProfilePrefId = adpref
                        }));

                    var constr = await _connService.GetConnectionStringByOperator(model.OperatorId);

                    var countryId = await _connService.GetCountryIdFromAdtoneId(model.CountryId, constr);
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                    preferenceId = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertProfilePreference, new
                                    {
                                        CountryId = countryId,
                                        CampaignProfileId = campaignId,
                                        AdtoneServerCampaignProfilePrefId = preferenceId
                                    }));
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "InsertProfilePreference";
                    await _logServ.LogError();
                    throw;
                }
            }

            return preferenceId;
        }




        #region GeographicProfile

        
        public async Task<int> UpdateGeographicProfile(CreateOrUpdateCampaignProfileGeographicCommand model, string conns)
        {
            var x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateGeographicProfile, new
                                        {
                                            PostCode = model.PostCode,
                                            Location_Demographics = model.Location_Demographics,
                                            Id = model.CampaignProfileId
                                        }));


                
                    var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, conns);

                    x += await _executers.ExecuteCommand(conns,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateGeographicProfile, new
                                    {
                                        PostCode = model.PostCode,
                                        Location_Demographics = model.Location_Demographics,
                                        Id = campaignId
                                    }));
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateGeographicProfile";
                await _logServ.LogError();
                
                throw;
            }
            return x;
        }


        public async Task<int> UpdateMatchCampaignGeographic(CreateOrUpdateCampaignProfileGeographicCommand model, string constr)
        {
            int x = 0;
            try
            {


                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                x = await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateMatchCampaignGeographic, new
                                {
                                    Id = campaignId,
                                    Location_Demographics = model.Location_Demographics,
                                }));
            }
            catch
            {
                throw;
            }
            return x;
        }

        #endregion


        #region DemographicProfile


        public async Task<int> UpdateDemographicProfile(CreateOrUpdateCampaignProfileDemographicsCommand model, string constr)
        {
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
                                            Id = model.CampaignProfileId
                                        }));

                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);


                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

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
                                    Id = campaignId
                                }));
            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<int> UpdateMatchCampaignDemographic(CreateOrUpdateCampaignProfileDemographicsCommand model, string constr)
        {
            int x = 0;
            try
            {
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

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
            catch
            {
                throw;
            }
            return x;
        }



        #endregion


        #region TimeSettingProfile


        public async Task<int> InsertTimeSettingsProfile(CampaignProfileTimeSetting timeSettings, string constr)
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
                catch
                {
                    throw;
                }
            }
            else
            {
                timeSettings.CampaignProfileTimeSettingsId = prefs.CampaignProfileTimeSettingsId;
                preferenceId = await UpdateTimeSettingsProfile(timeSettings, constr);
            }
            return preferenceId;
        }


        public async Task<int> UpdateTimeSettingsProfile(CampaignProfileTimeSetting timeSettings, string constr)
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
                                            Id = timeSettings.CampaignProfileId
                                        }));

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
                                    Id = campaignId
                                }));
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


        public async Task<int> UpdateMobileProfile(CreateOrUpdateCampaignProfileMobileCommand model, string constr)
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

                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                x += await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateMobileProfile, new
                                {
                                    Id = campaignId,
                                    ContractType_Mobile = model.ContractType_Mobile,
                                    Spend_Mobile = model.Spend_Mobile
                                }));
            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<int> UpdateMatchCampaignMobile(CreateOrUpdateCampaignProfileMobileCommand model, string constr)
        {
            int x = 0;
            try
            {
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                x = await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateMatchCampaignMobile, new
                                {
                                    Id = campaignId,
                                    ContractType_Mobile = model.ContractType_Mobile,
                                    Spend_Mobile = model.Spend_Mobile
                                }));
            }
            catch
            {
                throw;
            }
            return x;
        }


        #endregion


        #region Questionnaire

        

        public async Task<int> UpdateQuestionnaireProfile(CreateOrUpdateCampaignProfileSkizaCommand model, string constr)
        {
            var x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateQuestionnaireProfile, new
                                        {
                                            Id = model.CampaignProfileId,
                                            DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType,
                                            Mass_AdType = model.Mass_AdType,
                                            Hustlers_AdType = model.Hustlers_AdType,
                                            Youth_AdType = model.Youth_AdType
                                        }));

                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);


                x += await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateQuestionnaireProfile, new
                                {
                                    Id = campaignId,
                                    DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType,
                                    Mass_AdType = model.Mass_AdType,
                                    Hustlers_AdType = model.Hustlers_AdType,
                                    Youth_AdType = model.Youth_AdType
                                }));
            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<int> UpdateMatchCampaignQuestionnaire(CreateOrUpdateCampaignProfileSkizaCommand model, string constr)
        {
            int x = 0;
            try
            {
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                x = await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateMatchCampaignQuestionnaire, new
                                {
                                    Id = campaignId,
                                    DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType,
                                    Mass_AdType = model.Mass_AdType,
                                    Hustlers_AdType = model.Hustlers_AdType,
                                    Youth_AdType = model.Youth_AdType
                                }));
            }
            catch
            {
                throw;
            }
            return x;
        }



        #endregion


        #region AdvertProfile
 

        public async Task<int> UpdateAdvertProfile(CreateOrUpdateCampaignProfileAdvertCommand model, string constr)
        {
            var x = 0;
            var cid = model.CampaignProfileId;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                                        conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateAdvertProfile, model));

                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                model.CampaignProfileId = campaignId;

                x += await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateAdvertProfile, model));
            }
            catch
            {
                throw;
            }
            model.CampaignProfileId = cid;
            return x;
        }


        public async Task<int> UpdateMatchCampaignAdvert(CreateOrUpdateCampaignProfileAdvertCommand model, string constr)
        {
            var cid = model.CampaignProfileId;
            int x = 0;
            try
            {
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                model.CampaignProfileId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                x = await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateMatchCampaignAdvert, model));
            }
            catch
            {
                throw;
            }
            model.CampaignProfileId = cid;
            return x;
        }



        #endregion

        //public async Task<int> InsertMatchCampaignGeographic(CreateOrUpdateCampaignProfileGeographicCommand model, List<string> conns)
        //{
        //    int x = 0;
        //    try
        //    {
        //        foreach (string constr in conns)
        //        {
        //            var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

        //            var matchDetails = await _executers.ExecuteCommand(constr,
        //                            conn => conn.Query<NewCampaignProfileFormModel>(UserMatchQuery.GetCampaignMatchesDetailsById, new
        //                            {
        //                                Id = campaignId
        //                            }));

        //            if (matchDetails == null)
        //                x = await _executers.ExecuteCommand(constr,
        //                                conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertMatchCampaignGeographic, new
        //                                {
        //                                    Id = campaignId,
        //                                    Location_Demographics = model.Location_Demographics,
        //                                }));
        //            else
        //                x = await UpdateMatchCampaignGeographic(model, conns);
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return x;
        //}

        //public async Task<int> InsertMobileProfile(CreateOrUpdateCampaignProfileMobileCommand model)
        //{

        //    var prefs = await GetCampaignProfilePreferenceId(model.CampaignProfileId);
        //    int preferenceId = 0;
        //    if (prefs == 0)
        //    {
        //        string adpref = null;
        //        try
        //        {
        //            preferenceId = await _executers.ExecuteCommand(_connStr,
        //                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertMobileProfile, new
        //                                    {
        //                                        CampaignProfileId = model.CampaignProfileId,
        //                                        ContractType_Mobile = model.ContractType_Mobile,
        //                                        Spend_Mobile = model.Spend_Mobile,
        //                                        AdtoneServerCampaignProfilePrefId = adpref
        //                                    }));

        //            var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

        //            var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
        //            List<string> conns = lst.ToList();

        //            foreach (string constr in conns)
        //            {
        //                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

        //                preferenceId = await _executers.ExecuteCommand(constr,
        //                                conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertMobileProfile, new
        //                                {
        //                                    CampaignProfileId = campaignId,
        //                                    ContractType_Mobile = model.ContractType_Mobile,
        //                                    Spend_Mobile = model.Spend_Mobile,
        //                                    AdtoneServerCampaignProfilePrefId = preferenceId
        //                                }));
        //            }
        //        }
        //        catch
        //        {
        //            throw;
        //        }
        //    }
        //    else
        //    {
        //        model.CampaignProfileMobileId = prefs;
        //        preferenceId = await UpdateMobileProfile(model);
        //    }
        //    return preferenceId;
        //}


        //public async Task<int> InsertDemographicProfile(CreateOrUpdateCampaignProfileDemographicsCommand model)
        //{
        //    var prefs = await GetCampaignProfilePreferenceId(model.CampaignProfileId);
        //    int preferenceId = 0;
        //    if (prefs == 0)
        //    {
        //        string adpref = null;
        //        try
        //        {
        //            preferenceId = await _executers.ExecuteCommand(_connStr,
        //                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertDemographicProfile, new
        //                                    {
        //                                        CampaignProfileId = model.CampaignProfileId,
        //                                        DOBEnd_Demographics = model.DOBEnd_Demographics,
        //                                        DOBStart_Demographics = model.DOBStart_Demographics,
        //                                        Age_Demographics = model.Age_Demographics,
        //                                        Education_Demographics = model.Education_Demographics,
        //                                        Gender_Demographics = model.Gender_Demographics,
        //                                        HouseholdStatus_Demographics = model.HouseholdStatus_Demographics,
        //                                        IncomeBracket_Demographics = model.IncomeBracket_Demographics,
        //                                        RelationshipStatus_Demographics = model.RelationshipStatus_Demographics,
        //                                        WorkingStatus_Demographics = model.WorkingStatus_Demographics,
        //                                        AdtoneServerCampaignProfilePrefId = adpref
        //                                    }));

        //            var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

        //            var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
        //            List<string> conns = lst.ToList();

        //            foreach (string constr in conns)
        //            {
        //                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

        //                preferenceId = await _executers.ExecuteCommand(constr,
        //                                conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertDemographicProfile, new
        //                                {
        //                                    CampaignProfileId = campaignId,
        //                                    AdtoneServerCampaignProfilePrefId = preferenceId,
        //                                    DOBEnd_Demographics = model.DOBEnd_Demographics,
        //                                    DOBStart_Demographics = model.DOBStart_Demographics,
        //                                    Age_Demographics = model.Age_Demographics,
        //                                    Education_Demographics = model.Education_Demographics,
        //                                    Gender_Demographics = model.Gender_Demographics,
        //                                    HouseholdStatus_Demographics = model.HouseholdStatus_Demographics,
        //                                    IncomeBracket_Demographics = model.IncomeBracket_Demographics,
        //                                    RelationshipStatus_Demographics = model.RelationshipStatus_Demographics,
        //                                    WorkingStatus_Demographics = model.WorkingStatus_Demographics
        //                                }));
        //            }
        //        }
        //        catch
        //        {
        //            throw;
        //        }
        //    }
        //    else
        //    {
        //        model.Id = prefs;
        //        preferenceId = await UpdateDemographicProfile(model);
        //    }
        //    return preferenceId;
        //}



        //public async Task<int> InsertAdvertProfile(CampaignProfileAdvertFormModel model)
        //{

        //    var prefs = await GetCampaignProfilePreferenceId(model.CampaignProfileId);
        //    int preferenceId = 0;
        //    if (prefs == 0)
        //    {
        //        try
        //        {
        //            preferenceId = await _executers.ExecuteCommand(_connStr,
        //                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertAdvertProfile, model));

        //            var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

        //            var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
        //            List<string> conns = lst.ToList();

        //            foreach (string constr in conns)
        //            {
        //                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

        //                model.CampaignProfileId = campaignId;

        //                var x = await _executers.ExecuteCommand(constr,
        //                                conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertAdvertProfile, model));
        //            }
        //        }
        //        catch
        //        {
        //            throw;
        //        }
        //    }
        //    else
        //    {
        //        model.CampaignProfileAdvertsId = prefs;
        //        preferenceId = await UpdateAdvertProfile(model);
        //    }
        //    return preferenceId;
        //}




        //public async Task<int> InsertMatchCampaignDemographic(CampaignProfileDemographicsFormModel model)
        //{
        //    int x = 0;
        //    try
        //    {
        //        var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

        //        var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
        //        List<string> conns = lst.ToList();

        //        foreach (string constr in conns)
        //        {
        //            var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

        //            var matchDetails = await _executers.ExecuteCommand(constr,
        //                            conn => conn.Query<NewCampaignProfileFormModel>(UserMatchQuery.GetCampaignMatchesDetailsById, new
        //                            {
        //                                Id = campaignId
        //                            }));

        //            if (matchDetails == null)
        //                x = await _executers.ExecuteCommand(constr,
        //                                conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertMatchCampaignDemographic, new
        //                                {
        //                                    Id = campaignId,
        //                                    Age_Demographics = model.Age_Demographics,
        //                                    Education_Demographics = model.Education_Demographics,
        //                                    Gender_Demographics = model.Gender_Demographics,
        //                                    HouseholdStatus_Demographics = model.HouseholdStatus_Demographics,
        //                                    IncomeBracket_Demographics = model.IncomeBracket_Demographics,
        //                                    RelationshipStatus_Demographics = model.RelationshipStatus_Demographics,
        //                                    WorkingStatus_Demographics = model.WorkingStatus_Demographics,
        //                                }));
        //            else
        //                x = await UpdateMatchCampaignDemographic(model);
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return x;
        //}


        //public async Task<int> InsertQuestionnaireProfile(CampaignProfileSkizaFormModel model)
        //{

        //    var prefs = await GetCampaignProfilePreferenceId(model.CampaignProfileId);
        //    int preferenceId = 0;
        //    if (prefs == 0)
        //    {
        //        string adpref = null;
        //        try
        //        {
        //            preferenceId = await _executers.ExecuteCommand(_connStr,
        //                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertQuestionnaireProfile, new
        //                                    {
        //                                        CampaignProfileId = model.CampaignProfileId,
        //                                        CountryId = model.CountryId,
        //                                        DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType,
        //                                        MassQuestion = model.MassQuestion,
        //                                        Hustlers_AdType = model.Hustlers_AdType,
        //                                        Youth_AdType = model.Youth_AdType,
        //                                        AdtoneServerCampaignProfilePrefId = adpref
        //                                    }));

        //            var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

        //            var lst = await _connService.GetConnectionStringsByCountry(campaignDetails.CountryId.Value);
        //            List<string> conns = lst.ToList();

        //            foreach (string constr in conns)
        //            {
        //                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);
        //                var countryId = await _connService.GetCountryIdFromAdtoneId(model.CountryId, constr);

        //                var x = await _executers.ExecuteCommand(constr,
        //                                conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertQuestionnaireProfile, new
        //                                {
        //                                    CampaignProfileId = campaignId,
        //                                    CountryId = countryId,
        //                                    DiscerningProfessionals_AdType = model.DiscerningProfessionals_AdType,
        //                                    MassQuestion = model.MassQuestion,
        //                                    Hustlers_AdType = model.Hustlers_AdType,
        //                                    Youth_AdType = model.Youth_AdType,
        //                                    AdtoneServerCampaignProfilePrefId = preferenceId
        //                                }));
        //            }
        //        }
        //        catch
        //        {
        //            throw;
        //        }
        //    }
        //    else
        //    {
        //        model.CampaignProfileSKizaId = prefs;
        //        preferenceId = await UpdateQuestionnaireProfile(model);
        //    }
        //    return preferenceId;
        //}



    }

}
