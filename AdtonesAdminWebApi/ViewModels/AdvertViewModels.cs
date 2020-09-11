using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class AdvertCategoryResult
    {
        public int AdvertCategoryId { get; set; }
        public string CategoryName { get; set; }

        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        public string CreatedDate { get; set; }

        public int? AdtoneServerAdvertCategoryId { get; set; }
    }

    public class UserAdvertResult
    {
        public int AdvertId { get; set; }
        public string AdvertName { get; set; }
        public string Brand { get; set; }

        // This holds whole string including server
        public string MediaFileLocation { get; set; }

        // This is the file path stored in DB
        public string MediaFile { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public string Script { get; set; }

        public string ScriptFileLocation { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string SmsBody { get; set; }
        public string EmailBody { get; set; }
        public int Status { get; set; }

        public int PrevStatus { get; set; }
        public int CampaignProfileId { get; set; } = 0;
        public string rStatus => $"{(Enums.AdvertStatus)Status}";
        public string RejectionReason { get; set; }
        public int UpdatedBy { get; set; }
        public int OperatorId { get; set; }
        public bool UploadedToMediaServer { get; set; }
        public string SoapToneId { get; set; }
        public string SoapToneCode { get; set; }
    }

    public class CampaignAdverts
    {
        public int CampaignAdvertId { get; set; }
        public int CampaignProfileId { get; set; }
        public int AdvertId { get; set; }
        public bool NextStatus { get; set; }
        public int AdtoneServerCampaignAdvertId { get; set; }
    }
}
