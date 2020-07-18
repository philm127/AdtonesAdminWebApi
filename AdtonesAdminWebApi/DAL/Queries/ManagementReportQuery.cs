using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class ManagementReportQuery
    {
        public static string GetTotalCredit => @"SELECT CampaignProfileId,TotalCredit,CurrencyCode FROM CampaignProfile AS cp
                                                INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                                WHERE CampaignProfileId IN (SELECT DISTINCT(CampaignProfileId) FROM CampaignAudit)
                                                AND CreatedDateTime>=@start AND CreatedDateTime<=@end AND OperatorId IN @operators;";


        public static string GetTotalCost => @"SELECT ca.CampaignProfileId,SUM(TotalCost) AS TotalCost,TotalCredit,CurrencyCode 
                                                FROM CampaignAudit AS ca
                                                INNER JOIN CampaignProfile AS cp ON cp.CampaignProfileId=ca.CampaignProfileId
                                                INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                                AND CreatedDateTime>=@start AND CreatedDateTime<=@end AND OperatorId IN @operators
                                                GROUP BY ca.CampaignProfileId,CurrencyCode,TotalCredit;";


        public static string NumOfTotalUser => @"SELECT COUNT(UserId) AS NumOfTotalUser FROM Users WHERE RoleId = 2 AND Activated = 1 
                                                    AND DateCreated>=@start AND DateCreated<=@end AND OperatorId IN @operators;";
        
        
        public static string NumOfRemovedUser => @"SELECT COUNT(UserId) AS NumOfRemovedUser FROM Users 
                                                    WHERE RoleId = 2 AND Activated = 3 AND DateCreated>=@start AND DateCreated<=@end 
                                                    AND OperatorId IN @operators;";
        public static string NumOfTextFile => @"SELECT ISNULL(SUM(NumOfTextFile),0) AS NumOfTextFile FROM ImportFileTracks 
                                                WHERE AddedDate>=@start AND AddedDate<=@end AND OperatorId IN @operators;";


        public static string NumOfTextLine => @"SELECT ISNULL(SUM(NumOfTextLine),0) AS NumOfTextLine FROM ImportFileTracks 
                                                WHERE AddedDate>=@start AND AddedDate<=@end AND OperatorId IN @operators;";


        public static string NumOfUpdateToAudit => @"SELECT COUNT(Id) AS NumOfUpdateToAudit FROM Imports WHERE Proceed = 1 
                                                    AND AddedDate>=@start AND AddedDate<=@end;";


        public static string NumOfPlay => @"SELECT COUNT(CampaignAuditId) AS NumOfPlay FROM CampaignAudit AS ca
                                            INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                            INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                            WHERE ca.Status= 'played' AND ca.PlayLengthTicks > 6000
                                            AND AddedDate>=@start AND AddedDate<=@end AND OperatorId IN @operators;";

        public static string NumOfPlayUnder6 => @"SELECT COUNT(CampaignAuditId) AS NumOfPlay FROM CampaignAudit AS ca
                                            INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                            INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                            WHERE ca.Status= 'played' AND ca.PlayLengthTicks <= 6000
                                            AND AddedDate>=@start AND AddedDate<=@end AND OperatorId IN @operators;";


        public static string NumOfSMS => @"SELECT COUNT(CampaignAuditId) AS NumOfSMS FROM CampaignAudit AS ca
                                            INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                            INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                            WHERE ca.Status= 'played' AND ca.PlayLengthTicks > 6000 AND ca.SMSCost != 0
                                            AND AddedDate>=@start AND AddedDate<=@end AND OperatorId IN @operators;";

        public static string NumOfEmail => @"SELECT COUNT(CampaignAuditId) AS NumOfEmail FROM CampaignAudit AS ca
                                            INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                            INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                            WHERE ca.Status= 'played' AND ca.PlayLengthTicks > 6000 AND ca.EmailCost != 0
                                            AND AddedDate>=@start AND AddedDate<=@end AND OperatorId IN @operators;";

        public static string NumOfCancel => @"SELECT COUNT(CampaignAuditId) AS NumOfCancel FROM CampaignAudit AS ca
                                            INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                            INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                            WHERE ca.Status= 'cancelled'
                                            AND AddedDate>=@start AND AddedDate<=@end AND OperatorId IN @operators;";

        public static string NumOfLiveCampaign => @"SELECT COUNT(DISTINCT(ca.CampaignProfileId)) AS NumOfLiveCampaign
                                                    FROM CampaignAudit AS ca 
                                                    INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                                    WHERE AddedDate>=@start AND AddedDate<=@end AND OperatorId IN @operators;";

        public static string NumberOfAdsProvisioned => @"SELECT COUNT(Advertid) AS NumberOfAdsProvisioned 
                                                        FROM Advert AS ad WHERE CreatedDateTime>=@start AND CreatedDateTime<=@end 
                                                        AND OperatorId IN @operators;";


        public static string GetAllOperators => @"SELECT OperatorId FROM Operators;";


        public static string GetOperatorNameById => @"SELECT OperatorName FROM Operators WHERE OperatorId IN @operators;";

    }
}
