using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IOperatorDAL
    {
        Task<IEnumerable<OperatorResult>> LoadOperatorResultSet();
        Task<bool> CheckOperatorExists(OperatorFormModel model);
        Task<int> AddOperator(OperatorFormModel model);
        Task<OperatorFormModel> GetOperatorById(int id);
        Task<int> UpdateOperator(OperatorFormModel model);
        Task<IEnumerable<OperatorMaxAdvertsFormModel>> LoadOperatorMaxAdvertResultSet();
        Task<bool> CheckMaxAdvertExists(OperatorMaxAdvertsFormModel model);
        Task<int> AddOperatorMaxAdvert(OperatorMaxAdvertsFormModel model);
        Task<OperatorMaxAdvertsFormModel> GetOperatorMaxAdvertById(int id);
        Task<int> UpdateOperatorMaxAdvert(OperatorMaxAdvertsFormModel model);
    }
}
