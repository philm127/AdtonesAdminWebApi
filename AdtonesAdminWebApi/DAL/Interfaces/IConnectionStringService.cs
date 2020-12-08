using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IConnectionStringService
    {
        Task<string> GetConnectionStringByOperator(int Id);
        Task<IEnumerable<string>> GetConnectionStrings();
        Task<int> GetUserIdFromAdtoneId(int Id, int operatorId);
        Task<int> GetUserIdFromAdtoneIdByConnString(int Id, string conn);
        Task<int> GetCampaignProfileIdFromAdtoneId(int Id, int operatorId);
        Task<int> GetCampaignProfileIdFromAdtoneIdByConnString(int Id, string conn);
        Task<int> GetAdvertIdFromAdtoneId(int Id, int operatorId);
        Task<string> GetOperatorConnectionByUserId(int id);
        Task<IEnumerable<string>> GetConnectionStringsByCountry(int Id);
        Task<string> GetConnectionStringsByCountryId(int Id);
        Task<int> GetOperatorIdFromAdtoneId(int operatorId);
        Task<int> GetCountryIdFromAdtoneId(int Id, string conn);
    }
}
