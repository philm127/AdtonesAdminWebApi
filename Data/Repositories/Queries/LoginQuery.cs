using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories.Queries
{

    public static class LoginQuery
    {
        public static string LoginUser => @"SELECT UserId,RoleId,Email,FirstName,LastName,PasswordHash,Activated,OperatorId, 
                                    Organisation, DateCreated, VerificationStatus,LastPasswordChangedDate,LockOutTime,Permissions
                                    FROM Users WHERE LOWER(Email)=@email AND Activated !=3;";


        public static string UpdateLockout => @"UPDATE Users SET Activated=@activated, LockOutTime=@lockOutTime WHERE;";

        public static string UpdatePassword => @"UPDATE Users SET PasswordHash=@PasswordHash, LastPasswordChangedDate=GETDATE() WHERE Email=@Email";

        public static string UpdateLoggedIn => @"UPDATE Users SET LastLoginTime=GETDATE() WHERE UserId=@Id";


    }
}
