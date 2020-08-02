using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ISystemConfigService
    {
        Task<ReturnResult> LoadSystemConfigurationDataTable();
        Task<ReturnResult> GetSystemConfig(int id);
        Task<ReturnResult> AddSystemConfig(SystemConfigResult model);
        Task<ReturnResult> DeleteSystemConfig(int id);
        Task<ReturnResult> UpdateSystemConfig(SystemConfigResult model);
        ReturnResult GetSystemConfigType();
    }
}
