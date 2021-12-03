using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum CampaignAuditStatus
    {
        Played = 1,
        Cancelled = 2,
        Short = 3

    }
    public enum CampaignAuditSMSStatus
    {
        Yes = 1,
        No = 2,
        Both = 3

    }
}
