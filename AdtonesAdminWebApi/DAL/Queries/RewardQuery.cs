using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class RewardQuery
    {
        public static string LoadRewardsDataTable => @"SELECT RewardId,RewardName,CONVERT(DECIMAL(18,2),replace(RewardValue, ',', '')) AS RewardValue,
                                                        r.AddedDate AS CreatedDate,r.OperatorId,op.OperatorName
                                                      FROM Rewards AS r LEFT JOIN Operators AS op ON r.OperatorId=op.OperatorId";


        public static string GetRewardDetail => @"SELECT RewardId,RewardName,CONVERT(DECIMAL(18,2),replace(RewardValue, ',', '')) AS RewardValue,
                                                    r.AddedDate,r.UpdatedDate AS CreatedDate,r.OperatorId,op.OperatorName
                                                      FROM Rewards AS r LEFT JOIN Operators AS op ON r.OperatorId=op.OperatorId
                                                       WHERE RewardId=@Id";


        public static string AddReward => @"INSERT INTO Rewards(OperatorId,RewardName,RewardValue,AddedDate,UpdatedDate,AdtoneServerRewardId)
                                                VALUES(@OperatorId,@RewardName,@RewardValue,GETDATE(),GETDATE(),@AdtoneServerRewardId);
                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";


        public static string UpdateReward => @"UPDATE Rewards SET RewardValue = @RewardValue,UpdatedDate = GETDATE() ";



        public static string DeleteReward => @"DELETE FROM Rewards ";



        }
}
