using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class RewardResult
    {
        public int RewardId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string AddedDate { get; set; }

        //Add 21-02-2019
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
    }
}