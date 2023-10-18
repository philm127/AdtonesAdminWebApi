using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IConnectionStringService
    {
        Task<string> GetConnectionStringByOperator(int Id);
        Task<List<string>> GetConnectionStrings();
        Task<int> GetUserIdFromAdtoneId(int Id, int operatorId);
        Task<int> GetUserIdFromAdtoneIdByConnString(int Id, string conn);
        Task<int> GetCampaignProfileIdFromAdtoneId(int Id, int operatorId);
        Task<int> GetCampaignProfileIdFromAdtoneIdByConn(int Id, string conn);
        Task<int> GetCampaignProfileIdFromAdtoneIdByConnString(int Id, string conn);
        Task<int> GetAdvertIdFromAdtoneId(int Id, int operatorId);
        Task<List<string>> GetConnectionStringsByCountryId(int Id);

        Task<List<string>> GetConnectionStringsByUserId(int id);
        Task<int> GetOperatorIdFromAdtoneId(int operatorId);
        Task<int> GetCountryIdFromAdtoneId(int Id, string conn);
        Task<int> GetClientIdFromAdtoneIdByConnString(int Id, string conn);
        Task<int> GetCampaignProfilePreferenceIdFromAdtoneId(int Id, string conn);
    }
}
