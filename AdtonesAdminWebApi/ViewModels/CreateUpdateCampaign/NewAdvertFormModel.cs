using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign
{
    public class NewAdvertFormModel
    {
        public int AdvertId { get; set; }

        public int UserId { get; set; }

        public int? AdvertClientId { get; set; }

        public string BrandName { get; set; }

        public string AdvertName { get; set; }

        public int AdvertCategoryId { get; set; }

        public string UploadMediaFile { get; set; }

        public string UploadScriptFile { get; set; }

        public string Script { get; set; }

        public string Numberofadsinabatch { get; set; }

        public string PhoneticAlphabet { get; set; }

        public bool IsTermChecked { get; set; }


        public string ScriptFileLocation { get; set; }

        public string MediaFileLocation { get; set; }

        public bool UploadedToMediaServer { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        public int Status { get; set; }

        public bool IsAdminApproval { get; set; }

        public int CountryId { get; set; }

        public int SoapToneId { get; set; }

        public bool NextStatus { get; set; }

        public int? CampProfileId { get; set; }

        public int? AdtoneServerAdvertId { get; set; }

        public int OperatorId { get; set; }
    }
}