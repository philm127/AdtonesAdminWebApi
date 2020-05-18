using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.OperatorSpecific
{
    public class ExpressoPromotionalCampaign
    {
        private readonly IConfiguration _configuration;
        public IConnectionStringService _connService { get; }
        private readonly ISaveFiles _saveFile;

        ReturnResult result = new ReturnResult();

        public ExpressoPromotionalCampaign(IConfiguration configuration, IConnectionStringService connService, ISaveFiles saveFile)

        {
            _configuration = configuration;
            _connService = connService;
            _saveFile = saveFile;
        }

        public async Task<string> AddPromotionalCampaign(PromotionalCampaignResult model) {

            

            //Add Promotional Campaign Data on DB Server
            CreateOrUpdatePromotionalCampaignCommand promotionalCampaignCommand =
                Mapper.Map<PromotionalCampaignFormModel, CreateOrUpdatePromotionalCampaignCommand>(model);
            ICommandResult promotionalCampaignResult = _commandBus.Submit(promotionalCampaignCommand);
            if (promotionalCampaignResult.Success)
            {
                int promotionalCampaignId = promotionalCampaignResult.Id;
                string adName = "";
                if (model.AdvertLocation == null || model.AdvertLocation == "")
                {
                    adName = "";
                }
                else
                {

                        var operatorFTPDetails = _operatorFTPDetailsRepository.Get(top => top.OperatorId == model.OperatorId);

                        //Transfer Advert File From Operator Server to Linux Server
                        var returnValue = CopyAdToOpeartorServer(model.AdvertLocation, model.OperatorId, operatorFTPDetails);
                        if (returnValue == "Success")
                        {
                            if (operatorFTPDetails != null) adName = operatorFTPDetails.FtpRoot + "/" + model.AdvertLocation.Split('/')[3];

                            //Get and Update Promotional Campaign Data on DB Server
                            var promotionalCampaignData = db.PromotionalCampaigns.Where(top => top.AdtoneServerPromotionalCampaignId == promotionalCampaignId && top.BatchID == model.BatchID && top.CampaignName.ToLower().Equals(model.CampaignName.ToLower())).FirstOrDefault();
                            if (promotionalCampaignData != null)
                            {
                                promotionalCampaignData.AdvertLocation = adName;
                                db.SaveChanges();
                            }
                        }
                }

                //Add Promotional Advert Data on DB Server
                PromotionalAdvertFormModel promotionalAdvertModel = new PromotionalAdvertFormModel();
                promotionalAdvertModel.ID = model.ID;
                promotionalAdvertModel.CampaignID = promotionalCampaignId;
                promotionalAdvertModel.OperatorID = model.OperatorID;
                promotionalAdvertModel.AdvertName = model.AdvertName;
                promotionalAdvertModel.AdvertLocation = model.AdvertLocation;

                CreateOrUpdatePromotionalAdvertCommand promotionalAdvertCommand =
                    Mapper.Map<PromotionalAdvertFormModel, CreateOrUpdatePromotionalAdvertCommand>(promotionalAdvertModel);
                ICommandResult promotionalAdvertResult = _commandBus.Submit(promotionalAdvertCommand);
                if (promotionalAdvertResult.Success)
                {
                    int promotionalAdvertId = promotionalAdvertResult.Id;
                    //Get and Update Promotional Advert Data on DB Server
                    var promotionalAdvertData = db.PromotionalAdverts.Where(top => top.AdtoneServerPromotionalAdvertId == promotionalAdvertId).FirstOrDefault();
                    if (promotionalAdvertData != null)
                    {
                        promotionalAdvertData.AdvertLocation = adName;
                        db.SaveChanges();
                    }
                }

                await OnCampaignCreated(model.BatchID);
            }

            return "Campaign and Advert added successfully for operator. ";
        }


        
    }
}
