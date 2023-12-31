﻿using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface ICampaignService
    {
        
        Task<ReturnResult> LoadCampaignDataTableById(int id);
        Task<ReturnResult> LoadCampaignDataTable(int id=0);
        Task<ReturnResult> UpdateCampaignStatus(IdCollectionViewModel model);
        Task<bool> ChangeCampaignStatus(int campaignId);
        Task<ReturnResult> LoadCampaignDataTableSalesExec(int id = 0);
        Task<ReturnResult> AddCampaignCategory(CampaignCategoryResult model);
    }
}
