using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.DTOs
{

    public class AdvertTableAdvertiserDto
    {
        public int AdvertId { get; set; }
        public string AdvertName { get; set; }
        public string Brand { get; set; }

        // This holds whole string including server
        public string MediaFileLocation { get; set; }

        // This is the file path stored in DB
        public string MediaFile { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        
        public int UserId { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        public int Status { get; set; }

        public int PrevStatus { get; set; }
        public int CampaignProfileId { get; set; } = 0;
        public string CampaignName { get; set; }
        public string rStatus => $"{(Enums.AdvertStatus)Status}";
        public int UpdatedBy { get; set; }
        public int OperatorId { get; set; }
        public Decimal AverageBid { get; set; }
        public int TotalPlays { get; set; }
        public int CountryId { get; set; }
    }

    public class AdvertiserAdvertTableDashboardDto
    {
        public decimal AvgPlayLength { get; set; }
        public long TotalSMS { get; set; }
        public long TotalEmail { get; set; }
        public long TotalPlays { get; set; }
    }
}