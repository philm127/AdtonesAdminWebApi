﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories.Queries
{
    public static class SalesManagementQuery
    {
        public static string GetUnallocated => @"SELECT u.UserId,CONCAT(u.FirstName,' ',u.LastName) AS FullName FROM Users AS u
                                                   INNER JOIN Contacts AS con ON con.UserId=u.UserId
                                                   WHERE RoleId=3 AND VerificationStatus=1 AND u.UserId NOT IN 
                                                    (SELECT AdvertiserId FROM Advertisers_SalesTeam WHERE IsActive=1) ";


        public static string GetAllocatedBySalesExec => @"SELECT u.UserId,CONCAT(u.FirstName,' ',u.LastName) AS FullName FROM Users AS u 
                                                            INNER JOIN Contacts AS con ON con.UserId=u.UserId
                                                            WHERE RoleId=3 AND u.UserId IN 
                                                                (SELECT AdvertiserId FROM Advertisers_SalesTeam 
                                                                  WHERE IsActive=1 AND SalesExecId=@userId) ";

        public static string GetSalesExecDDList => @"SELECT u.UserId AS Value,CONCAT(u.FirstName,' ',u.LastName) AS Text FROM Users AS u
                                                    INNER JOIN SalesManager_SalesExec AS s ON u.UserId=s.ExecId WHERE Active=1 AND ManId=@Id";


        public static string CheckAdvertiserExists => @"SELECT COUNT(1) FROM Advertisers_SalesTeam WHERE AdvertiserId = @Id";


        public static string UpdateSalesToAdvertiserToInActive => @"UPDATE Advertisers_SalesTeam SET IsActive=0,UpdatedDate=GETDATE() 
                                                                    WHERE AdvertiserId=@AdId AND SalesExecId=@Sid";


        public static string InsertNewAdToSales => @"INSERT INTO Advertisers_SalesTeam(AdvertiserId,SalesExecId,SalesManId,MailSupressed,IsActive,CreatedDate,UpdatedDate)
                                                        VALUES(@AdId,@Sid, @ManId, @Suppress,1,GETDATE(), GETDATE())";


        public static string UpdateAdToSales => @"UPDATE Advertisers_SalesTeam SET SalesExecId=@Sid, IsActive=1,UpdatedDate=GETDATE() 
                                                    WHERE AdvertiserId=@AdId";

        public static string GetSalesManagerId => @"SELECT ManId FROM SalesManager_SalesExec WHERE ExecId=@Id";


        public static string GetSalesExecInvDets => @"SELECT u.Email FROM Users AS u INNER JOIN Advertisers_SalesTeam AS adsal 
                                                        ON adsal.SalesExecId=u.UserId WHERE MailSupressed=1 AND IsActive=1
                                                        AND AdvertiserId=@Id";
    }
}
