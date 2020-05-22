﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface IUserDashboardQuery
    {
        string AdvertiserResultQuery { get; }
        string OperatorResultQuery { get; }
        string SubscriberResultQuery { get; }
    }


    public class UserDashboardQuery : IUserDashboardQuery
    {
        public string AdvertiserResultQuery => @"SELECT item.UserId,item.RoleId,item.Email,item.FirstName,item.LastName,
                                                  ISNULL(camp.NoOfactivecampaign, 0) AS NoOfactivecampaign,
                                                   ISNULL(ad.NoOfunapprovedadverts, 0) AS NoOfunapprovedadverts,
                                                   ISNULL(cred.AssignCredit, 0) AS creditlimit,ISNULL(billit.outStandingInvoice, 0) AS outStandingInvoice,
                                                   item.Activated,item.DateCreated,ISNULL(tkt.TicketCount, 0) AS TicketCount
                                                   FROM
                                                        (SELECT item.UserId, item.RoleId, item.Email, item.DateCreated, item.Activated,
                                                        item.FirstName,item.LastName
                                                        FROM Users item Where item.VerificationStatus = 1 AND (item.RoleId = 3 OR item.RoleId = 4)) item
                                                    LEFT JOIN
                                                        (SELECT a.[UserId], b.[AssignCredit], a.[Id] 
                                                        FROM
                                                            (SELECT[UserId], MIN(Id) AS Id FROM UsersCredit GROUP BY[UserId]) a
                                                        INNER JOIN UsersCredit b ON a.[UserId] = b.[UserId] AND a.Id = b.Id) cred
                                                    ON item.UserId = cred.UserId
                                                    LEFT JOIN
                                                        (SELECT COUNT(UserId)as TicketCount, UserId FROM Question WHERE Status IN (1, 2) GROUP BY UserId) tkt
                                                    ON item.UserId = tkt.UserId
                                                    LEFT JOIN
                                                        (SELECT COUNT(CampaignProfileId) AS NoOfactivecampaign,UserId FROM Campaignprofile 
                                                            WHERE Status IN (4, 3, 2, 1) GROUP BY UserId) camp
                                                    ON item.UserId = camp.UserId
                                                    LEFT JOIN
                                                        (SELECT COUNT(AdvertId) AS NoOfunapprovedadverts,UserId FROM Advert WHERE Status = 4 GROUP BY UserId) ad
                                                    ON item.UserId = ad.UserId
                                                    LEFT JOIN
                                                        (SELECT COUNT(bill3.UserId) AS outStandingInvoice,bill3.UserId
                                                        FROM
                                                            (SELECT SUM(FundAmount) AS totalAmount, CampaignProfileId, UserId
                                                            FROM Billing WHERE PaymentMethodId = 1 GROUP BY CampaignProfileId, UserId) bill3
                                                        LEFT JOIN
                                                            (SELECT sum(Amount) AS paidAmount, UserId, CampaignProfileId
                                                            FROM UsersCreditPayment GROUP BY CampaignProfileId, UserId) uc
                                                        ON bill3.UserId = uc.UserId AND bill3.CampaignProfileId = uc.CampaignProfileId
                                                        WHERE (ISNULL(bill3.totalAmount, 0) - ISNULL(uc.paidAmount, 0)) > 0
                                                        GROUP BY bill3.UserId) billit
                                                    ON item.UserId = billit.UserId;";


        public string OperatorResultQuery => @"SELECT u.UserId,FirstName,LastName,Email,ISNULL(Organisation,'-') AS Organisation,u.OperatorId,o.CountryId,
                                                c.Name AS CountryName,o.OperatorName,u.Activated,u.DateCreated
                                                FROM Users AS u LEFT JOIN Operators AS o ON u.OperatorId=o.OperatorId
                                                LEFT JOIN Country AS c ON o.CountryId=c.Id
                                                WHERE RoleId=6 ORDER BY u.DateCreated DESC";


        public string SubscriberResultQuery => @"SELECT TOP 1000 u.UserId,u.Activated,u.DateCreated,FirstName,LastName,p.MSISDN,u.OperatorId,op.CountryId,u.Email
                                                  FROM Users AS u LEFT JOIN UserProfile AS p ON p.UserId=u.UserId
                                                  INNER JOIN Operators AS op ON op.OperatorId=u.OperatorId
                                                  WHERE u.RoleId=2 AND u.VerificationStatus=1 AND u.OperatorId IS NOT NULL
                                                  ORDER BY u.DateCreated DESC;";
    }

    
}
