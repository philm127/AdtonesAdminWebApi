using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels.CreateUpdateCampaign
{
    public class NewAdvertFormModel
    {
        public int AdvertId { get; set; }

        public int AdvertiserId { get; set; }

        public string Brand { get; set; }

        public string AdvertName { get; set; }

        public int AdvertCategoryId { get; set; }


        public string Script { get; set; }

        public string Numberofadsinabatch { get; set; }

        public string PhoneticAlphabet { get; set; }

        public bool IsTermChecked { get; set; }


        public string ScriptFileLocation { get; set; }

        public string MediaFileLocation { get; set; }

        public bool UploadedToMediaServer { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        public int UpdatedBy { get; set; }

        public int Status { get; set; }

        public bool IsAdminApproval { get; set; }

        public int CountryId { get; set; }

        public int SoapToneId { get; set; }

        public bool NextStatus { get; set; }

        public int CampaignProfileId { get; set; }

        public int? AdtoneServerAdvertId { get; set; }

        public int OperatorId { get; set; }

        public int? ClientId { get; set; }

        public List<IFormFile> file { get; set; }

        public IFormFile MediaFile { get; set; }

        public bool FileUpdate { get; set; } = false;
        public IFormFile ScriptFile { get; set; }
    }


 }