using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IUsersCreditService
    {
        Task<ReturnResult> AddCredit(UserCreditFormModel _creditmodel);
        Task<ReturnResult> GetAddCreditUsersList();
        
        
        Task<ReturnResult> GetCreditDetails(IdCollectionViewModel model);
        Task<ReturnResult> UpdateCredit(UsersCreditFormModel _creditmodel);
        Task<ReturnResult> LoadDataTable();

        // Task<ReturnResult> GetCreditDetailsUsersList();
        // Task<ReturnResult> UserCreditpayment(IdCollectionViewModel model);


    }
}
