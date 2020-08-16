using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{
    public interface IPrematchProcess
    {
        Task PrematchProcessForCampaign(int campaignId, string conn);
    }



    public  class PrematchProcess : IPrematchProcess
    { 
        
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IConnectionStringService _connService;
        private readonly IUserMatchDAL _matchDAL;

        public PrematchProcess(IHttpContextAccessor httpAccessor, IConnectionStringService connService,
                                IUserMatchDAL matchDAL)
        {
            _httpAccessor = httpAccessor;
            _connService = connService;
            _matchDAL = matchDAL;
        }

        //public static void PreCampaignUsermatchProcess(int userId, string userMatchTableNumber, string conn)
        //{

        //    if (userMatchTableNumber == "UserMatch1")
        //    {
        //        ExecuteSP("CampaignUserMatchSpByUserId", conn, userId);
        //    }
        //    else if (userMatchTableNumber == "UserMatch2")
        //    {
        //        ExecuteSP("CampaignUserMatchSp2ByUserId", conn, userId);
        //    }
        //    else if (userMatchTableNumber == "UserMatch3")
        //    {
        //        ExecuteSP("CampaignUserMatchSp3ByUserId", conn, userId);
        //    }
        //    else if (userMatchTableNumber == "UserMatch4")
        //    {
        //        ExecuteSP("CampaignUserMatchSp4ByUserId", conn, userId);
        //    }
        //    else if (userMatchTableNumber == "UserMatch5")
        //    {
        //        ExecuteSP("CampaignUserMatchSp5ByUserId", conn, userId);
        //    }
        //    else if (userMatchTableNumber == "UserMatch6")
        //    {
        //        ExecuteSP("CampaignUserMatchSp6ByUserId", conn, userId);
        //    }
        //    else if (userMatchTableNumber == "UserMatch7")
        //    {
        //        ExecuteSP("CampaignUserMatchSp7ByUserId", conn, userId);
        //    }
        //    else if (userMatchTableNumber == "UserMatch8")
        //    {
        //        ExecuteSP("CampaignUserMatchSp8ByUserId", conn, userId);
        //    }
        //    else if (userMatchTableNumber == "UserMatch9")
        //    {
        //        ExecuteSP("CampaignUserMatchSp9ByUserId", conn, userId);
        //    }
        //    else if (userMatchTableNumber == "UserMatch10")
        //    {
        //        ExecuteSP("CampaignUserMatchSp10ByUserId", conn, userId);
        //    }

        //    EFMVCDataContex SQLServerEntities = new EFMVCDataContex(conn);
        //    using (SQLServerEntities)
        //    {
        //        var userProfileData = SQLServerEntities.Userprofiles.Where(s => s.UserId == userId).FirstOrDefault();
        //        if (userProfileData != null)
        //        {
        //            var userProfileId = userProfileData.UserProfileId;
        //            var campaignIdList = SQLServerEntities.PreMatch.Where(s => s.MsUserProfileId == userProfileId.ToString()).Select(s => s.MSCampaignProfileId).ToList();
        //            if (campaignIdList.Count() > 0)
        //            {
        //                foreach (var item in campaignIdList)
        //                {
        //                    var campId = Convert.ToInt32(item);
        //                    UpdateCampaignBudget(SQLServerEntities, campId);
        //                }
        //            }
        //        }
        //    }
        //}


        public async Task PrematchProcessForCampaign(int campaignId, string conn)
        {
            await _matchDAL.PrematchProcessForCampaign(campaignId, conn);

            await UpdateCampaignBudget(campaignId, conn);
        }

        private async Task UpdateCampaignBudget(int campaignId, string conn)
        {
            // var campaignData = new CampaignBudgetModel();
            var campaignData = await _matchDAL.GetBudgetAmounts(campaignId, conn);

            if (campaignData != null)
            {
                var bucketCountFloat = (float)campaignData.MaxHourlyBudget / campaignData.MaxBid;
                int bucketCount = 0;
                if (bucketCountFloat > 0)
                {
                    bucketCount = (int)bucketCountFloat;
                }
                await _matchDAL.UpdateBucketCount(campaignId, conn, bucketCount);
            }
            // }

        }


        //public static void ExecuteSP(string spname, string conn, int userId)
        //{
        //    using (SqlConnection con = new SqlConnection(conn))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(spname, con))
        //        {
        //            cmd.CommandTimeout = 3600;
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@UserId", userId);
        //            con.Open();
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //        }
        //    }
        //}

    }
}
