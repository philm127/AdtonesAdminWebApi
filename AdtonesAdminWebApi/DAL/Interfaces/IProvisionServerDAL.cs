using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IProvisionServerDAL
    {
        Task<IEnumerable<string>> GetMsisdnCheckForExisting(string command, string conString);
        Task<bool> GetPromoUserBatchIdCheckForExisting(string command, string conString);
    }
}
