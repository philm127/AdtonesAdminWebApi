using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class ManagementReportQuery
    {
        //public static string GetTotalCredit => @"SELECT CampaignProfileId,TotalCredit,CurrencyCode FROM CampaignProfile AS cp
        //                                        INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
        //                                        WHERE CampaignProfileId IN (SELECT DISTINCT(CampaignProfileId) FROM CampaignAudit)
        //                                        AND CreatedDateTime>=@start AND CreatedDateTime<=@end AND op.OperatorId IN @searchOperators ";


        public static string GetTotalCost => @"SELECT ca.CampaignProfileId,ISNULL(SUM(TotalCost),0) AS TotalCost,ISNULL(TotalCredit,0) AS TotalCredit,CurrencyCode 
                                                FROM CampaignProfile AS cp
                                                LEFT JOIN CampaignAudit AS ca ON cp.CampaignProfileId=ca.CampaignProfileId
                                                INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                                WHERE op.OperatorId IN @searchOperators ";


        //public static string NumOfTotalUser => @"SELECT ISNULL(COUNT(UserId),0) AS NumOfTotalUser FROM Users AS u LEFT JOIN Operators AS op ON op.OperatorId=u.OperatorId 
        //                                            WHERE RoleId = 2 AND Activated = 1 
        //                                            AND DateCreated>=@start AND DateCreated<=@end AND op.OperatorId IN @searchOperators ";

        //public static string NumOfTotalUserForever => @"SELECT ISNULL(SUM(CASE WHEN Activated=1 THEN 1 ELSE 0 END),0) AS NumOfTotalUser,
        //                                                ISNULL(SUM(CASE WHEN Activated = 3 THEN 1 ELSE 0 END),0) AS NumOfRemovedUser
        //                                                FROM Users AS u LEFT JOIN Operators AS op ON op.OperatorId=u.OperatorId 
        //                                                WHERE RoleId = 2 AND Activated = 1 AND op.OperatorId IN @searchOperators ";

        public static string TotalUsers => @"SELECT ISNULL(SUM(CASE WHEN Activated=1 THEN 1 ELSE 0 END),0) AS TotalNumOfUsers,
                                                ISNULL(SUM(CASE WHEN Activated = 3 THEN 1 ELSE 0 END),0) AS NumOfRemovedUser
                                                FROM Users AS u INNER JOIN Operators AS op ON op.OperatorId=u.OperatorId 
                                                WHERE RoleId = 2  
                                                AND op.OperatorId IN @searchOperators ";


        //public static string NumOfRemovedUser => @"SELECT ISNULL(COUNT(UserId),0) AS NumOfRemovedUser FROM Users AS u LEFT JOIN Operators AS op ON op.OperatorId=u.OperatorId
        //                                            WHERE RoleId = 2 AND Activated = 3 AND DateCreated>=@start AND DateCreated<=@end 
        //                                            AND op.OperatorId IN @searchOperators ";
        //public static string NumOfTextFile => @"SELECT ISNULL(SUM(NumOfTextFile),0) AS NumOfTextFile FROM ImportFileTracks  AS imp LEFT JOIN Operators AS op ON op.OperatorId=imp.OperatorId
        //                                        WHERE AddedDate>=@start AND AddedDate<=@end AND op.OperatorId IN @searchOperators ";


        //public static string NumOfTextLine => @"SELECT ISNULL(SUM(NumOfTextLine),0) AS NumOfTextLine FROM ImportFileTracks AS imp LEFT JOIN Operators AS op ON op.OperatorId=imp.OperatorId
        //                                        WHERE AddedDate>=@start AND AddedDate<=@end AND op.OperatorId IN @searchOperators ";


        // public static string NumOfUpdateToAudit => @"SELECT COUNT(Id) AS NumOfUpdateToAudit FROM Imports WHERE Proceed = 1 
                                  //                  AND AddedDate>=@start AND AddedDate<=@end ";


        public static string NumOfPlay => @"SELECT ISNULL(SUM(CASE WHEN ca.PlayLengthTicks >= 6000 THEN 1 ELSE 0 END),0) AS NumOfPlaySixOver,
                                            ISNULL(SUM(CASE WHEN ca.PlayLengthTicks < 6000 THEN 1 ELSE 0 END),0) AS NumOfPlayUnderSix,
                                            ISNULL(SUM(ISNULL(ca.PlayLengthTicks,0)),0) AS Playlength,ISNULL(COUNT(*),0) AS TotalPlays,
                                            ISNULL(SUM(CASE WHEN (ca.PlayLengthTicks >= 6000 AND ca.SMSCost != 0) THEN 1 ELSE 0 END),0) AS NumOfSMS,
                                            ISNULL(SUM(CASE WHEN (ca.PlayLengthTicks >= 6000 AND ca.EmailCost != 0) THEN 1 ELSE 0 END),0) AS NumOfEmail,
                                            ISNULL(SUM(CASE WHEN ca.Status= 'cancelled' THEN 1 ELSE 0 END),0) AS NumCancelled
                                            FROM CampaignAudit AS ca
                                            INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                            INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                            WHERE StartTime>=@start AND EndTime<=@end AND op.OperatorId IN @searchOperators ";

        //public static string NumOfPlayUnder6 => @"SELECT ISNULL(COUNT(CampaignAuditId),0) AS NumOfPlay,ISNULL(SUM(ca.PlayLengthTicks),0) AS Playlength 
        //                                            FROM CampaignAudit AS ca
        //                                            INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
        //                                            INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
        //                                            WHERE ca.Status= 'played' AND ca.PlayLengthTicks < 6000
        //                                            AND AddedDate>=@start AND AddedDate<=@end AND op.OperatorId IN @searchOperators ";


        //public static string NumOfSMS => @"SELECT ISNULL(COUNT(CampaignAuditId),0) AS NumOfSMS FROM CampaignAudit AS ca
        //                                    INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
        //                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
        //                                    WHERE ca.Status= 'played' AND ca.PlayLengthTicks > 6000 AND ca.SMSCost != 0
        //                                    AND AddedDate>=@start AND AddedDate<=@end AND op.OperatorId IN @searchOperators ";

        //public static string NumOfEmail => @"SELECT ISNULL(COUNT(CampaignAuditId),0) AS NumOfEmail FROM CampaignAudit AS ca
        //                                    INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
        //                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
        //                                    WHERE ca.Status= 'played' AND ca.PlayLengthTicks > 6000 AND ca.EmailCost != 0
        //                                    AND AddedDate>=@start AND AddedDate<=@end AND op.OperatorId IN @searchOperators ";

        //public static string NumOfCancel => @"SELECT ISNULL(COUNT(CampaignAuditId),0) AS NumOfCancel FROM CampaignAudit AS ca
        //                                    INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
        //                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
        //                                    WHERE ca.Status= 'cancelled'
        //                                    AND AddedDate>=@start AND AddedDate<=@end AND op.OperatorId IN @searchOperators ";

        public static string NumOfLiveCampaign => @"SELECT ISNULL(COUNT(cp.CampaignProfileId),0) AS NumOfLiveCampaign
                                                    FROM CampaignProfile AS cp
                                                    INNER JOIN
                                                    (SELECT DISTINCT(CampaignProfileId) FROM CampaignAudit) AS ca
                                                    ON ca.CampaignProfileId=cp.CampaignProfileId
                                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                                    WHERE op.OperatorId IN @searchOperators ";

        public static string NumberOfAdsProvisioned => @"SELECT ISNULL(COUNT(Advertid),0) AS NumberOfAdsProvisioned 
                                                        FROM Advert AS ad INNER JOIN Operators AS op ON op.OperatorId=ad.OperatorId
                                                        WHERE CreatedDateTime>=@start AND CreatedDateTime<=@end 
                                                        AND op.OperatorId IN @searchOperators ";


        public static string GetAllOperators => @"SELECT OperatorId FROM Operators;";


        public static string GetOperatorNameById => @"SELECT OperatorName FROM Operators WHERE OperatorId IN @searchOperators;";

    }
}
