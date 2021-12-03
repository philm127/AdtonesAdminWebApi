using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface IOperatorConfigService
    {
        Task<ReturnResult> LoadOperatorConfigurationDataTable();
        Task<ReturnResult> GetOperatorConfig(int id);
        Task<ReturnResult> AddOperatorConfig(OperatorConfigurationResult model);
        Task<ReturnResult> UpdateOperatorConfig(OperatorConfigurationResult model);
    }
}
