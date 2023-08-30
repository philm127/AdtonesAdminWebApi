using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class UserDashboardQuery
    {
        public static string AdvertiserResultQuery => @"WITH MaxCompanyDetail AS
                                                        (
                                                            SELECT UserId, MAX(Id) as MaxId
                                                            FROM CompanyDetails
                                                            GROUP BY UserId
                                                        )

                                                        SELECT u.UserId,u.RoleId,u.Email,u.FirstName,u.LastName,
                                                            ISNULL(camp.NoOfactivecampaign, 0) AS NoOfactivecampaign,
	                                                        ISNULL(co.Name, 'N/A') AS CountryName,
                                                            ISNULL(ad.NoOfunapprovedadverts, 0) AS NoOfunapprovedadverts,
                                                            ISNULL(cred.AssignCredit, 0) AS creditlimit,
	                                                        ISNULL(billit.outStandingInvoice, 0) AS outStandingInvoice,
                                                            u.Activated,u.DateCreated,
	                                                        ISNULL(tkt.TicketCount, 0) AS TicketCount,
                                                            CONCAT(cont.MobileNumber,' ',cont.FixedLine, ' ',cont.PhoneNumber) AS MobileNumber,com.CompanyName
                                                            FROM Users AS u
	                                                        INNER JOIN
                                                                (SELECT con.UserId,con.CountryId,
                                                                (Case WHEN LEN(con.MobileNumber)>7 THEN CONCAT('M:',REPLACE(con.MobileNumber,' ','')) ELSE NULL END) AS MobileNumber,
                                                                (Case WHEN LEN(con.FixedLine)>7 THEN CONCAT('F:',REPLACE(con.FixedLine,' ','')) ELSE NULL END) AS FixedLine,
                                                                (Case WHEN LEN(con.PhoneNumber)>7 THEN CONCAT('P:',REPLACE(con.PhoneNumber,' ','')) ELSE NULL END) AS PhoneNumber
                                                                FROM Contacts AS con) cont
		                                                        ON u.UserId=cont.UserId
	                                                        LEFT JOIN (
                                                                    SELECT cd.UserId, cd.CompanyName
                                                                    FROM CompanyDetails cd
                                                                    JOIN MaxCompanyDetail mcd ON cd.UserId = mcd.UserId AND cd.Id = mcd.MaxId
                                                                ) com ON u.UserId = com.UserId
                                                            LEFT JOIN
                                                                (SELECT a.[UserId], b.[AssignCredit], a.[Id] 
			                                                        FROM
				                                                        (SELECT[UserId], MIN(Id) AS Id FROM UsersCredit GROUP BY[UserId]) a
					                                                        INNER JOIN UsersCredit b ON a.[UserId] = b.[UserId] AND a.Id = b.Id) cred
		                                                        ON u.UserId = cred.UserId
                                                            LEFT JOIN
                                                                (SELECT COUNT(UserId)as TicketCount, UserId FROM Question WHERE Status IN (1, 2) GROUP BY UserId) tkt
		                                                        ON u.UserId = tkt.UserId
                                                            LEFT JOIN
                                                                (SELECT COUNT(CampaignProfileId) AS NoOfactivecampaign,UserId FROM Campaignprofile 
                                                                    WHERE Status IN (4, 3, 2, 1) GROUP BY UserId) camp
		                                                        ON u.UserId = camp.UserId
                                                            LEFT JOIN
                                                                (SELECT COUNT(AdvertId) AS NoOfunapprovedadverts,UserId FROM Advert 
                                                                    WHERE Status = 4 GROUP BY UserId) ad
		                                                        ON u.UserId = ad.UserId
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
                                                            ON u.UserId = billit.UserId
                                                            LEFT JOIN Country AS co ON cont.CountryId=co.Id
                                                            LEFT JOIN Operators AS op ON op.CountryId=co.Id
	                                                        WHERE u.RoleId=3 AND u.VerificationStatus = 1 ";


        // Operators version of Advertisers table. INNER JOINS on Advert for only their adverts, adds mobileNumber and does not select Role.
        public static string OperatorAdvertiserResultQuery => @"SELECT item.UserId,item.RoleId,item.Email,item.FirstName,item.LastName,
                                                          ISNULL(camp.NoOfactivecampaign, 0) AS NoOfactivecampaign,cont.MobileNumber,
                                                           ISNULL(ad.NoOfunapprovedadverts, 0) AS NoOfunapprovedadverts,
                                                           ISNULL(cred.AssignCredit, 0) AS creditlimit,ISNULL(billit.outStandingInvoice, 0) AS outStandingInvoice,
                                                           item.Activated,item.DateCreated,ISNULL(tkt.TicketCount, 0) AS TicketCount
                                                           FROM
                                                                (SELECT item.UserId, item.RoleId, item.Email, item.DateCreated, item.Activated,
                                                                item.FirstName,item.LastName
                                                                FROM Users item Where item.VerificationStatus = 1 AND item.UserId IN
                                                                                (SELECT DISTINCT(UserId) AS UserId FROM Advert WHERE OperatorId=@operatorId)) item
                                                            LEFT JOIN
                                                                (SELECT a.[UserId], b.[AssignCredit], a.[Id] 
                                                                    FROM
                                                                        (SELECT[UserId], MIN(Id) AS Id FROM UsersCredit GROUP BY[UserId]) a
                                                            LEFT JOIN UsersCredit b ON a.[UserId] = b.[UserId] AND a.Id = b.Id) cred
                                                            ON item.UserId = cred.UserId
                                                            LEFT JOIN
                                                                (SELECT COUNT(UserId) AS TicketCount, UserId FROM Question WHERE Status IN (1, 2) GROUP BY UserId) tkt
                                                            ON item.UserId = tkt.UserId
                                                            LEFT JOIN
                                                                (SELECT COUNT(CampaignProfileId) AS NoOfactivecampaign,UserId FROM Campaignprofile 
                                                                    WHERE Status IN (4, 3, 2, 1) GROUP BY UserId) camp
                                                            ON item.UserId = camp.UserId
                                                            LEFT JOIN
                                                                (SELECT COUNT(AdvertId) AS NoOfunapprovedadverts,UserId,OperatorId FROM Advert WHERE Status = 4 GROUP BY UserId,OperatorId) ad
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
                                                            ON item.UserId = billit.UserId
                                                            LEFT JOIN Contacts AS cont ON cont.UserId=item.UserId;";

        
        public static string SalesManagerAdvertiserResultQuery => @"WITH MaxCompanyDetail AS
                                                                    (
                                                                        SELECT UserId, MAX(Id) as MaxId
                                                                        FROM CompanyDetails
                                                                        GROUP BY UserId
                                                                    )

                                                                    SELECT u.UserId,u.RoleId,u.Email,u.FirstName,u.LastName,
                                                                        ISNULL(sales.SalesExec, 'UnAllocated') AS SalesExec, sales.UserId AS SUserId,
                                                                        ISNULL(camp.NoOfactivecampaign, 0) AS NoOfactivecampaign,
	                                                                    ISNULL(co.Name, 'N/A') AS CountryName,
                                                                        ISNULL(ad.NoOfunapprovedadverts, 0) AS NoOfunapprovedadverts,
                                                                        ISNULL(cred.AssignCredit, 0) AS creditlimit,
	                                                                    ISNULL(billit.outStandingInvoice, 0) AS outStandingInvoice,
                                                                        u.Activated,u.DateCreated,
	                                                                    ISNULL(tkt.TicketCount, 0) AS TicketCount,
                                                                        CONCAT(cont.MobileNumber,' ',cont.FixedLine, ' ',cont.PhoneNumber) AS MobileNumber,com.CompanyName
                                                                        FROM Users AS u
	                                                                    INNER JOIN
                                                                            (SELECT con.UserId,con.CountryId,
                                                                            (Case WHEN LEN(con.MobileNumber)>7 THEN CONCAT('M:',REPLACE(con.MobileNumber,' ','')) ELSE NULL END) AS MobileNumber,
                                                                            (Case WHEN LEN(con.FixedLine)>7 THEN CONCAT('F:',REPLACE(con.FixedLine,' ','')) ELSE NULL END) AS FixedLine,
                                                                            (Case WHEN LEN(con.PhoneNumber)>7 THEN CONCAT('P:',REPLACE(con.PhoneNumber,' ','')) ELSE NULL END) AS PhoneNumber
                                                                            FROM Contacts AS con) cont
		                                                                    ON u.UserId=cont.UserId
                                                                        LEFT JOIN
                                                                            (SELECT CONCAT(u.FirstName,' ',u.LastName) AS SalesExec,u.UserId,sales.AdvertiserId 
                                                                            FROM Users AS u INNER JOIN Advertisers_SalesTeam AS sales ON sales.SalesExecId=u.UserId
                                                                            WHERE IsActive=1) AS sales
                                                                            ON sales.AdvertiserId=u.UserId
	                                                                    LEFT JOIN (
                                                                                SELECT cd.UserId, cd.CompanyName
                                                                                FROM CompanyDetails cd
                                                                                JOIN MaxCompanyDetail mcd ON cd.UserId = mcd.UserId AND cd.Id = mcd.MaxId
                                                                            ) com ON u.UserId = com.UserId
                                                                                                                        LEFT JOIN
                                                                            (SELECT a.[UserId], b.[AssignCredit], a.[Id] 
			                                                                    FROM
				                                                                    (SELECT[UserId], MIN(Id) AS Id FROM UsersCredit GROUP BY[UserId]) a
					                                                                    INNER JOIN UsersCredit b ON a.[UserId] = b.[UserId] AND a.Id = b.Id) cred
		                                                                    ON u.UserId = cred.UserId
                                                                        LEFT JOIN
                                                                            (SELECT COUNT(UserId)as TicketCount, UserId FROM Question WHERE Status IN (1, 2) GROUP BY UserId) tkt
		                                                                    ON u.UserId = tkt.UserId
                                                                        LEFT JOIN
                                                                            (SELECT COUNT(CampaignProfileId) AS NoOfactivecampaign,UserId FROM Campaignprofile 
                                                                                WHERE Status IN (4, 3, 2, 1) GROUP BY UserId) camp
		                                                                    ON u.UserId = camp.UserId
                                                                        LEFT JOIN
                                                                            (SELECT COUNT(AdvertId) AS NoOfunapprovedadverts,UserId FROM Advert 
                                                                                WHERE Status = 4 GROUP BY UserId) ad
		                                                                    ON u.UserId = ad.UserId
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
                                                                        ON u.UserId = billit.UserId
                                                                        LEFT JOIN Country AS co ON cont.CountryId=co.Id
                                                                        LEFT JOIN Operators AS op ON op.CountryId=co.Id
	                                                                    WHERE u.RoleId=3 AND u.VerificationStatus = 1  ";



        public static string OperatorResultQuery => @"SELECT u.UserId,FirstName,LastName,Email,ISNULL(Organisation,'-') AS Organisation,u.OperatorId,o.CountryId,
                                                c.Name AS CountryName,o.OperatorName,u.Activated,u.DateCreated
                                                FROM Users AS u LEFT JOIN Operators AS o ON u.OperatorId=o.OperatorId
                                                LEFT JOIN Country AS c ON o.CountryId=c.Id
                                                WHERE RoleId=6 ";



        public static string AdminResultQuery => @"SELECT u.UserId,u.FirstName,u.LastName,u.Email,ISNULL(u.Organisation,'-') AS Organisation,o.CountryId,
                                                c.Name AS CountryName,u.Activated,u.DateCreated, RoleId,MobileNumber
                                                FROM Users AS u LEFT JOIN Contacts AS o ON u.UserId=o.UserId
                                                LEFT JOIN Country AS c ON o.CountryId=c.Id
                                                WHERE RoleId IN(1,4,5,7,8) ORDER BY u.DateCreated DESC";




        public static string SubscriberResultQuery => @"SELECT u.UserId,u.Activated,u.DateCreated,FirstName,LastName,p.MSISDN,op.OperatorName,u.Email,u.Activated,
                                                  co.Name AS CountryName
                                                  FROM Users AS u LEFT JOIN UserProfile AS p ON p.UserId=u.UserId
                                                  INNER JOIN Operators AS op ON op.OperatorId=u.OperatorId
                                                  INNER JOIN Country AS co ON co.Id=op.CountryId
                                                  WHERE u.RoleId=2 AND u.VerificationStatus=1 ";



        public static string SalesExecResultQuery => @"SELECT ss.ExecId AS UserId,u.Email, u.DateCreated, u.Activated,
                                                        u.FirstName,u.LastName,COUNT(x.UserId) As NoAdvertisers,SUM(x.NoOfactivecampaign) AS NoOfactivecampaign,
                                                        SUM(x.NoOfunapprovedadverts) AS NoOfunapprovedadverts,
                                                        SUM(x.outStandingInvoice) AS outStandingInvoice, SUM(x.TicketCount) AS TicketCount
                                                        FROM SalesManager_SalesExec AS ss 
                                                        LEFT JOIN Advertisers_SalesTeam AS adsales ON adsales.SalesExecId=ss.ExecId
                                                        INNER JOIN Users AS u ON u.UserId=ss.ExecId
                                                        LEFT JOIN
                                                        (
                                                        SELECT item.UserId,
                                                        ISNULL(camp.NoOfactivecampaign, 0) AS NoOfactivecampaign,
                                                        ISNULL(ad.NoOfunapprovedadverts, 0) AS NoOfunapprovedadverts,
                                                        ISNULL(billit.outStandingInvoice, 0) AS outStandingInvoice,
                                                        ISNULL(tkt.TicketCount, 0) AS TicketCount
                                                        FROM
                                                            (SELECT item.UserId
                                                            FROM Users item Where item.VerificationStatus = 1 AND item.RoleId = 3) item
                                                        LEFT JOIN
                                                            (SELECT COUNT(UserId)as TicketCount, UserId FROM Question WHERE Status IN (1, 2) 
                                                                GROUP BY UserId) tkt
                                                        ON item.UserId = tkt.UserId
                                                        LEFT JOIN
                                                            (SELECT COUNT(CampaignProfileId) AS NoOfactivecampaign,UserId FROM Campaignprofile 
                                                                WHERE Status IN (4, 3, 2, 1) GROUP BY UserId) camp
                                                        ON item.UserId = camp.UserId
                                                        LEFT JOIN
                                                            (SELECT COUNT(AdvertId) AS NoOfunapprovedadverts,UserId FROM Advert 
                                                                WHERE Status = 4 GROUP BY UserId) ad
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
                                                        ON item.UserId = billit.UserId
                                                        ) AS x
                                                        ON adsales.AdvertiserId = x.UserId ";


        public static string SalesExecForAdminResultQuery => @"SELECT u.UserId,u.Email, u.DateCreated, u.Activated,co.Name AS CountryName,
                                                                u.FirstName,u.LastName,CONCAT(um.FirstName,' ',um.LastName) AS SalesManager,
                                                                COUNT(x.UserId) As NoAdvertisers,SUM(x.NoOfactivecampaign) AS NoOfactivecampaign,
                                                                SUM(x.NoOfunapprovedadverts) AS NoOfunapprovedadverts,
                                                                SUM(x.outStandingInvoice) AS outStandingInvoice
                                                                FROM Users AS u 
                                                                LEFT JOIN Advertisers_SalesTeam AS adsales ON adsales.SalesExecId=u.UserId
                                                                LEFT JOIN SalesManager_SalesExec AS ss ON u.UserId=ss.ExecId
                                                                LEFT JOIN Users AS um ON um.UserId=ss.ManId
                                                                LEFT JOIN Contacts AS con ON con.UserId=u.UserId
                                                                LEFT JOIN Country AS co ON co.Id=con.CountryId
                                                                LEFT JOIN
                                                                (
                                                                SELECT item.UserId,
                                                                ISNULL(camp.NoOfactivecampaign, 0) AS NoOfactivecampaign,
                                                                ISNULL(ad.NoOfunapprovedadverts, 0) AS NoOfunapprovedadverts,
                                                                ISNULL(billit.outStandingInvoice, 0) AS outStandingInvoice
                                                                FROM
                                                                    (SELECT item.UserId
                                                                    FROM Users item Where item.VerificationStatus = 1 AND item.RoleId = 3) item
                                                                LEFT JOIN
                                                                    (SELECT COUNT(CampaignProfileId) AS NoOfactivecampaign,UserId FROM Campaignprofile 
                                                                        WHERE Status IN (4, 3, 2, 1) GROUP BY UserId) camp
                                                                ON item.UserId = camp.UserId
                                                                LEFT JOIN
                                                                    (SELECT COUNT(AdvertId) AS NoOfunapprovedadverts,UserId FROM Advert 
                                                                        WHERE Status = 4 GROUP BY UserId) ad
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
                                                                ON item.UserId = billit.UserId
                                                                ) AS x
                                                                ON adsales.AdvertiserId = x.UserId
                                                                WHERE u.RoleId=9 
                                                                GROUP BY u.UserId,adsales.SalesExecId,u.Email, u.DateCreated, u.Activated,
                                                                u.FirstName, u.LastName,um.FirstName, um.LastName,co.Name";



        public static string AdvertiserForSalesTeamResultQuery => @"SELECT item.UserId,item.RoleId,item.Email,item.FirstName,item.LastName,
                                                  ISNULL(camp.NoOfactivecampaign, 0) AS NoOfactivecampaign,ISNULL(co.Name, 'N/A') AS CountryName,
                                                   ISNULL(ad.NoOfunapprovedadverts, 0) AS NoOfunapprovedadverts,
                                                   ISNULL(cred.AssignCredit, 0) AS creditlimit,ISNULL(billit.outStandingInvoice, 0) AS outStandingInvoice,
                                                   item.Activated,item.DateCreated,ISNULL(tkt.TicketCount, 0) AS TicketCount,
                                                    CONCAT(item.MobileNumber,' ',item.FixedLine, ' ',item.PhoneNumber) AS MobileNumber,item.CompanyName
                                                   FROM
                                                        (SELECT item.UserId, item.RoleId, item.Email, item.DateCreated, item.Activated,
                                                        item.FirstName,item.LastName,
                                                        (Case WHEN LEN(con.MobileNumber)>7 THEN CONCAT('M:',REPLACE(con.MobileNumber,' ','')) ELSE NULL END) AS MobileNumber,
                                                        (Case WHEN LEN(con.FixedLine)>7 THEN CONCAT('F:',REPLACE(con.FixedLine,' ','')) ELSE NULL END) AS FixedLine,
                                                        (Case WHEN LEN(con.PhoneNumber)>7 THEN CONCAT('P:',REPLACE(con.PhoneNumber,' ','')) ELSE NULL END) AS PhoneNumber,
                                                        com.CompanyName
                                                        FROM Users AS item LEFT JOIN Contacts AS con ON con.UserId=item.UserId
                                                        LEFT JOIN CompanyDetails AS com ON com.UserId=item.UserId 
                                                        Where item.VerificationStatus = 1 AND item.RoleId = 3) item
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
                                                        (SELECT COUNT(AdvertId) AS NoOfunapprovedadverts,UserId FROM Advert 
                                                            WHERE Status = 4 GROUP BY UserId) ad
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
                                                    ON item.UserId = billit.UserId
                                                    LEFT JOIN Contacts AS cont ON cont.UserId=item.UserId
                                                    LEFT JOIN Country AS co ON cont.CountryId=co.Id
                                                    LEFT JOIN Operators AS op ON op.CountryId=co.Id
                                                    INNER JOIN Advertisers_SalesTeam AS adsales ON item.UserId=adsales.AdvertiserId AND adsales.IsActive=1
                                                    WHERE adsales.SalesExecId=@Sid";
    }

    
}
