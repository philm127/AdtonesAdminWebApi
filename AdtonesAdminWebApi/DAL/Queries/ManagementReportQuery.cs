using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class ManagementReportQuery
    {
        public static string GetAmountCredit => @"SELECT cp.CampaignProfileId,0 AS TotalCost,ISNULL(TotalCredit,0) AS TotalCredit,CurrencyCode 
                                                FROM CampaignProfile AS cp
                                                INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                                WHERE CreatedDateTime BETWEEN @start AND @end AND op.OperatorId=@searchOperators ";



        public static string GetAmountSpent => @"SELECT cp.CampaignProfileId,ISNULL(SUM(cp.TotalCost),0) AS TotalCost,0 AS TotalCredit,CurrencyCode 
                                                    FROM CampaignProfile AS ca INNER JOIN (
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM (
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit WHERE AddedDate BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit2 WHERE AddedDate BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit3 WHERE AddedDate BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit4 WHERE AddedDate BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit5 WHERE AddedDate BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit6 WHERE AddedDate BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit7 WHERE AddedDate BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit8 WHERE AddedDate BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit9 WHERE AddedDate BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit10 WHERE AddedDate BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    ) as x
                                                    GROUP BY CampaignProfileId ) AS cp
                                                    ON cp.CampaignProfileId=ca.CampaignProfileId
                                                    INNER JOIN Operators AS op ON op.CountryId=ca.CountryId
                                                    AND op.OperatorId=@searchOperators ";


        public static string GetTotalCost => @"SELECT cp.CampaignProfileId,ISNULL(TotalCost,0) AS TotalCost,ISNULL(TotalCredit,0) AS TotalCredit,CurrencyCode 
                                                    FROM CampaignProfile AS cp LEFT JOIN (
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM (
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit2 GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit3 GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit4 GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit5 GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit6 GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit7 GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit8 GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit9 GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit10 GROUP BY CampaignProfileId
                                                    ) as x
                                                    GROUP BY CampaignProfileId ) AS ca
                                                    ON cp.CampaignProfileId=ca.CampaignProfileId
                                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                                    WHERE op.OperatorId=@searchOperators ";


        public static string TotalUsers => @"SELECT ISNULL(SUM(CASE WHEN Activated=1 THEN 1 ELSE 0 END),0) AS TotalUsers,
                                                ISNULL(SUM(CASE WHEN Activated=1 AND DateCreated BETWEEN @start AND @end THEN 1 ELSE 0 END),0) AS AddedUsers,
                                                ISNULL(SUM(CASE WHEN Activated = 3 THEN 1 ELSE 0 END),0) AS TotalRemovedUser
                                                FROM Users AS u INNER JOIN Operators AS op ON op.OperatorId=u.OperatorId 
                                                WHERE RoleId = 2  
                                                AND u.OperatorId=@searchOperators ";

        

        public static string TotalListened => @"SELECT COUNT(y.UserId) AS TotalItem,COUNT(z.UserId) AS NumItem FROM
                                                Users AS u
                                                INNER JOIN
                                                    (SELECT up.UserId FROM UserProfile AS up
	                                                    WHERE up.UserProfileId IN (SELECT DISTINCT UserProfileId FROM CampaignAudit)
                                                     ) AS y
                                                ON y.UserId=u.UserId
                                                LEFT JOIN
                                                    (SELECT up.UserId FROM UserProfile AS up
	                                                    WHERE up.UserProfileId IN (SELECT DISTINCT UserProfileId FROM CampaignAudit 
                                                                                         WHERE StartTime BETWEEN @start AND @end)
                                                        ) AS z
                                                ON z.UserId=u.UserId
                                                INNER JOIN Operators AS op ON op.OperatorId=u.OperatorId 
                                                WHERE u.RoleId=2 
                                                AND u.OperatorId=@searchOperators ";


        public static string ListenedUnionDistinct => @"SELECT DISTINCT UserProfileId from (
                                                SELECT DISTINCT UserProfileId FROM CampaignAudit
                                                UNION ALL
                                                SELECT DISTINCT UserProfileId FROM CampaignAudit2
                                                UNION ALL
                                                SELECT DISTINCT UserProfileId FROM CampaignAudit3
                                                UNION ALL
                                                SELECT DISTINCT UserProfileId FROM CampaignAudit4
                                                UNION ALL
                                                SELECT DISTINCT UserProfileId FROM CampaignAudit5
                                                UNION ALL
                                                SELECT DISTINCT UserProfileId FROM CampaignAudit6
                                                UNION ALL
                                                SELECT DISTINCT UserProfileId FROM CampaignAudit7
                                                UNION ALL
                                                SELECT DISTINCT UserProfileId FROM CampaignAudit8
                                                UNION ALL
                                                SELECT DISTINCT UserProfileId FROM CampaignAudit9
                                                UNION ALL
                                                SELECT DISTINCT UserProfileId FROM CampaignAudit10
                                                ) as x";


        public static string ListenedDateUnionDistinct => @"SELECT DISTINCT UserProfileId FROM (
                                                        SELECT DISTINCT UserProfileId FROM CampaignAudit WHERE StartTime BETWEEN @start AND @end
                                                        UNION ALL
                                                        SELECT DISTINCT UserProfileId FROM CampaignAudit2 WHERE StartTime BETWEEN @start AND @end
                                                        UNION ALL
                                                        SELECT DISTINCT UserProfileId FROM CampaignAudit3 WHERE StartTime BETWEEN @start AND @end
                                                        UNION ALL
                                                        SELECT DISTINCT UserProfileId FROM CampaignAudit4 WHERE StartTime BETWEEN @start AND @end
                                                        UNION ALL
                                                        SELECT DISTINCT UserProfileId FROM CampaignAudit5 WHERE StartTime BETWEEN @start AND @end
                                                        UNION ALL
                                                        SELECT DISTINCT UserProfileId FROM CampaignAudit6 WHERE StartTime BETWEEN @start AND @end
                                                        UNION ALL
                                                        SELECT DISTINCT UserProfileId FROM CampaignAudit7 WHERE StartTime BETWEEN @start AND @end
                                                        UNION ALL
                                                        SELECT DISTINCT UserProfileId FROM CampaignAudit8 WHERE StartTime BETWEEN @start AND @end
                                                        UNION ALL
                                                        SELECT DISTINCT UserProfileId FROM CampaignAudit9 WHERE StartTime BETWEEN @start AND @end
                                                        UNION ALL
                                                        SELECT DISTINCT UserProfileId FROM CampaignAudit10 WHERE StartTime BETWEEN @start AND @end
                                                        ) as x";

        
        

        public static string TotalPlayStuff => @"SELECT
                                                    ISNULL(SUM(TotOfPlaySixOver),0) AS TotOfPlaySixOver,
                                                    ISNULL(SUM(TotOfPlayUnderSix),0) AS TotOfPlayUnderSix,
                                                    ISNULL(SUM(TotPlaylength),0) AS TotPlaylength,
                                                    ISNULL(SUM(TotOfSMS),0) AS TotOfSMS,
                                                    ISNULL(SUM(TotOfEmail),0) AS TotOfEmail,
                                                    ISNULL(SUM(TotCancelled),0) AS TotCancelled,
                                                    ISNULL(SUM(NumOfPlaySixOver),0) AS NumOfPlaySixOver,
                                                    ISNULL(SUM(NumOfPlayUnderSix),0) AS NumOfPlayUnderSix,
                                                    ISNULL(SUM(Playlength),0) AS Playlength,
                                                    ISNULL(SUM(NumOfSMS),0) AS NumOfSMS,
                                                    ISNULL(SUM(NumOfEmail),0) AS NumOfEmail,
                                                    ISNULL(SUM(NumCancelled),0) AS NumCancelled
                                                    FROM (
                                                    SELECT CampaignProfileId,ISNULL(SUM(TotOfPlaySixOver),0) AS TotOfPlaySixOver,
                                                    ISNULL(SUM(TotOfPlayUnderSix),0) AS TotOfPlayUnderSix,
                                                    ISNULL(SUM(TotPlaylength),0) AS TotPlaylength,
                                                    ISNULL(SUM(TotOfSMS),0) AS TotOfSMS,
                                                    ISNULL(SUM(TotOfEmail),0) AS TotOfEmail,
                                                    ISNULL(SUM(TotCancelled),0) AS TotCancelled,
                                                    ISNULL(SUM(NumOfPlaySixOver),0) AS NumOfPlaySixOver,
                                                    ISNULL(SUM(NumOfPlayUnderSix),0) AS NumOfPlayUnderSix,
                                                    ISNULL(SUM(Playlength),0) AS Playlength,
                                                    ISNULL(SUM(NumOfSMS),0) AS NumOfSMS,
                                                    ISNULL(SUM(NumOfEmail),0) AS NumOfEmail,
                                                    ISNULL(SUM(NumCancelled),0) AS NumCancelled
                                                    FROM (
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit GROUP BY CampaignProfileId

                                                    UNION ALL
                                                    " + PlayStuffUnion + @"
                                                    FROM CampaignAudit2 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                    " + PlayStuffUnion + @"
                                                    FROM CampaignAudit3 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit4 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit5 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit6 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit7 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit8 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit9 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit10 GROUP BY CampaignProfileId
                                                    ) as x
                                                    GROUP BY CampaignProfileId ) AS ca
                                                    INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId 
                                                    WHERE op.OperatorId=@searchOperators ";


        public static string PlayStuffUnion => @"SELECT CampaignProfileId,ISNULL(SUM(CASE WHEN PlayLengthTicks >= 6000 THEN 1 ELSE 0 END),0) AS TotOfPlaySixOver,
                                                    ISNULL(SUM(CASE WHEN PlayLengthTicks < 6000 THEN 1 ELSE 0 END),0) AS TotOfPlayUnderSix,
                                                    ISNULL(SUM(ISNULL(PlayLengthTicks,0)),0) AS TotPlaylength,
                                                    ISNULL(SUM(CASE WHEN (PlayLengthTicks >= 6000 AND SMS IS NOT NULL) THEN 1 ELSE 0 END),0) AS TotOfSMS,
                                                    ISNULL(SUM(CASE WHEN (PlayLengthTicks >= 6000 AND EmailCost != 0) THEN 1 ELSE 0 END),0) AS TotOfEmail,
                                                    ISNULL(SUM(CASE WHEN Status= 'cancelled' THEN 1 ELSE 0 END),0) AS TotCancelled,
                                                    ISNULL(SUM(CASE WHEN PlayLengthTicks>=6000 AND StartTime 
                                                                        BETWEEN @start AND @end THEN 1 ELSE 0 END),0) AS NumOfPlaySixOver,
                                                    ISNULL(SUM(CASE WHEN PlayLengthTicks < 6000 AND StartTime BETWEEN @start AND @end THEN 1 ELSE 0 END),0) AS NumOfPlayUnderSix,
                                                    ISNULL(SUM(CASE WHEN PlayLengthTicks> 0 AND StartTime BETWEEN @start AND @end THEN PlayLengthTicks ELSE 0 END),0) AS Playlength,
                                                    ISNULL(SUM(CASE WHEN (PlayLengthTicks >= 6000 AND SMS IS NOT NULL AND StartTime BETWEEN @start AND @end) THEN 1 ELSE 0 END),0) AS NumOfSMS,
                                                    ISNULL(SUM(CASE WHEN (PlayLengthTicks >= 6000 AND EmailCost != 0 AND StartTime BETWEEN @start AND @end) THEN 1 ELSE 0 END),0) AS NumOfEmail,
                                                    ISNULL(SUM(CASE WHEN Status= 'cancelled' AND StartTime BETWEEN @start AND @end THEN 1 ELSE 0 END),0) AS NumCancelled";


        

        public static string TotalLiveCampaign => @"SELECT ISNULL(SUM(CASE WHEN cp.CampaignProfileId>0 THEN 1 ELSE 0 END),0) AS TotalItem,
                                                    ISNULL(SUM(CASE WHEN CreatedDateTime BETWEEN @start AND @end THEN 1 ELSE 0 END),0) AS NumItem
                                                    FROM CampaignProfile AS cp
                                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                                    WHERE cp.CampaignProfileId IN
                                                    (" + TotalCampaignDistinct + @")
                                                    AND op.OperatorId=@searchOperators ";

        public static string TotalCampaignDistinct => @"SELECT DISTINCT CampaignProfileId FROM (
                                                        SELECT CampaignProfileId FROM CampaignAudit
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit2
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit3
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit4
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit5
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit6
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit7
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit8
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit9
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit10
                                                        ) as x";


       

        public static string TotalSafRewards => @"SELECT SUM(x.IsRewardReceived) AS IsRewardReceivedTot,COUNT(DISTINCT x.UserProfileId) as UserProfileIdTot,
                                                    SUM(y.IsRewardReceived) AS IsRewardReceivedNum,COUNT(DISTINCT y.UserProfileId) as UserProfileIdNum
                                                    FROM UserProfile AS p
                                                    LEFT JOIN (
                                                                SELECT SUM(CAST(IsRewardReceived as integer)) AS IsRewardReceived,UserProfileId
                                                                FROM UserProfileAdvertsReceiveds WHERE IsRewardReceived=1 GROUP BY UserProfileId) AS x
												    ON x.UserProfileId=p.UserProfileId
                                                    LEFT JOIN (
                                                                SELECT SUM(CAST(IsRewardReceived as integer)) AS IsRewardReceived,UserProfileId
                                                                FROM UserProfileAdvertsReceiveds WHERE DateTimePlayed BETWEEN @start AND @end
                                                                    AND IsRewardReceived=1
												                    GROUP BY UserProfileId) AS y
                                                    ON y.UserProfileId=p.UserProfileId
                                                    INNER JOIN Users u ON u.UserId=p.UserId
                                                    INNER JOIN Operators AS op ON op.OperatorId=u.OperatorId
                                                    WHERE u.OperatorId=@searchOperators ";


       
        public static string TotalExpRewards => @"SELECT SUM(x.TotalItem) AS IsRewardReceivedTot,COUNT(DISTINCT x.UserId) as UserProfileIdTot,
                                                   SUM(y.TotalItem) AS IsRewardReceivedNum,COUNT(DISTINCT y.UserId) as UserProfileIdNum
                                                    FROM Users As u
                                                    LEFT JOIN 
                                                        ( SELECT COUNT(ClaimRewardAuditId) AS TotalItem,UserId
                                                          FROM ClaimRewardAudit GROUP BY UserId) AS x
                                                    ON u.UserId=x.UserId
                                                    LEFT JOIN 
                                                        ( SELECT COUNT(ClaimRewardAuditId) AS TotalItem,UserId
                                                            FROM ClaimRewardAudit WHERE EntryDateTimeUtc BETWEEN @start AND @end 
                                                            GROUP BY UserId) AS y
                                                    ON u.UserId=y.UserId
                                                    INNER JOIN Operators AS op ON op.OperatorId=u.OperatorId
                                                    WHERE u.OperatorId=@searchOperators ";


        public static string TotalAdsProvisioned => @"SELECT ISNULL(SUM(CASE WHEN AdvertId>0 THEN 1 ELSE 0 END),0) AS TotalItem,
                                                        ISNULL(SUM(CASE WHEN CreatedDateTime BETWEEN @start AND @end 
                                                                                THEN 1 ELSE 0 END),0) AS NumItem 
                                                        FROM Advert AS ad INNER JOIN Operators AS op ON op.OperatorId=ad.OperatorId
                                                        WHERE ad.OperatorId=@searchOperators ";


        public static string GetAllOperators => @"SELECT OperatorId FROM Operators;";


        public static string GetOperatorNameById => @"SELECT OperatorName FROM Operators WHERE OperatorId IN @searchOperators;";






        //public static string NumOfPlayStuff => @"SELECT ISNULL(SUM(CASE WHEN ca.PlayLengthTicks>=6000 THEN 1 ELSE 0 END),0) AS NumOfPlaySixOver,
        //                                    ISNULL(SUM(CASE WHEN ca.PlayLengthTicks < 6000 THEN 1 ELSE 0 END),0) AS NumOfPlayUnderSix,
        //                                    ISNULL(SUM(ISNULL(ca.PlayLengthTicks,0)),0) AS Playlength,
        //                                    ISNULL(SUM(CASE WHEN (ca.PlayLengthTicks >= 6000 AND ca.SMSCost != 0) THEN 1 ELSE 0 END),0) AS NumOfSMS,
        //                                    ISNULL(SUM(CASE WHEN (ca.PlayLengthTicks >= 6000 AND ca.EmailCost != 0) THEN 1 ELSE 0 END),0) AS NumOfEmail,
        //                                    ISNULL(SUM(CASE WHEN ca.Status= 'cancelled' THEN 1 ELSE 0 END),0) AS NumCancelled
        //                                    FROM CampaignAudit AS ca
        //                                    INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
        //                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
        //                                    WHERE StartTime BETWEEN @start AND @end AND op.OperatorId=@searchOperators ";




        //public static string NumLiveCampaign => @"SELECT ISNULL(COUNT(cp.CampaignProfileId),0) AS NumOfLiveCampaign
        //                                            FROM CampaignProfile AS cp
        //                                            INNER JOIN
        //                                            (SELECT DISTINCT(CampaignProfileId) FROM CampaignAudit) AS ca
        //                                            ON ca.CampaignProfileId=cp.CampaignProfileId
        //                                            INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
        //                                            WHERE CreatedDateTime BETWEEN @start AND @end AND op.OperatorId=@searchOperators ";


        //public static string NumberOfAdsProvisioned => @"SELECT ISNULL(COUNT(Advertid),0) AS NumberOfAdsProvisioned 
        //                                                FROM Advert AS ad INNER JOIN Operators AS op ON op.OperatorId=ad.OperatorId
        //                                                WHERE CreatedDateTime BETWEEN @start AND @end 
        //                                                AND op.OperatorId=@searchOperators ";


        //public static string NumOfPlayUnder6 => @"SELECT ISNULL(COUNT(CampaignAuditId),0) AS NumOfPlay,ISNULL(SUM(ca.PlayLengthTicks),0) AS Playlength 
        //                                            FROM CampaignAudit AS ca
        //                                            INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
        //                                            INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
        //                                            WHERE ca.Status= 'played' AND ca.PlayLengthTicks < 6000
        //                                            AND AddedDate>=@start AND AddedDate<=@end AND op.OperatorId=@searchOperators ";


        //public static string NumOfSMS => @"SELECT ISNULL(COUNT(CampaignAuditId),0) AS NumOfSMS FROM CampaignAudit AS ca
        //                                    INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
        //                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
        //                                    WHERE ca.Status= 'played' AND ca.PlayLengthTicks > 6000 AND ca.SMSCost != 0
        //                                    AND AddedDate>=@start AND AddedDate<=@end AND op.OperatorId=@searchOperators ";

        //public static string NumOfEmail => @"SELECT ISNULL(COUNT(CampaignAuditId),0) AS NumOfEmail FROM CampaignAudit AS ca
        //                                    INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
        //                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
        //                                    WHERE ca.Status= 'played' AND ca.PlayLengthTicks > 6000 AND ca.EmailCost != 0
        //                                    AND AddedDate>=@start AND AddedDate<=@end AND op.OperatorId=@searchOperators ";

        //public static string NumOfCancel => @"SELECT ISNULL(COUNT(CampaignAuditId),0) AS NumOfCancel FROM CampaignAudit AS ca
        //                                    INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
        //                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
        //                                    WHERE ca.Status= 'cancelled'
        //                                    AND AddedDate>=@start AND AddedDate<=@end AND op.OperatorId=@searchOperators ";


        //public static string NumOfTotalUser => @"SELECT ISNULL(COUNT(UserId),0) AS NumOfTotalUser FROM Users AS u LEFT JOIN Operators AS op ON op.OperatorId=u.OperatorId 
        //                                            WHERE RoleId = 2 AND Activated = 1 
        //                                            AND DateCreated>=@start AND DateCreated<=@end AND op.OperatorId=@searchOperators ";

        //public static string NumOfTotalUserForever => @"SELECT ISNULL(SUM(CASE WHEN Activated=1 THEN 1 ELSE 0 END),0) AS TotalUsers,
        //                                                ISNULL(SUM(CASE WHEN Activated = 3 THEN 1 ELSE 0 END),0) AS NumOfRemovedUser
        //                                                FROM Users AS u LEFT JOIN Operators AS op ON op.OperatorId=u.OperatorId 
        //                                                WHERE RoleId = 2 AND Activated = 1 AND op.OperatorId=@searchOperators ";

        //public static string NumOfRemovedUser => @"SELECT ISNULL(COUNT(UserId),0) AS NumOfRemovedUser FROM Users AS u LEFT JOIN Operators AS op ON op.OperatorId=u.OperatorId
        //                                            WHERE RoleId = 2 AND Activated = 3 AND DateCreated>=@start AND DateCreated<=@end 
        //                                            AND op.OperatorId=@searchOperators ";
        //public static string NumOfTextFile => @"SELECT ISNULL(SUM(NumOfTextFile),0) AS NumOfTextFile FROM ImportFileTracks  AS imp LEFT JOIN Operators AS op ON op.OperatorId=imp.OperatorId
        //                                        WHERE AddedDate>=@start AND AddedDate<=@end AND op.OperatorId=@searchOperators ";


        //public static string NumOfTextLine => @"SELECT ISNULL(SUM(NumOfTextLine),0) AS NumOfTextLine FROM ImportFileTracks AS imp LEFT JOIN Operators AS op ON op.OperatorId=imp.OperatorId
        //                                        WHERE AddedDate>=@start AND AddedDate<=@end AND op.OperatorId=@searchOperators ";


        // public static string NumOfUpdateToAudit => @"SELECT COUNT(Id) AS NumOfUpdateToAudit FROM Imports WHERE Proceed = 1 
        //                  AND AddedDate>=@start AND AddedDate<=@end ";


    }
}
