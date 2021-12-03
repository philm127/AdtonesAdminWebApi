using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface IGenerateReportDataByOperator
    {
        Task<ManagementReportModel> GetReportDataByOperator(ManagementReportsSearch search, int op);
    }
}
