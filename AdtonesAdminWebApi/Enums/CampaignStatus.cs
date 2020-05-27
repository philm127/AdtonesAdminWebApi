using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Enums
{
    public enum CampaignStatus
    {
        Planned = 1,
        Play = 2,
        Pause = 3,
        Stop = 4,
        Archive = 5,
        // Waitingforadapproval = 6,
        InProgress = 7,
        InsufficientFunds = 8,
        // Waitingforadminapproval = 9
        waitingforapproval = 6,
    }
}