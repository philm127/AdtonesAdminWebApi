using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    
    public static class UserProfileQuery
    {
        public static string GetMsisdnNumber => "SELECT MSISDN FROM UserProfile WHERE UserId=@Id;";
    }
}
