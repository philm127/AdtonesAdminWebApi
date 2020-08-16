

using Microsoft.Extensions.Primitives;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class PromotionalCampaignQuery
    {
        
        public static string GetPromoCampaignResultSet => @"SELECT promo.ID,promo.OperatorID AS OperatorId,op.OperatorName,promo.CampaignName,promo.BatchID,MaxDaily,MaxWeekly,
                                                    CASE WHEN promo.AdvertLocation IS NULL THEN promo.AdvertLocation 
                                                    ELSE CONCAT(@siteAddress,promo.AdvertLocation) END AS AdvertLocation,
                                                    promo.Status,pa.AdvertName,
                                                    CASE WHEN promo.Status=1 THEN 'Play' ELSE 'Stop' END AS rStatus
                                                    FROM PromotionalCampaigns AS promo 
                                                    LEFT JOIN PromotionalAdverts AS pa ON pa.CampaignID=promo.ID
                                                    LEFT JOIN Operators AS op ON op.OperatorId=promo.OperatorID
                                                    ORDER BY promo.ID DESC;";



        public static string UpdatePromotionalCampaignStatus => @"UPDATE PromotionalCampaigns SET Status=@Status WHERE ";


        public static string CheckExistingMSISDN => @"SELECT DISTINCT(msisdn) 
                                                FROM
                                                    (SELECT DISTINCT(msisdn) AS msisdn FROM PromotionalUsers
                                                    UNION ALL
                                                    SELECT DISTINCT(msisdn) AS msisdn FROM UserProfile) t";


        public static string CheckIfBatchExists => @"SELECT COUNT(1) FROM PromotionalUsers WHERE BatchId=@id";


        public static string CheckIfBatchInCampaignsExists => @"SELECT COUNT(1) FROM PromotionalCampaigns WHERE BatchID=@id AND OperatorId=@op;";


        public static string GetBatchIdForPromocampaign => @"SELECT DISTINCT BatchID AS Value, BatchID AS Text FROM PromotionalUsers
                                                        Where Status=1 AND BatchID NOT IN( SELECT BatchID FROM PromotionalCampaigns)";



        public static string AddPromoCampaign => @"INSERT INTO PromotionalCampaigns(OperatorID,CampaignName,BatchID,MaxDaily,MaxWeekly,AdvertLocation,
                                            Status,AdtoneServerPromotionalCampaignId)
                                            VALUES(@OperatorId,@CampaignName,@BatchID,@MaxDaily,@MaxWeekly,@AdvertLocation,
                                            @Status,@AdtoneServerPromotionalCampaignId);
                                            SELECT CAST(SCOPE_IDENTITY() AS INT);";


        public static string AddPromoAdvert => @"INSERT INTO PromotionalAdverts(CampaignID,AdvertName,AdvertLocation,AdtoneServerPromotionalAdvertId) 
                                            VALUES(@CampaignID,@AdvertName,@AdvertLocation,@AdtoneServerPromotionalAdvertId);
                                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

    }
}
