

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class CampaignAuditQuery
    {

		public static string GetCampaignDashboardSummariesFROMCampaignAudit => @"( SELECT CampaignProfileId,SUM(TotalCost) AS TotalPlayedCost,
																AVG(BidValue) AS TotalAvgBid,
																SUM(TotalSMS) AS TotalSMS,
																SUM(TotalSMSCost) AS TotalSMSCost,
																SUM(TotalEmail) AS TotalEmail,
																SUM(TotalEmailCost) AS TotalEmailCost,
																count(CampaignProfileId) AS TotalPlayTracks  
																FROM(
																	 " + GetCampaignDashboardSummariesUNIONForCosts + @"
																	FROM CampaignAudit WHERE PlayLengthTicks >= 6000 AND Proceed = 1

																	UNION ALL
																	" + GetCampaignDashboardSummariesUNIONForCosts + @"
																	FROM CampaignAudit2 WHERE PlayLengthTicks >= 6000 AND Proceed = 1

																	UNION ALL
																	" + GetCampaignDashboardSummariesUNIONForCosts + @"
																	FROM CampaignAudit3 WHERE PlayLengthTicks >= 6000 AND Proceed = 1

																	UNION ALL
																	 " + GetCampaignDashboardSummariesUNIONForCosts + @"
																	FROM CampaignAudit4 WHERE PlayLengthTicks >= 6000 AND Proceed = 1

																	UNION ALL
																	 " + GetCampaignDashboardSummariesUNIONForCosts + @"
																	FROM CampaignAudit5 WHERE PlayLengthTicks >= 6000 AND Proceed = 1

																	UNION ALL
																	 " + GetCampaignDashboardSummariesUNIONForCosts + @"
																	FROM CampaignAudit6 WHERE PlayLengthTicks >= 6000 AND Proceed = 1

																	UNION ALL
																	 " + GetCampaignDashboardSummariesUNIONForCosts + @"
																	FROM CampaignAudit7 WHERE PlayLengthTicks >= 6000 AND Proceed = 1

																	UNION ALL
																	 " + GetCampaignDashboardSummariesUNIONForCosts + @"
																	FROM CampaignAudit8 WHERE PlayLengthTicks >= 6000 AND Proceed = 1

																	UNION ALL
																	 " + GetCampaignDashboardSummariesUNIONForCosts + @"
																	FROM CampaignAudit9 WHERE PlayLengthTicks >= 6000 AND Proceed = 1

																	UNION ALL
																	 " + GetCampaignDashboardSummariesUNIONForCosts + @"
																	FROM CampaignAudit10 WHERE PlayLengthTicks >= 6000 AND Proceed = 1
																	) as x
																	GROUP BY CampaignProfileId ) AS g 
														ON g.CampaignProfileId = cp.CampaignProfileId
														LEFT JOIN 
															( SELECT CampaignProfileId, SUM(UniqueListenrs) AS UniqueListenrs
																FROM (
																		 " + GetCampaignDashboardSummariesUNIONForListeners + @"
																		FROM CampaignAudit WHERE PlayLengthTicks >= 6000 AND Proceed = 1 GROUP BY CampaignProfileId

																		UNION ALL
																		" + GetCampaignDashboardSummariesUNIONForListeners + @"
																		FROM CampaignAudit2 WHERE PlayLengthTicks >= 6000 AND Proceed = 1 GROUP BY CampaignProfileId

																		UNION ALL
																		" + GetCampaignDashboardSummariesUNIONForListeners + @"
																		FROM CampaignAudit3 WHERE PlayLengthTicks >= 6000 AND Proceed = 1 GROUP BY CampaignProfileId

																		UNION ALL
																		 " + GetCampaignDashboardSummariesUNIONForListeners + @"
																		FROM CampaignAudit4 WHERE PlayLengthTicks >= 6000 AND Proceed = 1 GROUP BY CampaignProfileId

																		UNION ALL
																		 " + GetCampaignDashboardSummariesUNIONForListeners + @"
																		FROM CampaignAudit5 WHERE PlayLengthTicks >= 6000 AND Proceed = 1 GROUP BY CampaignProfileId

																		UNION ALL
																		 " + GetCampaignDashboardSummariesUNIONForListeners + @"
																		FROM CampaignAudit6 WHERE PlayLengthTicks >= 6000 AND Proceed = 1 GROUP BY CampaignProfileId

																		UNION ALL
																		 " + GetCampaignDashboardSummariesUNIONForListeners + @"
																		FROM CampaignAudit7 WHERE PlayLengthTicks >= 6000 AND Proceed = 1 GROUP BY CampaignProfileId

																		UNION ALL
																		 " + GetCampaignDashboardSummariesUNIONForListeners + @"
																		FROM CampaignAudit8 WHERE PlayLengthTicks >= 6000 AND Proceed = 1 GROUP BY CampaignProfileId

																		UNION ALL
																		 " + GetCampaignDashboardSummariesUNIONForListeners + @"
																		FROM CampaignAudit9 WHERE PlayLengthTicks >= 6000 AND Proceed = 1 GROUP BY CampaignProfileId

																		UNION ALL
																		 " + GetCampaignDashboardSummariesUNIONForListeners + @"
																		FROM CampaignAudit10 WHERE PlayLengthTicks >= 6000 AND Proceed = 1 GROUP BY CampaignProfileId
																		) as x
																		GROUP BY CampaignProfileId
															) AS r ON r.CampaignProfileId = cp.CampaignProfileId
														LEFT JOIN 
															(  SELECT CampaignProfileId, COUNT(*) AS TotalPlayTracks,AVG(PlayLengthTicks) AS AvgPlayLen,
																MAX(PlayLengthTicks) AS MaxPlayLen,MAX(BidValue) AS MaxBid
																FROM (
																		 " + GetCampaignDashboardSummariesUNIONForMaxBid + @"
																		FROM CampaignAudit WHERE Proceed = 1

																		UNION ALL
																		" + GetCampaignDashboardSummariesUNIONForMaxBid + @"
																		FROM CampaignAudit2 WHERE Proceed = 1

																		UNION ALL
																		" + GetCampaignDashboardSummariesUNIONForMaxBid + @"
																		FROM CampaignAudit3 WHERE Proceed = 1

																		UNION ALL
																		 " + GetCampaignDashboardSummariesUNIONForMaxBid + @"
																		FROM CampaignAudit4 WHERE Proceed = 1

																		UNION ALL
																		 " + GetCampaignDashboardSummariesUNIONForMaxBid + @"
																		FROM CampaignAudit5 WHERE Proceed = 1

																		UNION ALL
																		 " + GetCampaignDashboardSummariesUNIONForMaxBid + @"
																		FROM CampaignAudit6 WHERE Proceed = 1

																		UNION ALL
																		 " + GetCampaignDashboardSummariesUNIONForMaxBid + @"
																		FROM CampaignAudit7 WHERE Proceed = 1

																		UNION ALL
																		 " + GetCampaignDashboardSummariesUNIONForMaxBid + @"
																		FROM CampaignAudit8 WHERE Proceed = 1

																		UNION ALL
																		 " + GetCampaignDashboardSummariesUNIONForMaxBid + @"
																		FROM CampaignAudit9 WHERE Proceed = 1

																		UNION ALL
																		 " + GetCampaignDashboardSummariesUNIONForMaxBid + @"
																		FROM CampaignAudit10 WHERE Proceed = 1
																		) as x
																GROUP BY CampaignProfileId
															) AS p ON p.CampaignProfileId = cp.CampaignProfileId ";

		public static string GetCampaignDashboardSummariesUNIONForCosts => @"SELECT CampaignProfileId, TotalCost,BidValue,
																			CASE WHEN SMS IS NOT NULL THEN 1 ELSE 0 END AS TotalSMS,
																			ISNULL(SMSCost,0.00) AS TotalSMSCost,
																			CASE WHEN Email IS NOT NULL THEN 1 ELSE 0 END AS TotalEmail,
																			ISNULL(EmailCost,0.00) AS TotalEmailCost ";


		public static string GetCampaignDashboardSummariesUNIONForListeners => @"SELECT CampaignProfileId, COUNT(DISTINCT UserProfileId)  AS UniqueListenrs ";



		public static string GetCampaignDashboardSummariesUNIONForMaxBid => @"SELECT CampaignProfileId, PlayLengthTicks,BidValue ";

	}
}
