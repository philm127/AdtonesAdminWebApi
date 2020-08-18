using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IConnectionStringService
    {
        Task<string> GetSingleConnectionString(int Id);
        Task<IEnumerable<string>> GetConnectionStrings(int Id = 0);
        Task<int> GetUserIdFromAdtoneId(int Id, int operatorId);
        Task<int> GetCampaignProfileIdFromAdtoneId(int Id, int operatorId);
        Task<int> GetAdvertIdFromAdtoneId(int Id, int operatorId);
        Task<string> GetOperatorConnectionByUserId(int id);
        Task<IEnumerable<string>> GetConnectionStringsByCountry(int Id);
        Task<int> GetOperatorIdFromAdtoneId(int Id, int operatorId);
    }
}
