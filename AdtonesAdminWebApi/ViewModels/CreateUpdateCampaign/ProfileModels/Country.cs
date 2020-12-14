using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels
{
    public class Country
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string Name { get; set; }
        public string ShortName { get; set; }

        public string CountryCode { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int Status { get; set; }

        public string TermAndConditionFileName { get; set; }

        public int? AdtoneServeCountryId { get; set; }

    }
}
