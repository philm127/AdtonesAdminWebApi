

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class CampaignAuditQuery
    {
        public static string GetCampaignDashboardSummaries => @"SELECT u.Userid,cp.CampaignProfileId,a.AdvertId,cp.CampaignName,a.AdvertName,
														CONCAT(u.FirstName,' ',u.LastName, ' (', u.Email, ')') AS CampaignHolder,
														ISNULL(cp.TotalBudget,0) AS Budget, ISNULL(g.TotalPlayedCost,0) AS Spend,
														ISNULL(cp.TotalBudget,0) - ISNULL(g.TotalPlayedCost,0) AS FundsAvailable,
														ISNULL(g.TotalAvgCost,0) AS AvgBid,CAST(ISNULL(g.TotalSMS,0) AS bigint) AS TotalSMS,
														ISNULL(g.TotalSMSCost,0.00) AS TotalSMSCost,CAST(ISNULL(g.TotalEmail,0) AS bigint) AS TotalEmail,
														ISNULL(g.TotalEmailCost,0.00) AS TotalEmailCost,Cast(ISNULL(p.TotalPlayTracks,0) AS bigint) AS TotalPlays,
														Cast(ISNULL(g.TotalPlayTracks,0) AS bigint) AS MoreSixSecPlays,
														Cast(ISNULL(p.TotalPlayTracks,0) - ISNULL(g.TotalPlayTracks,0) AS bigint) AS FreePlays,
														Cast(ISNULL(p.AvgPlayLen,0) AS bigint) AS AvgPlayLength,
														Cast(ISNULL(p.MaxPlayLen,0) AS bigint) AS MaxPlayLength,
														Cast(ISNULL(r.UniqueListenrs,0) AS bigint) AS Reach,
														CAST(ISNULL(p.MaxBid, 0) AS numeric(16,2)) AS MaxBid,
														cp.CurrencyCode AS CurrencyCode,ctu.TotalReach
														FROM Users AS u 
														LEFT JOIN CampaignProfile AS cp ON cp.UserId = u.UserId
														LEFT JOIN Client AS c ON c.Id = cp.ClientId
														LEFT JOIN 
															(  SELECT ca.CampaignProfileId,CONVERT(numeric(16,2), SUM(ca.TotalCost)) AS TotalPlayedCost,
																CONVERT(numeric(16,2), AVG(ca.TotalCost)) AS TotalAvgCost,
																SUM(CASE WHEN ca.SMS IS NOT NULL THEN 1 ELSE 0 END) AS TotalSMS,
																CONVERT(numeric(16,2), SUM(ISNULL(ca.SMSCost,0.00))) AS TotalSMSCost,
																SUM(CASE WHEN ca.Email IS NOT NULL THEN 1 ELSE 0 END) AS TotalEmail,
																CONVERT(NUMERIC(16,2), SUM(ISNULL(ca.EmailCost,0.00))) AS TotalEmailCost,
																count(*) AS TotalPlayTracks 
																FROM CampaignAudit AS ca
																WHERE ca.PlayLengthTicks >= 6000 AND ca.Proceed = 1
																GROUP BY ca.CampaignProfileId
															) AS g ON g.CampaignProfileId = cp.CampaignProfileId
														LEFT JOIN 
															( SELECT ca.CampaignProfileId, COUNT(DISTINCT ca.UserProfileId) AS UniqueListenrs
																FROM CampaignAudit AS ca
																WHERE ca.PlayLengthTicks >= 6000 AND ca.Proceed = 1
																GROUP BY ca.CampaignProfileId
															) AS r ON r.CampaignProfileId = cp.CampaignProfileId
														LEFT JOIN 
															(  SELECT ca.CampaignProfileId, COUNT(*) AS TotalPlayTracks,AVG(ca.PlayLengthTicks) AS AvgPlayLen,
																MAX(ca.PlayLengthTicks) AS MaxPlayLen,MAX(ca.BidValue) AS MaxBid
																FROM CampaignAudit ca
																WHERE ca.Proceed = 1
																GROUP BY ca.CampaignProfileId
															) AS p ON p.CampaignProfileId = cp.CampaignProfileId
														LEFT JOIN 
															( SELECT DISTINCT AdvertId,CampaignProfileId FROM CampaignAdverts
															) AS cad ON cad.CampaignProfileId=cp.CampaignProfileId
														LEFT JOIN Advert AS a ON a.AdvertId = cad.AdvertId
														LEFT JOIN
															(SELECT COUNT(UserId) AS TotalReach,op.CountryId FROM Users AS u 
																INNER JOIN Operators AS op ON u.OperatorId=op.OperatorId 
																WHERE VerificationStatus=1 AND Activated=1 GROUP BY op.CountryId) AS ctu
														ON cp.CountryId=ctu.CountryId
														LEFT JOIN Operators as op ON op.CountryId=cp.CountryId
														WHERE ";


		public static string GetCampaignDashboardSummariesByOperator => @"SELECT ISNULL(cp.TotalBudget,0) AS Budget, ISNULL(g.TotalPlayedCost,0) AS Spend,
																	ISNULL(cp.TotalBudget,0) - ISNULL(g.TotalPlayedCost,0) AS FundsAvailable,
																	ISNULL(g.TotalAvgCost,0) AS AvgBid,CAST(ISNULL(g.TotalSMS,0) AS bigint) AS TotalSMS,
																	ISNULL(g.TotalSMSCost,0) AS TotalSMSCost,CAST(ISNULL(g.TotalEmail,0) AS bigint) AS TotalEmail,
																	ISNULL(g.TotalEmailCost,0) AS TotalEmailCost,Cast(ISNULL(p.TotalPlayTracks,0) AS bigint) AS TotalPlays,
																	Cast(ISNULL(g.TotalPlayTracks,0) AS bigint) AS MoreSixSecPlays,
																	ISNULL(p.TotalPlayTracks,0) - ISNULL(g.TotalPlayTracks,0) AS FreePlays,
																	ISNULL(p.AvgPlayLen,0) AS AvgPlayLength,
																	ISNULL(p.MaxPlayLen,0) AS MaxPlayLength,
																	ISNULL(r.UniqueListenrs,0) AS Reach,
																	CAST(ISNULL(p.MaxBid, 0) AS numeric(16,2)) AS MaxBid,
																	cp.CurrencyCode AS CurrencyCode,ctu.TotalReach
																	FROM 
																		(SELECT SUM(ISNULL(TotalBudget,0)) AS TotalBudget,CountryId,CurrencyCode FROM CampaignProfile
																		GROUP BY CountryId,CurrencyCode) AS cp
																	INNER JOIN
																		( SELECT cpi.CountryId,CONVERT(numeric(16,0), SUM(ca.TotalCost)) AS TotalPlayedCost,
																			CONVERT(numeric(16,0), AVG(ca.TotalCost)) AS TotalAvgCost,
																			SUM(CASE WHEN ca.SMS IS NOT NULL THEN 1 ELSE 0 END) AS TotalSMS,
																			CONVERT(numeric(16,0), SUM(ISNULL(ca.SMSCost,0))) AS TotalSMSCost,
																			SUM(CASE WHEN ca.Email IS NOT NULL THEN 1 ELSE 0 END) AS TotalEmail,
																			CONVERT(NUMERIC(16,0), SUM(ISNULL(ca.EmailCost,0))) AS TotalEmailCost,
																			count(*) AS TotalPlayTracks 
																			FROM CampaignAudit AS ca INNER JOIN CampaignProfile AS cpi ON cpi.CampaignProfileId=ca.CampaignProfileId
																			WHERE ca.PlayLengthTicks >= 6000 AND ca.Proceed = 1
																			GROUP BY cpi.CountryId
																		) AS g ON g.CountryId = cp.CountryId
																	LEFT JOIN 
																		( SELECT cpi.CountryId, COUNT(DISTINCT ca.UserProfileId) AS UniqueListenrs
																			FROM CampaignAudit AS ca INNER JOIN CampaignProfile AS cpi ON cpi.CampaignProfileId=ca.CampaignProfileId
																			WHERE ca.PlayLengthTicks >= 6000 AND ca.Proceed = 1
																			GROUP BY cpi.CountryId
																		) AS r ON r.CountryId = cp.CountryId
																	LEFT JOIN 
																		(  SELECT cpi.CountryId, COUNT(*) AS TotalPlayTracks,AVG(ca.PlayLengthTicks) AS AvgPlayLen,
																			MAX(ca.PlayLengthTicks) AS MaxPlayLen,MAX(ca.BidValue) AS MaxBid
																			FROM CampaignAudit AS ca INNER JOIN CampaignProfile AS cpi ON cpi.CampaignProfileId=ca.CampaignProfileId
																			WHERE ca.Proceed = 1
																			GROUP BY cpi.CountryId
																		) AS p ON p.CountryId = cp.CountryId

																	LEFT JOIN
																		(SELECT COUNT(UserId) AS TotalReach,op.CountryId FROM Users AS u 
																			INNER JOIN Operators AS op ON u.OperatorId=op.OperatorId 
																			WHERE VerificationStatus=1 AND Activated=1 GROUP BY op.CountryId) AS ctu
																	ON cp.CountryId=ctu.CountryId
																	LEFT JOIN Operators as op ON op.CountryId=cp.CountryId
																	WHERE op.OperatorId=@opId;";


		public static string GetPlayDetailsByCampaign => @"SELECT CAST(ISNULL(ca.TotalCost,0) AS NUMERIC(36,2)) AS TotalCost,CAST(ISNULL(ca.BidValue,0) AS NUMERIC(36,2)) AS PlayCost,
													CAST(ISNULL(ca.EmailCost,0) AS NUMERIC(36,2)) AS EmailCost,CAST(ISNULL(ca.SMSCost,0) AS NUMERIC(36,2)) AS SMSCost,
													ca.StartTime,ca.EndTime,CAST((ca.PlayLengthTicks / 1000) AS NUMERIC(36,2)) AS PlayLength,ca.Email AS EmailMsg,ca.SMS,
													up.UserId,cp.CurrencyCode,ad.AdvertName,CampaignAuditId
													FROM CampaignProfile AS cp INNER JOIN CampaignAudit AS ca ON ca.CampaignProfileId=cp.CampaignProfileId
													LEFT JOIN UserProfile AS up ON ca.UserProfileId=up.UserProfileId
													LEFT JOIN 
														(SELECT AdvertId,CampaignProfileId FROM CampaignAdverts WHERE AdvertId in
			                                                (SELECT MAX(AdvertId) FROM CampaignAdverts GROUP BY CampaignProfileId)
		                                                ) AS cad 
													ON cad.CampaignProfileId=ca.CampaignProfileId
													LEFT JOIN Advert AS ad ON ad.AdvertId=cad.AdvertId
													WHERE cp.Status != 5
													AND ca.Status='Played'
													AND cp.CampaignProfileId=@Id ";


		public static string GetPromoCampaignDashboard => @"SELECT (CAST((ticks.sumplay / audCT.totalPlayCount) as DECIMAL(9,2)) / 1000) AS AveragePlayTime,
													pc.CampaignName,audCT.totalPlayCount AS TotalPlayed,dist.totalReach AS Reach,op.OperatorName,
													pa.AdvertName,CONCAT(@siteAddress,pa.AdvertLocation) AS AdvertLocation
													FROM PromotionalCampaigns AS pc LEFT JOIN PromotionalCampaignAudits AS pca
													ON pc.ID=pca.PromotionalCampaignId
													INNER JOIN PromotionalAdverts AS pa
													ON pa.CampaignID=pca.PromotionalCampaignId
													LEFT JOIN 
														(SELECT COUNT(PromotionalCampaignAuditId)  AS totalPlayCount,pca.PromotionalCampaignId 
															FROM PromotionalCampaignAudits AS pca
															GROUP BY pca.PromotionalCampaignId) AS audCT
													ON audCT.PromotionalCampaignId=pc.ID
													LEFT JOIN
														(SELECT COUNT(DISTINCT(MSISDN)) AS totalReach, PromotionalCampaignId 
															FROM PromotionalCampaignAudits
															WHERE (DTMFKey != '0' AND DTMFKey IS NOT NULL) GROUP BY PromotionalCampaignId) AS dist
													ON dist.PromotionalCampaignId=pc.ID
													LEFT JOIN 
														(SELECT SUM(PlayLengthTicks) AS sumplay, PromotionalCampaignId FROM PromotionalCampaignAudits
															GROUP BY PromotionalCampaignId) AS ticks
													ON ticks.PromotionalCampaignId=pc.ID
													INNER JOIN Operators AS op ON op.OperatorId=pc.OperatorID
													WHERE pc.ID=@Id
													GROUP BY ticks.sumplay,pc.CampaignName,audCT.totalPlayCount,dist.totalReach,op.OperatorName,
													pa.AdvertName,pa.AdvertLocation";


		public static string GetPromoPlayDetails => @"SELECT ROUND(pca.PlayLengthTicks/1000,0) AS PlayLength,pa.AdvertName,
												pca.PromotionalCampaignAuditId AS AuditId,pca.MSISDN,pca.StartTime,ISNULL(pca.DTMFKey,'-') AS DTMFKey
												FROM PromotionalCampaignAudits AS pca
												INNER JOIN PromotionalAdverts AS pa
												ON pa.CampaignID=pca.PromotionalCampaignId
												WHERE pca.PromotionalCampaignId=@Id";


	}
}
