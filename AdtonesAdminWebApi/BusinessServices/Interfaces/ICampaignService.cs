﻿using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ICampaignService
    {
        Task<ReturnResult> LoadCampaignCreditDataTable(int id=0);
        Task<ReturnResult> AddCampaignCredit(CampaignCreditResult model);
        Task<ReturnResult> UpdateCampaignCredit(CampaignCreditResult model);
        Task<ReturnResult> LoadCampaignDataTableById(int id);
        Task<ReturnResult> LoadCampaignDataTable(int id=0);
        Task<ReturnResult> UpdateCampaignStatus(IdCollectionViewModel model);
        Task<bool> ChangeCampaignStatus(int campaignId);
    }
}
