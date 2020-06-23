﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface IUserCreditQuery
    {
        string LoadUserCreditDataTable { get; }
        string UpdateUserCredit { get; }
        string AddUserCredit { get; }
    }


    public class UserCreditQuery : IUserCreditQuery
    {
        public string LoadUserCreditDataTable => @"SELECT u.Id,u.UserId,usrs.Email,usrs.FullName,usrs.Organisation,u.CreatedDate,
                                                 u.AssignCredit AS Credit,u.AvailableCredit,ISNULL(bil.FundAmount,0) AS TotalUsed,ISNULL(pay.Amount,0) AS TotalPaid,
                                                 (ISNULL(bil.FundAmount,0) - ISNULL(pay.Amount,0)) AS RemainingAmount,ctry.Name AS CountryName
                                                 FROM UsersCredit AS u
                                                 LEFT JOIN 
                                                 (SELECT UserId,SUM(FundAmount) AS FundAmount FROM Billing WHERE PaymentMethodId=1 GROUP BY UserId) bil
                                                 ON u.UserId=bil.UserId
                                                 LEFT JOIN
                                                 (SELECT UserId,SUM(Amount) AS Amount from UsersCreditPayment GROUP BY UserId) pay
                                                 ON u.UserId=pay.UserId
                                                 LEFT JOIN
                                                 (SELECT UserId,Email,CONCAT(FirstName,' ',LastName) AS FullName,Organisation FROM Users) usrs
                                                 ON usrs.UserId=u.UserId
                                                LEFT JOIN Currencies AS cur ON u.CurrencyId=cur.CurrencyId
                                                LEFT JOIN Country AS ctry ON ctry.Id=cur.CountryId
                                                 ORDER BY u.Id DESC;";


        public string UpdateUserCredit => @"UPDATE UsersCredit SET UserId=@UserId,AssignCredit=@AssignCredit,AvailableCredit=@AssignCredit,
                                                UpdatedDate=GETDATE(),CurrencyId=@CurrencyId
                                                WHERE Id = @Id";


        public string AddUserCredit => @"INSERT INTO UsersCredit(UserId,AssignCredit,AvailableCredit,UpdatedDate,CreatedDate,CurrencyId) 
                                            VALUES(@UserId,@AssignCredit,@AssignCredit,GETDATE(),GETDATE(),@CurrencyId)";

    }

}
