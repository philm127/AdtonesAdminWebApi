using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels.CreateUpdateCampaign.ProfileModels
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


        public int? CountryId { get; set; }

        public int? AdtoneServerClientId { get; set; }

        public string CurrencyCode { get; set; }
    }
}
