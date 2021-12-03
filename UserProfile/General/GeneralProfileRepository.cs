using System;
using System.Collections.Generic;
using System.Text;
using UserProfile.Services;

namespace UserProfile.General
{
    public class GeneralProfileRepository : IConnectionStrings,IGeneralProfileRepository
    {
        private readonly IConnectionStrings _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;
        private readonly IConnectionStringService _connService;
        private readonly ICampaignDAL _campDAL;
        private readonly ILoggingService _logServ;
        const string PageName = "UserMatchDAL";

        public GeneralProfileRepository(IConnectionStrings configuration, IExecutionCommand executers, IConnectionStringService connService, ICampaignDAL campDAL,
                            ILoggingService logServ)
        {
            _configuration = configuration;
            _connStr = _configuration.mainConnection;
            _executers = executers;
            _connService = connService;
            _campDAL = campDAL;
            _logServ = logServ;
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
                                                                                        new { Id = countryId, profileName = profileName }));
            }

        }


        public async Task<IEnumerable<string>> GetProfileMatchLabels(int infoId)
        {
            using (var connection = new SqlConnection(_connStr))
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<string>(UserMatchQuery.GetProfileMatchLabels, new { Id = infoId }));
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


        public async Task<int> AddCampaignMatchData(NewCampaignProfileFormModel model, string conn)
        {
            try
            {

                return await _executers.ExecuteCommand(conn,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.InsertNewCampaignMatchData, model));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateCampaignMatchData(NewCampaignProfileFormModel model, string conn)
        {
            var campId = model.CampaignProfileId;
            var x = 0;
            try
            {
                model.CampaignProfileId = await _connService.GetCampaignProfileIdFromAdtoneIdByConn(model.CampaignProfileId, conn);

                x = await _executers.ExecuteCommand(conn,
                                    conn => conn.ExecuteScalar<int>(UserMatchQuery.UpdateCampaignMatchData, model));
            }
            catch
            {
                throw;
            }
            model.CampaignProfileId = campId;
            return x;
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

    }

}