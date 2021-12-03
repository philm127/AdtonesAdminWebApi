using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class ClientViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public int CountryId { get; set; }
        public int OperatorId { get; set; }
        public int? AdtoneServerClientId { get; set; }

    }
}
