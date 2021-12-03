using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class AdvertiserCreditSharedListsModel
    {
        public IEnumerable<SharedSelectListViewModel> users { get; set; }
        public IEnumerable<SharedSelectListViewModel> country { get; set; }
        public IEnumerable<SharedSelectListViewModel> currency { get; set; }
        public IEnumerable<SharedSelectListViewModel> addUser { get; set; }
    }
}
