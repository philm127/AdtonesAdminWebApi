using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.OperatorSpecific
{
    public interface IExpressoProcessPromoUser
    {
        Task<ReturnResult> ProcPromotionalUser(HashSet<string> promoMsisdns, string DestinationTableName,
                                                            string operatorConnectionString, PromotionalUserFormModel model);
    }
}
