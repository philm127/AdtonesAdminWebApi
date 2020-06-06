using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    
    public interface IUserProfileQuery
    {
        string GetMsisdnNumber { get; }
    }



    public class UserProfileQuery : IUserProfileQuery
    {
        public string GetMsisdnNumber => "SELECT MSISDN FROM UserProfile WHERE UserId=@Id;";
    }
}
