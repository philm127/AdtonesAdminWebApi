using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    /// <summary>
    /// To hold individual items or a couple so that we can take
    /// fromthe body and not the url
    /// </summary>
    public class IdCollectionViewModel
    {
        public int id { get; set; } = 0;
        public int userId { get; set; } = 0;
        public int currencyId { get; set; } = 0;
        public int countryId { get; set; } = 0;
        public int billingId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string rStatus { get; set; } = string.Empty;
        public int status { get; set; } = 0;
        public int operatorId { get; set; }
        public List<PermissionModel> permData { get; set; }
    }
}
