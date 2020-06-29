using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IAdvertiserCreditService
    {
        Task<ReturnResult> AddCredit(AdvertiserCreditFormModel _creditmodel);
        Task<ReturnResult> GetCreditDetails(int id);
        // Task<ReturnResult> UpdateCredit(UsersCreditFormModel _creditmodel);
        Task<ReturnResult> LoadDataTable();
    }
}
