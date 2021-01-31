using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign
{
    public class NewClientFormModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ClientContactInfo { get; set; }

        public decimal ClientBudget { get; set; }

        public string Email { get; set; }

        public string ClientPhoneticAlphabet { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int ClientStatus { get; set; }

        public bool NextStatus { get; set; }

        public string ContactPhone { get; set; }

        public int? CountryId { get; set; }

        public int? AdtoneServerClientId { get; set; }
    }
}