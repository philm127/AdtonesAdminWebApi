

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class CampaignQuery
    {
        public static string GetCampaignResultSet => @"SELECT camp.CampaignProfileId,camp.UserId,u.Email,CONCAT(u.FirstName,' ',u.LastName) AS UserName,op.OperatorName
                                                ,camp.ClientId, ISNULL(cl.Name,'-') AS ClientName,CampaignName,camp.CreatedDateTime AS CreatedDate
                                                ,camp.IsAdminApproval,ad.AdvertId, ad.AdvertName,camp.TotalBudget,u.Organisation,ctry.Name AS CountryName,
                                                CASE WHEN bill.Id>0 THEN camp.Status ELSE 8 END AS Status,play.AvgBidValue,play.TotalSpend,bill.CurrencyCode,
                                                (camp.TotalBudget - play.TotalSpend) AS FundsAvailable,play.ct AS finaltotalplays,con.MobileNumber
                                                FROM CampaignProfile AS camp LEFT JOIN Users As u ON u.UserId=camp.UserId
                                                LEFT JOIN Client AS cl ON camp.ClientId=cl.Id
                                                LEFT JOIN CampaignAdverts AS campAd ON campAd.CampaignProfileId=camp.CampaignProfileId
												LEFT JOIN Advert AS ad ON ad.AdvertId=campAd.AdvertId
                                                LEFT JOIN 
		                                                (SELECT Id,CampaignProfileId,CurrencyCode FROM Billing WHERE Id in
			                                                (SELECT MAX(Id) FROM Billing GROUP BY CampaignProfileId,CurrencyCode)
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
                                                LEFT JOIN Operators AS op ON op.CountryId=camp.CountryId
                                                LEFT JOIN Contacts AS con ON con.UserId=camp.UserId
                                                LEFT JOIN Country AS ctry ON ctry.Id=camp.CountryId ";


		public static string GetCampaignResultSetForProfile => @"SELECT camp.CampaignProfileId,op.OperatorName
                                                ,CampaignName,camp.CreatedDateTime AS CreatedDate
                                                ,camp.IsAdminApproval,ad.AdvertId, ad.AdvertName,
                                                camp.Status AS Status,
                                                play.ct AS finaltotalplays
                                                FROM CampaignProfile AS camp
                                                LEFT JOIN CampaignAdverts AS campAd ON campAd.CampaignProfileId=camp.CampaignProfileId
												LEFT JOIN Advert AS ad ON ad.AdvertId=campAd.AdvertId
                                                LEFT JOIN 
		                                                (SELECT CampaignProfileId,COUNT(CampaignProfileId) AS ct 
			                                                FROM CampaignAudit
			                                                WHERE LOWER(Status)='played' AND PlayLengthTicks>6000
			                                                GROUP BY CampaignProfileId
		                                                ) AS play
                                                ON camp.CampaignProfileId=play.CampaignProfileId
                                                LEFT JOIN Operators AS op ON op.CountryId=camp.CountryId
                                                LEFT JOIN Contacts AS con ON con.UserId=camp.UserId
                                                LEFT JOIN Country AS ctry ON ctry.Id=camp.CountryId ";


		public static string GetCampaignResultSetFromProv => @"SELECT camp.CampaignProfileId,ct,AvgBidValue,TotalSpend
										FROM CampaignProfile AS camp
										INNER JOIN (
												SELECT DISTINCT CampaignProfileId,SUM(ct) AS ct,CAST(AVG(AvgBidValue) AS NUMERIC(36,2)) AS AvgBidValue,
												SUM(CAST(ISNULL(ToTalSpend,0) AS NUMERIC(36,2))) AS TotalSpend 
												FROM 
												(
													SELECT CampaignProfileId,COUNT(CampaignProfileId) AS ct,CAST(AVG(BidValue) AS NUMERIC(36,2)) AS AvgBidValue,
													SUM(CAST(ISNULL(ToTalCost,0) AS NUMERIC(36,2))) AS TotalSpend FROM CampaignAudit 
													WHERE PlayLengthTicks >= 6000 GROUP BY CampaignProfileId
													UNION ALL
													SELECT CampaignProfileId,COUNT(CampaignProfileId) AS ct,CAST(AVG(BidValue) AS NUMERIC(36,2)) AS AvgBidValue,
													SUM(CAST(ISNULL(ToTalCost,0) AS NUMERIC(36,2))) AS TotalSpend FROM CampaignAudit2 
													WHERE PlayLengthTicks >= 6000 GROUP BY CampaignProfileId

													UNION ALL
													SELECT CampaignProfileId,COUNT(CampaignProfileId) AS ct,CAST(AVG(BidValue) AS NUMERIC(36,2)) AS AvgBidValue,
													SUM(CAST(ISNULL(ToTalCost,0) AS NUMERIC(36,2))) AS TotalSpend FROM CampaignAudit3 
													WHERE PlayLengthTicks >= 6000 GROUP BY CampaignProfileId

													UNION ALL
														SELECT CampaignProfileId,COUNT(CampaignProfileId) AS ct,CAST(AVG(BidValue) AS NUMERIC(36,2)) AS AvgBidValue,
													SUM(CAST(ISNULL(ToTalCost,0) AS NUMERIC(36,2))) AS TotalSpend FROM CampaignAudit4 
													WHERE PlayLengthTicks >= 6000 GROUP BY CampaignProfileId

													UNION ALL
														SELECT CampaignProfileId,COUNT(CampaignProfileId) AS ct,CAST(AVG(BidValue) AS NUMERIC(36,2)) AS AvgBidValue,
													SUM(CAST(ISNULL(ToTalCost,0) AS NUMERIC(36,2))) AS TotalSpend FROM CampaignAudit5 
													WHERE PlayLengthTicks >= 6000 GROUP BY CampaignProfileId

													UNION ALL
														SELECT CampaignProfileId,COUNT(CampaignProfileId) AS ct,CAST(AVG(BidValue) AS NUMERIC(36,2)) AS AvgBidValue,
													SUM(CAST(ISNULL(ToTalCost,0) AS NUMERIC(36,2))) AS TotalSpend FROM CampaignAudit6 
													WHERE PlayLengthTicks >= 6000 GROUP BY CampaignProfileId

													UNION ALL
														SELECT CampaignProfileId,COUNT(CampaignProfileId) AS ct,CAST(AVG(BidValue) AS NUMERIC(36,2)) AS AvgBidValue,
													SUM(CAST(ISNULL(ToTalCost,0) AS NUMERIC(36,2))) AS TotalSpend FROM CampaignAudit7 
													WHERE PlayLengthTicks >= 6000 GROUP BY CampaignProfileId

													UNION ALL
														SELECT CampaignProfileId,COUNT(CampaignProfileId) AS ct,CAST(AVG(BidValue) AS NUMERIC(36,2)) AS AvgBidValue,
													SUM(CAST(ISNULL(ToTalCost,0) AS NUMERIC(36,2))) AS TotalSpend FROM CampaignAudit8 
													WHERE PlayLengthTicks >= 6000 GROUP BY CampaignProfileId

													UNION ALL
														SELECT CampaignProfileId,COUNT(CampaignProfileId) AS ct,CAST(AVG(BidValue) AS NUMERIC(36,2)) AS AvgBidValue,
													SUM(CAST(ISNULL(ToTalCost,0) AS NUMERIC(36,2))) AS TotalSpend FROM CampaignAudit9 
													WHERE PlayLengthTicks >= 6000 GROUP BY CampaignProfileId

													UNION ALL
														SELECT CampaignProfileId,COUNT(CampaignProfileId) AS ct,CAST(AVG(BidValue) AS NUMERIC(36,2)) AS AvgBidValue,
													SUM(CAST(ISNULL(ToTalCost,0) AS NUMERIC(36,2))) AS TotalSpend FROM CampaignAudit10 
													WHERE PlayLengthTicks >= 6000 GROUP BY CampaignProfileId
													) as x
												GROUP BY CampaignProfileId ) AS play
												ON camp.CampaignProfileId=play.CampaignProfileId
                                                LEFT JOIN Operators AS op ON op.CountryId=camp.CountryId
                                                LEFT JOIN Contacts AS con ON con.UserId=camp.UserId
                                                LEFT JOIN Country AS ctry ON ctry.Id=camp.CountryId ";



		public static string GetCampaignResultSetForSales => @"SELECT camp.CampaignProfileId,camp.UserId,u.Email,CONCAT(u.FirstName,' ',u.LastName) AS UserName,op.OperatorName
                                                ,camp.ClientId, ISNULL(cl.Name,'-') AS ClientName,CampaignName,camp.CreatedDateTime AS CreatedDate
                                                ,camp.IsAdminApproval,ad.AdvertId, ad.AdvertName,camp.TotalBudget,u.Organisation,ctry.Name AS CountryName,
                                                CASE WHEN bill.Id>0 THEN camp.Status ELSE 8 END AS Status,play.AvgBidValue,play.TotalSpend,bill.CurrencyCode,
                                                (camp.TotalBudget - play.TotalSpend) AS FundsAvailable,play.ct AS finaltotalplays,con.MobileNumber,
                                                CASE WHEN sexcs.FirstName IS NULL THEN 'UnAllocated' ELSE CONCAT(sexcs.FirstName,' ',sexcs.LastName) 
                                                    END AS SalesExec,sexcs.UserId AS sUserId
                                                FROM CampaignProfile AS camp LEFT JOIN Users As u ON u.UserId=camp.UserId
                                                LEFT JOIN Client AS cl ON camp.ClientId=cl.Id
                                                LEFT JOIN CampaignAdverts AS campAd ON campAd.CampaignProfileId=camp.CampaignProfileId
												LEFT JOIN Advert AS ad ON ad.AdvertId=campAd.AdvertId
                                                LEFT JOIN 
		                                                (SELECT Id,CampaignProfileId,CurrencyCode FROM Billing WHERE Id in
			                                                (SELECT MAX(Id) FROM Billing GROUP BY CampaignProfileId,CurrencyCode)
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
                                                LEFT JOIN Operators AS op ON op.CountryId=camp.CountryId
                                                LEFT JOIN Contacts AS con ON con.UserId=camp.UserId
                                                LEFT JOIN Country AS ctry ON ctry.Id=camp.CountryId 
                                                LEFT JOIN Advertisers_SalesTeam AS sales ON camp.UserId=sales.AdvertiserId 
                                                LEFT JOIN Users AS sexcs ON sexcs.UserId=sales.SalesExecId ";



        public static string GetCampaignProfileById => @"SELECT CampaignProfileId,UserId,ClientId,CampaignName,CampaignDescription,
                                                    TotalBudget,MaxBid,TotalCredit,SpendToDate,AvailableCredit,PlaysToDate,
                                                    CancelledToDate,SmsToDate,EmailToDate,EmailFileLocation,Active,NumberOfPlays,
                                                    AverageDailyPlays,SmsRequests,EmailsDelievered,EmailSubject,EmailBody,SmsBody,
                                                    SMSFileLocation,CreatedDateTime,UpdatedDateTime,Status,StartDate,EndDate,
                                                    camp.CountryId,IsAdminApproval,ProvidendSpendAmount,AdtoneServerCampaignProfileId,
                                                    CurrencyCode
                                                    FROM CampaignProfile AS camp 
                                                    LEFT JOIN Operators AS op ON camp.CountryId=op.CountryId 
                                                    WHERE CampaignProfileId=@Id";


        public static string UpdateCampaignProfileStatus => @"UPDATE CampaignProfile SET Status=@Status,IsAdminApproval=1,
                                                        UpdatedDateTime = GETDATE() WHERE ";


        public static string GetCampaignAdvertDetailsById => @"SELECT CampaignAdvertId,CampaignProfileId,AdvertId,
                                                        NextStatus,AdtoneServerCampaignAdvertId
                                                        FROM CampaignAdverts WHERE ";

		
        public static string CheckCampaignBillingExists => @"SELECT COUNT(1) FROM Billing WHERE CampaignProfileId=@Id;";


		public static string CheckCampaignNameExists => @"SELECT COUNT(1) FROM CampaignProfile WHERE LOWER(CampaignName)=@Id AND UserId=@UserId;";


		public static string UpdateCampaignMatchStatus => @"UPDATE CampaignMatches SET Status=@Status WHERE MSCampaignProfileId=@Id";


        public static string GetAdvertIdFromCampaignAd => @"SELECT AdvertId FROM CampaignAdvert WHERE CampaignProfileId=@Id";

        public static string UpdateCampaignMatchFromBilling => @"UPDATE CampaignMatches SET UpdatedDateTime=GETDATE(), Status=@Status, NextStatus=0 WHERE MSCampaignProfileId=@Id";


        public static string UpdateCampaignBilling => @"UPDATE CampaignProfile SET UpdatedDateTime=GETDATE(), Status=@Status, TotalCredit=TotalCredit + @TotalCredit,TotalBudget = TotalBudget + @TotalBudget WHERE CampaignProfileId=@Id";
    }
}
