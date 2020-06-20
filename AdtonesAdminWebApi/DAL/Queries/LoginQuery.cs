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
        string UpdatePassword { get; }
    }


    public class LoginQuery : ILoginQuery
    {
        //,LastPasswordChangedDate,LockOutTime,
        public string LoginUser => @"SELECT UserId,RoleId,Email,FirstName,LastName,PasswordHash,Activated,
                                    OperatorId, Organisation, DateCreated, VerificationStatus
                                    FROM Users WHERE LOWER(Email)=@email AND Activated !=3;";


        public string UpdateLockout => @"UPDATE Users SET Activated=@activated, LockOutTime=@lockOutTime WHERE;";

        public string UpdatePassword => @"Update Users SET PasswordHash=@PasswordHash, LastPasswordChangedDate=GETDATE() WHERE Email=@Email";


    }
}
