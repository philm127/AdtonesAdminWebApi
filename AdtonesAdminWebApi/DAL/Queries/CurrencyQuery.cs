using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class CurrencyQuery
    {


        public static string GetCurrencyByUserIdContact => @"SELECT con.CurrencyId, cur.CurrencyCode,con.CountryId FROM Contacts AS con 
                                                            INNER JOIN Currencies AS cur ON cur.CountryId=con.CountryId
                                                            WHERE con.UserId = @userId ";
    }
}
