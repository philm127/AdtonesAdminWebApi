
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.OperatorSpecific
{
    public class ExpressoProcessPromoUser : IExpressoProcessPromoUser
    {
        private const int ProvisionBatchSize = 1500;
        ReturnResult result = new ReturnResult();

        private readonly ISavePUToDatabase _putDB;
        private readonly IConfiguration _configuration;
        private readonly ILoggingService _logServ;

        public ExpressoProcessPromoUser(ISavePUToDatabase putDB, IConfiguration configuration, ILoggingService logServ)
        {
            _putDB = putDB;
            _configuration = configuration;
            _logServ = logServ;
        }

        public async Task<ReturnResult> ProcPromotionalUser(HashSet<string> promoMsisdns, PromotionalUserFormModel model)
        {
            try
            {
                var promoMsisdnsList = promoMsisdns.ToList(); // to ensure Skip,Take would handle records in a same order.

                int overallCount = promoMsisdnsList.Count;
                int chunkCount = overallCount / ProvisionBatchSize;
                int processedCount = 0;

                for (int i = 0; i < chunkCount; i++)
                {
                    var nextChunk = promoMsisdnsList.Skip(processedCount).Take(ProvisionBatchSize).ToList();
                    processedCount += nextChunk.Count;
                    await DoExpressoProvisionAndSaveToDatabase(nextChunk, model);
                }

                if (overallCount - processedCount > 0) // processing the rest of the batch.
                    await DoExpressoProvisionAndSaveToDatabase(promoMsisdnsList.Skip(processedCount).ToList(), model);

                result.body = "User(s) added successfully for Expresso ";
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = "Expresso";
                _logServ.ProcedureName = "ProcPromotionalUser";
                await _logServ.LogError();
                
                result.result = 0;
                result.error = "Failed to process users";
                return result;
            }
        }


        private async Task<bool> DoExpressoProvisionAndSaveToDatabase(List<string> nextChunk, PromotionalUserFormModel model)
        {
            try
            {
                var requests = nextChunk.Select(isdn => new ProvisionModelRequest { isdn = isdn, provision = true }).ToList();
                var provisionResults = new List<ProvisionModel>();
                // calling Expresso HLR
                var test = _configuration.GetValue<bool>("Environment:TestOperatorServer");
                if (!test)
                {
                    provisionResults = await ExpressoOperator.ExpressoProvisionBatch(requests);
                }
                else
                {
                    provisionResults = await ExpressoOperator.TESTExpressoProvisionBatch(requests);
                }
                // Saving records to Operator's DB.

                await _putDB.DoSaveToDatabase(provisionResults,
                    (source, table) => // this is a converter method that transforms all ProvisionRequests to a List of DataRows.
                                    source
                                    //.Where(ExpressoOperator.IsSuccess)   //TODO!!! uncomment to save ONLY NEWLY ONBOARDED (code==0001). 
                                    // Means only ACTIVE records will be saved to DB and Fail will be filtered out.
                                    .Select(pr =>
                                    {
                                        var row = table.NewRow();
                                        row.BeginEdit();
                                        row["MSISDN"] = pr.isdn;
                                        row["WeeklyPlay"] = 0;
                                        row["DailyPlay"] = 0;
                                        row["Status"] = ExpressoOperator.IsSuccess(pr)
                                            ? (int)Enums.PromotionalUserStatus.Active
                                            : (int)Enums.PromotionalUserStatus.Fail;
                                        row["BatchID"] = model.BatchID;
                                        row.EndEdit();
                                        return row;
                                    }).ToList(),
                                model.OperatorId);
                return true;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = "Expresso";
                _logServ.ProcedureName = "ProcPromotionalUser";
                await _logServ.LogError();
                throw;
            }
        }
    }
}
