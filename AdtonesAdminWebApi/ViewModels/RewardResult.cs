﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class RewardResult
    {
        public int RewardId { get; set; }
        public string RewardName { get; set; }
        public decimal RewardValue { get; set; }
        public DateTime CreatedDate { get; set; }

        //Add 21-02-2019
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
    }
}