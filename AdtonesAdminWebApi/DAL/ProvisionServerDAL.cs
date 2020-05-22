using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class ProvisionServerDAL : IProvisionServerDAL
    {
        private readonly IConfiguration _configuration;
        private readonly IExecutionCommand _executers;

        public ProvisionServerDAL(IConfiguration configuration, IExecutionCommand executers)
        {
            _configuration = configuration;
            _executers = executers;
        }


        public async Task<IEnumerable<string>> GetMsisdnCheckForExisting(string command, string conString)
        {
            try
            {
                return await _executers.ExecuteCommand(conString,
                             conn => conn.Query<string>(command));
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> GetPromoUserBatchIdCheckForExisting(string command, string conString)
        {
            try
            {
                return await _executers.ExecuteCommand(conString,
                             conn => conn.ExecuteScalar<bool>(command));
            }
            catch
            {
                throw;
            }
        }


        //public async Task<int> ChangeAdvertStatus(string query, UserAdvertResult command)
        //{

        //    var sb = new StringBuilder();
        //    sb.Append(query);
        //    sb.Append("AdtoneServerAdvertId=@AdvertId;");

        //    //Model.Advert advert = _advertRepository.GetById(command.AdvertId);
        //    var advertDetail = _advertRepository.GetById(command.AdvertId);
        //    advertDetail.Status = command.Status;
        //    advertDetail.UpdatedBy = command.UpdatedBy;
        //    _advertRepository.Update(advertDetail);
        //    var ConnString = ConnectionString.GetConnectionStringByCountryId(advertDetail.CountryId);
        //    if (ConnString != null && ConnString.Count() > 0)
        //    {
        //        foreach (var item in ConnString)
        //        {
        //            EFMVCDataContex db = new EFMVCDataContex(item);
        //            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, (int)command.UpdatedBy);
        //            var advertData = db.Adverts.Where(s => s.AdtoneServerAdvertId == command.AdvertId).FirstOrDefault();
        //            if (advertData != null)
        //            {
        //                advertData.Status = command.Status;
        //                if (externalServerUserId != 0)
        //                {
        //                    advertData.UpdatedBy = command.UpdatedBy;
        //                }
        //                else
        //                {
        //                    advertData.UpdatedBy = null;
        //                }

        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //    unitOfWork.Commit();
        //    return new CommandResult(true);
        //}


    }
}
