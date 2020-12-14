using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{

    public static class UserMatchQuery
    {
        public static string UpdateMediaLocation => "UPDATE CampaignMatches SET MEDIA_URL=@media WHERE MSCampaignProfileId=@campaignProfileId";

        public static string GetBudgetUpdateAmount => "SELECT MaxHourlyBudget, MaxBid FROM CampaignProfile WHERE CampaignProfileId=@Id";

        public static string UpdateBucketAmount => "Update CampaignProfile SET BucketCount=@BucketCount WHERE CampaignProfileId=@Id";


        public static string InsertNewCampaignData => @"INSERT INTO CampaignMatches(UserId,ClientId,CampaignName,CampaignDescription,TotalBudget,
                                                    MaxDailyBudget,MaxBid,MaxMonthBudget,MaxWeeklyBudget,MaxHourlyBudget,TotalCredit,
                                                    AvailableCredit,PlaysToDate,PlaysLastMonth,PlaysCurrentMonth,CancelledToDate,CancelledLastMonth,CancelledCurrentMonth,
                                                    SmsToDate,SmsLastMonth,SmsCurrentMonth,EmailToDate,EmailsLastMonth,EmailsCurrentMonth,
                                                    EmailFileLocation,Active,NumberOfPlays,AverageDailyPlays,SmsRequests,EmailsDelievered,
                                                    EmailSubject,EmailBody,EmailSenderAddress,SmsOriginator,SmsBody,SMSFileLocation,CreatedDateTime,
                                                    UpdatedDateTime,Status,StartDate,EndDate,NumberInBatch,CountryId,RemainingMaxMonthBudget,
                                                    RemainingMaxDailyBudget,RemainingMaxWeeklyBudget,RemainingMaxHourlyBudget,
                                                    NextStatus,MSCampaignProfileId,EMAIL_MESSAGE,SMS_MESSAGE,ORIGINATOR)
                                                    
                                                    VALUES(@UserId,@ClientId,@CampaignName,@CampaignDescription,@TotalBudget,@MaxDailyBudget,
                                                    @MaxBid,@MaxMonthBudget,@MaxWeeklyBudget,@MaxHourlyBudget,@TotalCredit,
                                                    @AvailableCredit,@PlaysToDate,@PlaysLastMonth,@PlaysCurrentMonth,@CancelledToDate,@CancelledLastMonth,
                                                    @CancelledCurrentMonth,@SmsToDate,@SmsLastMonth,@SmsCurrentMonth,@EmailToDate,@EmailsLastMonth,
                                                    @EmailsCurrentMonth,@EmailFileLocation,@Active,@NumberOfPlays,@AverageDailyPlays,@SmsRequests,
                                                    @EmailsDelievered,@EmailSubject,@EmailBody,@EmailSenderAddress,@SmsOriginator,@SmsBody,
                                                    @SMSFileLocation,GETDATE(),GETDATE(),@Status,@StartDate,@EndDate,@NumberInBatch,
                                                    @CountryId,@RemainingMaxMonthBudget,@RemainingMaxDailyBudget,@RemainingMaxWeeklyBudget,
                                                    @RemainingMaxHourlyBudget,@NextStatus,
                                                    @CampaignProfileId,@EmailBody,@SmsBody,@SmsOriginator);
                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";


//        public static string UpdateNewCampaignData => @"UPDATE CampaignMatches SET UserId=@UserId,ClientId=@ClientId,CampaignName,CampaignDescription,TotalBudget,
//                                                    MaxDailyBudget,MaxBid,MaxMonthBudget,MaxWeeklyBudget,MaxHourlyBudget,TotalCredit,
//                                                    AvailableCredit,PlaysToDate,PlaysLastMonth,PlaysCurrentMonth,CancelledToDate,CancelledLastMonth,CancelledCurrentMonth,
//                                                    SmsToDate,SmsLastMonth,SmsCurrentMonth,EmailToDate,EmailsLastMonth,EmailsCurrentMonth,
//                                                    EmailFileLocation,Active,NumberOfPlays,AverageDailyPlays,SmsRequests,EmailsDelievered,
//                                                    EmailSubject,EmailBody,EmailSenderAddress,SmsOriginator,SmsBody,SMSFileLocation,CreatedDateTime,
//                                                    UpdatedDateTime,Status,StartDate,EndDate,NumberInBatch,CountryId,RemainingMaxMonthBudget,
//                                                    RemainingMaxDailyBudget,RemainingMaxWeeklyBudget,RemainingMaxHourlyBudget,
//                                                    NextStatus,MSCampaignProfileId,EMAIL_MESSAGE,SMS_MESSAGE,ORIGINATOR
//WHERE CampaignProfileId=@id ";
    }
}
