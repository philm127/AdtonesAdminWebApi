

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface ICampaignQuery
    {
        string GetCampaignResultSet { get; }
        string GetPromoCampaignResultSet { get; }
        string GetCampaignCreditResultSet { get; }
        //string UpdateAdvertStatus { get; }
        //string InsertAdvertRejection { get; }
    }


    public class CampaignQuery : ICampaignQuery
    {
        public string GetCampaignResultSet => @"SELECT camp.CampaignProfileId,camp.UserId,u.Email,CONCAT(u.FirstName,'',u.LastName) AS UserName,op.OperatorName
                                                ,camp.ClientId, ISNULL(cl.Name,'-') AS ClientName,CampaignName,TotalBudget,camp.CreatedDateTime AS CreatedDate
                                                ,camp.IsAdminApproval,ad.AdvertId, ad.AdvertName,camp.TotalBudget,
                                                CASE WHEN bill.Id>0 THEN camp.Status ELSE 8 END AS Status,play.AvgBidValue,play.TotalSpend,
                                                (camp.TotalBudget - play.TotalSpend) AS FundsAvailable,play.ct AS finaltotalplays
                                                FROM CampaignProfile AS camp LEFT JOIN Users As u ON u.UserId=camp.UserId
                                                LEFT JOIN Client AS cl ON camp.ClientId=cl.Id
                                                LEFT JOIN 
		                                                (SELECT AdvertId,AdvertName,CampProfileId FROM Advert WHERE AdvertId in
			                                                (SELECT MAX(AdvertId) FROM Advert GROUP BY CampProfileId)
		                                                ) AS ad 
                                                ON ad.CampProfileId=camp.CampaignProfileId
                                                LEFT JOIN 
		                                                (SELECT Id,CampaignProfileId FROM Billing WHERE Id in
			                                                (SELECT MAX(Id) FROM Billing GROUP BY CampaignProfileId)
		                                                ) AS bill 
                                                ON bill.CampaignProfileId=camp.CampaignProfileId
                                                LEFT JOIN 
		                                                (SELECT CampaignProfileId,COUNT(CampaignProfileId) AS ct,CAST(AVG(BidValue) AS NUMERIC(36,2)) AS AvgBidValue,
                                                            CAST(ISNULL((ISNULL(SUM(BidValue),0))+(ISNULL(SUM(SMSCost),0))+(ISNULL(SUM(EmailCost),0)),0) AS NUMERIC(36,2)) AS TotalSpend 
			                                                FROM CampaignAudit
			                                                WHERE LOWER(Status)='played' AND PlayLengthTicks>6000
			                                                GROUP BY CampaignProfileId
		                                                ) AS play
                                                ON camp.CampaignProfileId=play.CampaignProfileId
                                                LEFT JOIN Operators AS op ON op.CountryId=camp.CountryId;";


        public string GetPromoCampaignResultSet =>  @"SELECT promo.ID,promo.OperatorID,op.OperatorName,promo.CampaignName,promo.BatchID,MaxDaily,MaxWeekly,
                                                    promo.AdvertLocation,promo.Status,pa.AdvertName,
                                                    CASE WHEN promo.Status=1 THEN 'Play' ELSE 'Stop' END AS rStatus
                                                    FROM PromotionalCampaigns AS promo 
                                                    LEFT JOIN PromotionalAdverts AS pa ON pa.CampaignID=promo.ID
                                                    LEFT JOIN Operators AS op ON op.OperatorId=promo.OperatorID
                                                    ORDER BY promo.ID DESC;";


        public string GetCampaignCreditResultSet => @"SELECT CampaignCreditPeriodId,ccp.UserId,CONCAT(usr.FirstName,' ',usr.LastName) AS UserName,
                                                            ccp.CampaignProfileId,camp.CampaignName,CreditPeriod,ccp.CreatedDate
                                                    FROM CampaignCreditPeriods AS ccp LEFT JOIN Users AS usr ON ccp.UserId=usr.UserId
                                                    LEFT JOIN CampaignProfile AS camp ON camp.CampaignProfileId=ccp.CampaignProfileId
                                                    ORDER BY CreatedDate DESC;";



        }
}
