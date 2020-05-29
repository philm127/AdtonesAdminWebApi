using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{

    public interface ILoginQuery
    {
        string LoginUser { get; }
        string UpdateLockout { get; }
    }


    public class LoginQuery : ILoginQuery
    {
        public string LoginUser => @"SELECT UserId,RoleId,Email,FirstName,LastName,PasswordHash,Activated,LockOutTime,
                                    OperatorId, Organisation, DateCreated, VerificationStatus,LastPasswordChangedDate
                                    FROM Users WHERE LOWER(Email)=@email AND Activated !=3;";


        public string UpdateLockout => @"UPDATE Users SET Activated=@activated, LockOutTime=@lockOutTime WHERE;";


    }
}
