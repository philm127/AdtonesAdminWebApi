using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class CreateUpdateCampaignDAL : BaseDAL, ICreateUpdateCampaignDAL
    {

        public CreateUpdateCampaignDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService,
                            IHttpContextAccessor httpAccessor) : base(configuration, executers, connService, httpAccessor)
        { }


        


        public async Task<int> AddProfileTimeSettings(CampaignProfileTimeSetting model, int countryId, int provCampaignId)
        {
            var x = 0;

            try
            {

                x = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.AddProfileTimeSettings,model));

                if(x > 0)
                {
                    var connList = await _connService.GetConnectionStringsByCountry(countryId);
                    if (connList != null)
                    {
                        foreach (var conn in connList)
                        {
                            model.CampaignProfileId = provCampaignId;
                            model.AdtoneServerCampaignProfileTimeId = x;

                            x = await _executers.ExecuteCommand(conn,
                                    conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.AddProfileTimeSettings, model));
                        }

                    }
                }
            }
            catch
            {
                throw;
            }

            return x;
        }



        public async Task<NewCampaignProfileFormModel> CreateNewCampaign(NewCampaignProfileFormModel model)
        {
            var x = 0;

            try
            {

                model.AdtoneServerCampaignProfileId = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.InsertNewCampaign, model));

                if (model.AdtoneServerCampaignProfileId != null && model.AdtoneServerCampaignProfileId > 0)
                {
                    var conn = await _connService.GetConnectionStringsByCountryId(model.CountryId);
                    if (conn != null && conn != "")
                        model.CampaignProfileId = await _executers.ExecuteCommand(conn,
                                conn => conn.ExecuteScalar<int>(CreateUpdateCampaignQuery.InsertNewCampaign, model));
                }
            }
            catch
            {
                throw;
            }

            return model;
        }


        public async Task<CampaignProfileTimeSetting> GetProfileTimeSettingsByCampId(int id)
        {

            var sb = new StringBuilder();
            var builder = new SqlBuilder();

            sb.Append(CreateUpdateCampaignQuery.GetProfileTimeSettingsByCampId);
            builder.AddParameters(new { Id = id });


            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<CampaignProfileTimeSetting>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }

    }
}
