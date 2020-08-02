using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    
    public static class AdvertiserCreditQuery
    {

        public static string CheckIfUserExists => @"SELECT COUNT(1) FROM UsersCredit WHERE UserId=@userId";

        public static string UpdateUserCredit => @"UPDATE UsersCredit SET UserId=@UserId,AssignCredit=@AssignCredit,AvailableCredit=@AvailableCredit,
                                                UpdatedDate=GETDATE(),CurrencyId=@CurrencyId
                                                WHERE Id = @Id";


        public static string AddUserCredit => @"INSERT INTO UsersCredit(UserId,AssignCredit,AvailableCredit,UpdatedDate,CreatedDate,CurrencyId) 
                                            VALUES(@UserId,@AssignCredit,@AssignCredit,GETDATE(),GETDATE(),@CurrencyId)";


        public static string UserCreditDetails => @"SELECT uc.Id,uc.UserId,AssignCredit,AvailableCredit,uc.CreatedDate,uc.CurrencyId,c.CountryId 
                                                FROM  UsersCredit AS uc LEFT JOIN Currencies AS c ON c.CurrencyId=uc.CurrencyId
                                                WHERE uc.UserId=@Id";


        public static string GetTotalPaymentsByUser => @"SELECT sum(Amount) FROM UsersCreditPayment WHERE UserId=@UserId GROUP BY UserId";


        public static string GetTotalBilledByUser => @"SELECT sum(FundAmount) FROM Billing WHERE PaymentMethodId=1 AND UserId=@UserId GROUP BY UserId";

    }

}
