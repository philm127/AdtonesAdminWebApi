using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Model
{
    public class Client
    {
        public int Id { get; set; }

        public int? UserId { get; set; }


        public string Name { get; set; }


        public string Description { get; set; }


        public string ContactInfo { get; set; }


        public decimal Budget { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public int Status { get; set; }

        public string Email { get; set; }

        public string PhoneticAlphabet { get; set; }

        public bool NextStatus { get; set; }

        public string ContactPhone { get; set; }

        public virtual User User { get; set; }

        public int? CountryId { get; set; }

        public int? AdtoneServerClientId { get; set; }

        public string CurrencyCode { get; set; }

        public virtual Country Country { get; set; }

        // public virtual ICollection<CampaignProfile> CampaignProfiles { get; set; }
        public virtual ICollection<UserAdvertResult> Advert { get; set; }
    }
}
