using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.Command;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class CampaignMatchDAL : BaseDAL, ICampaignMatchDAL
    {
        private readonly ICampaignDAL _campDAL;

        public CampaignMatchDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor,
            ICampaignDAL campDAL)
            : base(configuration, executers, connService, httpAccessor)
        {
            _campDAL = campDAL;
        }

        public async Task<int> UpdateMediaLocation(string conn, string media, int id)
        {
            var sqlUpdate = "UPDATE CampaignMatches SET MEDIA_URL=@media WHERE MSCampaignProfileId=@campaignProfileId";
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sqlUpdate);
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
                    return await connection.ExecuteScalarAsync<int>(sqlInsert, model);
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

        //public async Task<int> UpdateCampaignMatchesCredit(BillingPaymentModel model, string constr)
        //{
        //    string InsertMatchFinancial = @"UPDATE CampaignMatches SET TotalBudget=@TotalBudget,TotalCredit=@TotalCredit,
        //                                                AvailableCredit=@AvailableCredit,Status=@Status WHERE MSCampaignProfileId=@Id";
        //    var campModel = await GetCampaignProfileDetail(model.CampaignProfileId);
        //    var available = await _invDAL.GetAvailableCredit(model.AdvertiserId);
        //    int x = 0;
        //    try
        //    {

        //        if (constr != null && constr.Length > 10)
        //        {
        //            var campId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

        //            using (var connection = new SqlConnection(constr))
        //            {
        //                await connection.OpenAsync();
        //                x = await connection.ExecuteScalarAsync<int>(InsertMatchFinancial, new
        //                {
        //                    Id = campId,
        //                    Status = (int)Enums.CampaignStatus.Play,
        //                    TotalBudget = (campModel.TotalBudget + model.Fundamount),
        //                    TotalCredit = (campModel.TotalCredit + model.TotalAmount),
        //                    AvailableCredit = available
        //                });
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //    return x;
        //}


        #region GeographicProfile

        public async Task<int> UpdateMatchCampaignGeographic(CreateOrUpdateCampaignProfileGeographicCommand model, string constr)
        {
            int x = 0;
            string UpdateMatchCampaignGeographic = @"UPDATE CampaignMatches SET Location_Demographics=@Location_Demographics
                                                                WHERE MSCampaignProfileId=@Id";
            try
            {


                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                x = await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(UpdateMatchCampaignGeographic, new
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

        public async Task<int> UpdateMatchCampaignDemographic(CreateOrUpdateCampaignProfileDemographicsCommand model, string constr)
        {
            string UpdateMatchCampaignDemographic = @"UPDATE CampaignMatches SET Age_Demographics=@Age_Demographics,
                                                            Education_Demographics=@Education_Demographics,Gender_Demographics=@Gender_Demographics,
                                                            HouseholdStatus_Demographics=@HouseholdStatus_Demographics,IncomeBracket_Demographics=@IncomeBracket_Demographics,
                                                            RelationshipStatus_Demographics=@RelationshipStatus_Demographics,
                                                            WorkingStatus_Demographics=@WorkingStatus_Demographics
                                                            WHERE MSCampaignProfileId=@Id";
            int x = 0;
            try
            {
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                x = await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(UpdateMatchCampaignDemographic, new
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


        #region MobileProfile

        public async Task<int> UpdateMatchCampaignMobile(CreateOrUpdateCampaignProfileMobileCommand model, string constr)
        {
            string UpdateMatchCampaignMobile = @"UPDATE CampaignMatches SET ContractType_Mobile=@ContractType_Mobile,
                                                        Spend_Mobile=@Spend_Mobile WHERE MSCampaignProfileId=@Id";
            int x = 0;
            try
            {
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                x = await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(UpdateMatchCampaignMobile, new
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


        public async Task<int> UpdateMatchCampaignQuestionnaire(CreateOrUpdateCampaignProfileSkizaCommand model, string constr)
        {
            string UpdateMatchCampaignQuestionnaire = @"UPDATE CampaignMatches SET DiscerningProfessionals_AdType=@DiscerningProfessionals_AdType,
                                                            Mass_AdType=@Mass_AdType,Hustlers_AdType=@Hustlers_AdType,
                                                            Youth_AdType=@Youth_AdType
                                                            WHERE MSCampaignProfileId=@Id";
            int x = 0;
            try
            {
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                var campaignId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                x = await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(UpdateMatchCampaignQuestionnaire, new
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


        public async Task<int> UpdateMatchCampaignAdvert(CreateOrUpdateCampaignProfileAdvertCommand model, string constr)
        {
            string UpdateMatchCampaignAdvert = @"UPDATE CampaignMatches SET AlcoholicDrinks_Advert=@AlcoholicDrinks_Advert,Cinema_Advert=@Cinema_Advert,
                                                        CommunicationsInternet_Advert=@CommunicationsInternet_Advert,DIYGardening_Advert=@DIYGardening_Advert,
                                                        ElectronicsOtherPersonalItems_Advert=@ElectronicsOtherPersonalItems_Advert,
                                                        Environment_Advert=@Environment_Advert,FinancialServices_Advert=@FinancialServices_Advert,
                                                        Fitness_Advert=@Fitness_Advert,Food_Advert=@Food_Advert,GoingOut_Advert=@GoingOut_Advert,
                                                        HolidaysTravel_Advert=@HolidaysTravel_Advert,Householdproducts_Advert=@Householdproducts_Advert,
                                                        Motoring_Advert=@Motoring_Advert,Music_Advert=@Music_Advert,Newspapers_Advert=@Newspapers_Advert,
                                                        NonAlcoholicDrinks_Advert=@NonAlcoholicDrinks_Advert,PetsPetFood_Advert=@PetsPetFood_Advert,
                                                        PharmaceuticalChemistsProducts_Advert=@PharmaceuticalChemistsProducts_Advert,
                                                        Religion_Advert=@Religion_Advert,Shopping_Advert=@Shopping_Advert,
                                                        ShoppingRetailClothing_Advert=@ShoppingRetailClothing_Advert,
                                                        SocialNetworking_Advert=@SocialNetworking_Advert,SportsLeisure_Advert=@SportsLeisure_Advert,
                                                        SweetSaltySnacks_Advert=@SweetSaltySnacks_Advert,TV_Advert=@TV_Advert,TobaccoProducts_Advert=@TobaccoProducts_Advert,
                                                        ToiletriesCosmetics_Advert=@ToiletriesCosmetics_Advert,BusinessOrOpportunities_AdType=@BusinessOrOpportunities_AdType,
                                                        Gambling_AdType=@Gambling_AdType,Restaurants_AdType=@Restaurants_AdType,
                                                        Insurance_AdType=@Insurance_AdType,Furniture_AdType=@Furniture_AdType,
                                                        InformationTechnology_AdType=@InformationTechnology_AdType,Energy_AdType=@Energy_AdType,
                                                        Supermarkets_AdType=@Supermarkets_AdType,Healthcare_AdType=@Healthcare_AdType,
                                                        JobsAndEducation_AdType=@JobsAndEducation_AdType,Gifts_AdType=@Gifts_AdType,
                                                        AdvocacyOrLegal_AdType=@AdvocacyOrLegal_AdType,DatingAndPersonal_AdType=@DatingAndPersonal_AdType,
                                                        RealEstate_AdType=@RealEstate_AdType,Games_AdType=@Games_AdType 
                                                        WHERE MSCampaignProfileId=@CampaignProfileId";
            var cid = model.CampaignProfileId;
            int x = 0;
            try
            {
                // var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);

                model.CampaignProfileId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                x = await _executers.ExecuteCommand(constr,
                                conn => conn.ExecuteScalar<int>(UpdateMatchCampaignAdvert, model));
            }
            catch
            {
                throw;
            }
            model.CampaignProfileId = cid;
            return x;
        }

        #endregion


        public async Task<int> UpdateCampaignMatchCredit(CampaignCreditCommand model, List<string> conStrList)
        {
            string updateMatchFinancial = @"UPDATE CampaignMatches SET TotalBudget=@TotalBudget,TotalCredit=@TotalCredit,
                                             UpdatedDateTime=GETDATE(), Status=@Status, NextStatus=0, AvailableCredit=@AvailableCredit 
                                            WHERE MSCampaignProfileId=@Id";
            int x = 0;
            try
            {
                foreach (var constr in conStrList)
                {
                    if (constr != null && constr.Length > 2)
                    {
                        var campId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);
                        x = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(updateMatchFinancial, new
                                        {
                                            Id = campId,
                                            Status = model.Status,
                                            TotalBudget = model.TotalBudget,
                                            TotalCredit = model.TotalCredit,
                                            AvailableCredit = model.AvailableCredit
                                        }));
                    }
                }
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<int> UpdateCampaignMatch(int campaignProfileId, int operatorId, int status)
        {
            var operatorConnectionString = await _connService.GetConnectionStringByOperator(operatorId);

            var sb = "UPDATE CampaignMatches SET Status=@Status WHERE MSCampaignProfileId=@Id";
            try
            {
                var campId = await _connService.GetCampaignProfileIdFromAdtoneId(campaignProfileId, operatorId);

                return await _executers.ExecuteCommand(operatorConnectionString,
                                    conn => conn.ExecuteScalar<int>(sb, new { Id = campId, Status = status } ));
            }
            catch
            {
                throw;
            }
        }

    }
}
