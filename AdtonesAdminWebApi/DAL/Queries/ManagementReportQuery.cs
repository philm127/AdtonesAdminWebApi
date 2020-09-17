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


        public static string GetTotalCost => @"SELECT ca.CampaignProfileId,SUM(TotalCost) AS TotalCost,TotalCredit,CurrencyCode 
                                                FROM CampaignAudit AS ca
                                                INNER JOIN CampaignProfile AS cp ON cp.CampaignProfileId=ca.CampaignProfileId
                                                INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                                AND CreatedDateTime>=@start AND CreatedDateTime<=@end AND op.OperatorId IN @searchOperators ";


        public static string NumOfTotalUser => @"SELECT COUNT(UserId) AS NumOfTotalUser FROM Users AS u LEFT JOIN Operators AS op ON op.OperatorId=u.OperatorId 
                                                    WHERE RoleId = 2 AND Activated = 1 
                                                    AND DateCreated>=@start AND DateCreated<=@end AND op.OperatorId IN @searchOperators ";
        public static string NumOfRemovedUser => @"SELECT COUNT(UserId) AS NumOfRemovedUser FROM Users AS u LEFT JOIN Operators AS op ON op.OperatorId=u.OperatorId
                                                    WHERE RoleId = 2 AND Activated = 3 AND DateCreated>=@start AND DateCreated<=@end 
                                                    AND op.OperatorId IN @searchOperators ";
        public static string NumOfTextFile => @"SELECT ISNULL(SUM(NumOfTextFile),0) AS NumOfTextFile FROM ImportFileTracks  AS imp LEFT JOIN Operators AS op ON op.OperatorId=imp.OperatorId
                                                WHERE AddedDate>=@start AND AddedDate<=@end AND op.OperatorId IN @searchOperators ";


        public static string NumOfTextLine => @"SELECT ISNULL(SUM(NumOfTextLine),0) AS NumOfTextLine FROM ImportFileTracks AS imp LEFT JOIN Operators AS op ON op.OperatorId=imp.OperatorId
                                                WHERE AddedDate>=@start AND AddedDate<=@end AND op.OperatorId IN @searchOperators ";


        // public static string NumOfUpdateToAudit => @"SELECT COUNT(Id) AS NumOfUpdateToAudit FROM Imports WHERE Proceed = 1 
                                  //                  AND AddedDate>=@start AND AddedDate<=@end ";


        public static string NumOfPlay => @"SELECT COUNT(CampaignAuditId) AS NumOfPlay FROM CampaignAudit AS ca
                                            INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                            INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                            WHERE ca.Status= 'played' AND ca.PlayLengthTicks >= 6000
                                            AND AddedDate>=@start AND AddedDate<=@end AND op.OperatorId IN @searchOperators ";

        public static string NumOfPlayUnder6 => @"SELECT COUNT(CampaignAuditId) AS NumOfPlay FROM CampaignAudit AS ca
                                            INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                            INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                            WHERE ca.Status= 'played' AND ca.PlayLengthTicks < 6000
                                            AND AddedDate>=@start AND AddedDate<=@end AND op.OperatorId IN @searchOperators ";


        public static string NumOfSMS => @"SELECT COUNT(CampaignAuditId) AS NumOfSMS FROM CampaignAudit AS ca
                                            INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                            INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                            WHERE ca.Status= 'played' AND ca.PlayLengthTicks > 6000 AND ca.SMSCost != 0
                                            AND AddedDate>=@start AND AddedDate<=@end AND op.OperatorId IN @searchOperators ";

        public static string NumOfEmail => @"SELECT COUNT(CampaignAuditId) AS NumOfEmail FROM CampaignAudit AS ca
                                            INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                            INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                            WHERE ca.Status= 'played' AND ca.PlayLengthTicks > 6000 AND ca.EmailCost != 0
                                            AND AddedDate>=@start AND AddedDate<=@end AND op.OperatorId IN @searchOperators ";

        public static string NumOfCancel => @"SELECT COUNT(CampaignAuditId) AS NumOfCancel FROM CampaignAudit AS ca
                                            INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                            INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                            WHERE ca.Status= 'cancelled'
                                            AND AddedDate>=@start AND AddedDate<=@end AND op.OperatorId IN @searchOperators ";

        public static string NumOfLiveCampaign => @"SELECT COUNT(cp.CampaignProfileId) AS NumOfLiveCampaign
                                                    FROM CampaignProfile AS cp
                                                    INNER JOIN
                                                    (SELECT DISTINCT(CampaignProfileId) FROM CampaignAudit WHERE AddedDate>=@start AND AddedDate<=@end) AS ca
                                                    ON ca.CampaignProfileId=cp.CampaignProfileId
                                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                                    WHERE op.OperatorId IN @searchOperators ";

        public static string NumberOfAdsProvisioned => @"SELECT COUNT(Advertid) AS NumberOfAdsProvisioned 
                                                        FROM Advert AS ad INNER JOIN Operators AS op ON op.OperatorId=ad.OperatorId
                                                        WHERE CreatedDateTime>=@start AND CreatedDateTime<=@end 
                                                        AND op.OperatorId IN @searchOperators ";


        public static string GetAllOperators => @"SELECT OperatorId FROM Operators;";


        public static string GetOperatorNameById => @"SELECT OperatorName FROM Operators WHERE OperatorId IN @searchOperators;";

    }
}
