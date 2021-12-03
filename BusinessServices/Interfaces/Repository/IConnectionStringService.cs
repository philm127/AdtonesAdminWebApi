using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository
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
        Task<List<string>> GetConnectionStringsByCountry(int Id);
        Task<string> GetConnectionStringsByCountryId(int Id);

        Task<List<string>> GetConnectionStringsByUserId(int id);
        Task<int> GetOperatorIdFromAdtoneId(int operatorId);
        Task<int> GetCountryIdFromAdtoneId(int Id, string conn);
        Task<int> GetClientIdFromAdtoneIdByConnString(int Id, string conn);
        Task<int> GetCampaignProfilePreferenceIdFromAdtoneId(int Id, string conn);
    }
}
