using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IUserProfileDAL
    {
        Task<string> GetUserProfileMsisdn(int id);
    }
}
