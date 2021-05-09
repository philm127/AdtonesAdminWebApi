using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.CrmApp.Core.Enums
{
    public enum UserStatus
    {
        Pending = 0,
        Approved = 1,
        Suspended = 2,
        Deleted = 3,
        Blocked = 4
    }
}
