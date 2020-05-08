﻿using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IOperatorService
    {
        Task<ReturnResult> LoadOperatorDataTable();
        Task<ReturnResult> AddOperator(OperatorFormModel operatormodel);
        Task<ReturnResult> GetOperator(IdCollectionViewModel model);
        Task<ReturnResult> UpdateOperator(OperatorFormModel operatormodel);
        Task<ReturnResult> LoadOperatorMaxAdvertDataTable();
        Task<ReturnResult> AddOperatorMaxAdverts(OperatorMaxAdvertsFormModel operatorMaxAdvertsFormModel);
        Task<ReturnResult> GetOperatorMaxAdvert(IdCollectionViewModel model);
        Task<ReturnResult> UpdateOperatorMaxAdverts(OperatorMaxAdvertsFormModel operatorMaxAdvertsFormModel);



    }
}
