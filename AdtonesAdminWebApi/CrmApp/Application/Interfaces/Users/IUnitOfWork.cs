using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.CrmApp.Application.Interfaces.Users
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        ISubscriberRepository Subscribers { get; }
    }
}
