using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class ProfileMatchInformationFormModel
    {
        public int Id { get; set; }
        public string ProfileName { get; set; }

        public bool IsActive { get; set; }

        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string ProfileType { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // public ProfileMatchLabelFormModel profileMatchLabelFormModel { get; set; }
        public List<ProfileMatchLabelFormModel> profileMatchLabelFormModels { get; set; }

        public ProfileMatchInformationFormModel()
        {
            // profileMatchLabelFormModel = new ProfileMatchLabelFormModel();
            profileMatchLabelFormModels = new List<ProfileMatchLabelFormModel>();
        }
    }
}