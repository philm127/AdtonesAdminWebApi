using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class CreateUpdateCampaignQuery
    {
        public static string InsertNewCampaign => @"INSERT INTO CampaignProfile(UserId,ClientId,CampaignName,CampaignDescription,TotalBudget,
                                                    MaxDailyBudget,MaxBid,MaxMonthBudget,MaxWeeklyBudget,MaxHourlyBudget,TotalCredit,SpendToDate,
                                                    AvailableCredit,PlaysToDate,PlaysLastMonth,PlaysCurrentMonth,CancelledToDate,CancelledLastMonth,CancelledCurrentMonth,
                                                    SmsToDate,SmsLastMonth,SmsCurrentMonth,EmailToDate,EmailsLastMonth,EmailsCurrentMonth,
                                                    EmailFileLocation,Active,NumberOfPlays,AverageDailyPlays,SmsRequests,EmailsDelievered,
                                                    EmailSubject,EmailBody,EmailSenderAddress,SmsOriginator,SmsBody,SMSFileLocation,CreatedDateTime,
                                                    UpdatedDateTime,Status,StartDate,EndDate,NumberInBatch,CountryId,IsAdminApproval,RemainingMaxMonthBudget,
                                                    RemainingMaxDailyBudget,RemainingMaxWeeklyBudget,RemainingMaxHourlyBudget,ProvidendSpendAmount,
                                                    BucketCount,PhoneticAlphabet,NextStatus,AdtoneServerCampaignProfileId,CurrencyCode)
                                                    
                                                    VALUES(@UserId,@ClientId,@CampaignName,@CampaignDescription,@TotalBudget,@MaxDailyBudget,
                                                    @MaxBid,@MaxMonthBudget,@MaxWeeklyBudget,@MaxHourlyBudget,@TotalCredit,@SpendToDate,
                                                    @AvailableCredit,@PlaysToDate,@PlaysLastMonth,@PlaysCurrentMonth,@CancelledToDate,@CancelledLastMonth,
                                                    @CancelledCurrentMonth,@SmsToDate,@SmsLastMonth,@SmsCurrentMonth,@EmailToDate,@EmailsLastMonth,
                                                    @EmailsCurrentMonth,@EmailFileLocation,@Active,@NumberOfPlays,@AverageDailyPlays,@SmsRequests,
                                                    @EmailsDelievered,@EmailSubject,@EmailBody,@EmailSenderAddress,@SmsOriginator,@SmsBody,
                                                    @SMSFileLocation,@CreatedDateTime,@UpdatedDateTime,@Status,@StartDate,@EndDate,@NumberInBatch,
                                                    @CountryId,@IsAdminApproval,@RemainingMaxMonthBudget,@RemainingMaxDailyBudget,@RemainingMaxWeeklyBudget,
                                                    @RemainingMaxHourlyBudget,@ProvidendSpendAmount,@BucketCount,@PhoneticAlphabet,@NextStatus,
                                                    @AdtoneServerCampaignProfileId,@CurrencyCode);
                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";



        public static string InsertNewCampaignFromExiting => @"INSERT INTO CampaignProfile(UserId,ClientId,CampaignName,CampaignDescription,TotalBudget,
                                                    MaxDailyBudget,MaxBid,MaxMonthBudget,MaxWeeklyBudget,MaxHourlyBudget,TotalCredit,SpendToDate,
                                                    AvailableCredit,PlaysToDate,PlaysLastMonth,PlaysCurrentMonth,CancelledToDate,CancelledLastMonth,CancelledCurrentMonth,
                                                    SmsToDate,SmsLastMonth,SmsCurrentMonth,EmailToDate,EmailsLastMonth,EmailsCurrentMonth,
                                                    EmailFileLocation,Active,NumberOfPlays,AverageDailyPlays,SmsRequests,EmailsDelievered,
                                                    EmailSubject,EmailBody,EmailSenderAddress,SmsOriginator,SmsBody,SMSFileLocation,CreatedDateTime,
                                                    UpdatedDateTime,Status,StartDate,EndDate,NumberInBatch,CountryId,IsAdminApproval,RemainingMaxMonthBudget,
                                                    RemainingMaxDailyBudget,RemainingMaxWeeklyBudget,RemainingMaxHourlyBudget,ProvidendSpendAmount,
                                                    BucketCount,PhoneticAlphabet,NextStatus,AdtoneServerCampaignProfileId,CurrencyCode)

                                                    SELECT UserId,ClientId,CampaignName,CampaignDescription,TotalBudget,
                                                    MaxDailyBudget,MaxBid,MaxMonthBudget,MaxWeeklyBudget,MaxHourlyBudget,TotalCredit,SpendToDate,
                                                    AvailableCredit,PlaysToDate,PlaysLastMonth,PlaysCurrentMonth,CancelledToDate,CancelledLastMonth,CancelledCurrentMonth,
                                                    SmsToDate,SmsLastMonth,SmsCurrentMonth,EmailToDate,EmailsLastMonth,EmailsCurrentMonth,
                                                    EmailFileLocation,Active,NumberOfPlays,AverageDailyPlays,SmsRequests,EmailsDelievered,
                                                    EmailSubject,EmailBody,EmailSenderAddress,SmsOriginator,SmsBody,SMSFileLocation,CreatedDateTime,
                                                    UpdatedDateTime,Status,StartDate,EndDate,NumberInBatch,CountryId,IsAdminApproval,RemainingMaxMonthBudget,
                                                    RemainingMaxDailyBudget,RemainingMaxWeeklyBudget,RemainingMaxHourlyBudget,ProvidendSpendAmount,
                                                    BucketCount,PhoneticAlphabet,NextStatus,AdtoneServerCampaignProfileId,CurrencyCode
                                                    FROM CampaignProfile WHERE CampaignProfileId=@Id ";


        public static string GetProfileTimeSettingsByCampId => @"SELECT CampaignProfileTimeSettingsId,CampaignProfileId,Monday,Tuesday
                                                                ,Wednesday,Thursday,Friday,Saturday,Sunday,AdtoneServerCampaignProfileTimeId
                                                                FROM CampaignProfileTimeSetting WHERE CampaignProfileId=@Id ";


        public static string AddProfileTimeSettings => @"INSERT INTO CampaignProfileTimeSetting (CampaignProfileId,Monday,Tuesday
                                                                ,Wednesday,Thursday,Friday,Saturday,Sunday,AdtoneServerCampaignProfileTimeId)
                                                                VALUES(@CampaignProfileId,@Monday,@Tuesday
                                                                ,@Wednesday,@Thursday,@Friday,@Saturday,@Sunday,@AdtoneServerCampaignProfileTimeId);
                                                                SELECT CAST(SCOPE_IDENTITY() AS INT);";



    }
}
