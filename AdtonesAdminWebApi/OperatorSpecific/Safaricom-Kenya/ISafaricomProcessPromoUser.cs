using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.OperatorSpecific
{
    public interface ISafaricomProcessPromoUser
    {
        Task<ReturnResult> ProcPromotionalUser(HashSet<string> promoMsisdns, PromotionalUserFormModel model);
    }
}
