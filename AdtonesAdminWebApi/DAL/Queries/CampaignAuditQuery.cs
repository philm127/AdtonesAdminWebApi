

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface ICampaignAuditQuery
    {
		string GetCampaignDashboardSummaries { get; }
    }


    public class CampaignAuditQuery : ICampaignAuditQuery
    {
        public string GetCampaignDashboardSummaries => @"SELECT u.Userid,cp.CampaignProfileId,a.AdvertId,cp.CampaignName,a.AdvertName,
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
														cp.CurrencyCode AS CurrencyCode
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
														WHERE ";


        }
}
