﻿using System;
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


        public static string UpdateCampaignDetails => @"UPDATE CampaignProfile SET CampaignDescription=@CampaignDescription,
                                                    MaxDailyBudget=@MaxDailyBudget,MaxBid=@MaxBid,MaxMonthBudget=@MaxMonthBudget,
                                                    MaxWeeklyBudget=@MaxWeeklyBudget,MaxHourlyBudget=@MaxHourlyBudget,
                                                    EmailSubject=@EmailSubject,EmailBody=@EmailBody,SmsOriginator=@SmsOriginator,SmsBody=@SmsBody,
                                                    UpdatedDateTime=GETDATE(),StartDate=@StartDate,EndDate=@EndDate
                                                    WHERE CampaignProfileId=@CampaignProfileId ";



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



        public static string AddCampaignProfileEx => @"INSERT INTO CampaignProfileExt (CampaignProfileId,NonBillable,IsProfileCampaign,CampaignCategoryId)
                                                                VALUES(@CampaignProfileId,@NonBillable,@IsProfileCampaign,@CampaignCategoryId);
                                                                SELECT CAST(SCOPE_IDENTITY() AS INT);";


        public static string UpdateCampaignProfileEx => @"UPDATE CampaignProfileExt SET CampaignCategoryId=@CampaignCategoryId
                                                                WHERE CampaignProfileId=@CampaignProfileId";


        public static string GetClientDetails => @"SELECT Id, Name, UserId, Description, Email, ContactPhone FROM Client WHERE Id=@Id";


        public static string InsertNewClient => @"INSERT INTO Client(Name, UserId, Description, Email,CountryId, ContactPhone,AdtoneServerClientId,CreatedDate,UpdatedDate,Status,Budget)
                                                VALUES(@Name, @UserId, @Description, @Email,@CountryId,@ContactPhone,@AdtoneServerClientId,GETDATE(),GETDATE(),1,0.00);
                                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

    }
}
