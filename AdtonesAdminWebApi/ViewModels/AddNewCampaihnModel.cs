//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace AdtonesAdminWebApi.ViewModels
//{
//    public class AddNewCampaihnModel
//    {
//        public string CampaignId { get; set; } 
//        public string CampaignName { get; set; }
//        public string CampaignDescription { get; set; }
//        public string PhoneticAlphabet { get; set; }
//        public string CountryId { get; set; }
//        public bool ClientCheck { get; set; }

//        public NewClientFormModel newClientFormModel { get; set; }
//        public NewBudgetFormModel newBudgetFormModel { get; set; }
//        public NewAdvertFormModel newAdvertFormModel { get; set; }
//        public NewCampaignDateandInteraction newCampaignDateandInteraction { get; set; }
//        public NewAdProfileMappingFormModel newAdProfileMappingFormModel { get; set; }

//        public NewCampaignProfileFormModel()
//        {
//            newClientFormModel = new NewClientFormModel();
//            newBudgetFormModel = new NewBudgetFormModel();
//            newAdvertFormModel = new NewAdvertFormModel();
//            newCampaignDateandInteraction = new NewCampaignDateandInteraction();
//            newAdProfileMappingFormModel = new NewAdProfileMappingFormModel();
//        }
//    }

//    public class NewClientFormModel
//    {
//        public int ClientId { get; set; }

//        public int UserId { get; set; }

//        // [ExcludeChar("/.,!@#$%", ErrorMessage = "Name contains invalid character.")]
//        public string ClientName { get; set; }

//        public string ClientDescription { get; set; }

//        public string ClientContactInfo { get; set; }

//        public decimal ClientBudget { get; set; }

//        public string ClientEmail { get; set; }

//        public string ClientPhoneticAlphabet { get; set; }

//        public DateTime CreatedDate { get; set; }

//        public DateTime UpdatedDate { get; set; }

//        public int ClientStatus { get; set; }

//        public bool NextStatus { get; set; }
//        public string ClientContactPhone { get; set; }

//        public int? CountryId { get; set; }

//        public int? AdtoneServerClientId { get; set; }
//    }


//    public class NewBudgetFormModel
//    {
//        public string MonthlyBudget { get; set; }
//        public string WeeklyBudget { get; set; }

//        public string DailyBudget { get; set; }

//        public string HourlyBudget { get; set; }

//        public string MaximumBid { get; set; }

//        public int CurrencyId { get; set; }
//    }


//    public class NewAdvertFormModel
//    {
//        public int AdvertId { get; set; }

//        public int UserId { get; set; }

//        //[Required(ErrorMessage = "The Client field is required.")]
//        public int? AdvertClientId { get; set; }
//        public string BrandName { get; set; }

//        public string AdvertName { get; set; }

//        public int AdvertCategoryId { get; set; }

//        public string UploadMediaFile { get; set; }

//        public string UploadScriptFile { get; set; }

//        public string Script { get; set; }

//        public string Numberofadsinabatch { get; set; }

//        public string PhoneticAlphabet { get; set; }

//        // [Range(typeof(bool), "true", "true", ErrorMessage = "Please Accept Terms & Condition.")]
//        public bool IsTermChecked { get; set; }


//        public string ScriptFileLocation { get; set; }

//        public string MediaFileLocation { get; set; }

//        public bool UploadedToMediaServer { get; set; }

//        public DateTime CreatedDateTime { get; set; }

//        public DateTime UpdatedDateTime { get; set; }

//        public int Status { get; set; }

//        public bool IsAdminApproval { get; set; }

//        public int CountryId { get; set; }

//        public int SoapToneId { get; set; }

//        public bool NextStatus { get; set; }

//        public int? CampProfileId { get; set; }

//        public int? AdtoneServerAdvertId { get; set; }

//        public int OperatorId { get; set; }
//    }


//    public class NewCampaignDateandInteraction
//    {
//        public DateTime? StartDate { get; set; }

//        public DateTime? EndDate { get; set; }

//        public string SmsOriginator { get; set; }

//        public string SmsBody { get; set; }

//        public string EmailSubject { get; set; }

//        public string EmailBody { get; set; }
//    }


    

//}
