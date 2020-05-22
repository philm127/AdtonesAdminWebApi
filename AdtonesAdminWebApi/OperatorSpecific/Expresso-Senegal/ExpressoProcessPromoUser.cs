﻿
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
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
        public async Task<ReturnResult> ProcPromotionalUser(HashSet<string> promoMsisdns, string DestinationTableName,
                                                            string operatorConnectionString, PromotionalUserFormModel model)
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
                    await DoExpressoProvisionAndSaveToDatabase(nextChunk, operatorConnectionString, DestinationTableName, model);
                }

                if (overallCount - processedCount > 0) // processing the rest of the batch.
                    await DoExpressoProvisionAndSaveToDatabase(promoMsisdnsList.Skip(processedCount).ToList(), operatorConnectionString, DestinationTableName, model);

                result.body = "User(s) added successfully for Expresso ";
                return result;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "Expresso",
                    ProcedureName = "ProcPromotionalUser"
                };
                _logging.LogError();
                result.result = 0;
                result.error = "Failed to process users";
                return result;
            }
        }


        private async Task<bool> DoExpressoProvisionAndSaveToDatabase(List<string> nextChunk, string operatorConnectionString, string destinationTableName, PromotionalUserFormModel model)
        {
            try
            {
                var requests = nextChunk.Select(isdn => new ProvisionModelRequest { isdn = isdn, provision = true }).ToList();

                // calling Expresso HLR
                var provisionResults = await ExpressoOperator.ExpressoProvisionBatch(requests);

                // Saving records to Operator's DB.
                SavePUToDatabase pu = new SavePUToDatabase();
                await pu.DoSaveToDatabase(provisionResults,
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
                                        row["DeliveryServerConnectionString"] = model.DeliveryServerConnectionString;
                                        row["DeliveryServerIpAddress"] = model.DeliveryServerIpAddress;
                                        row.EndEdit();
                                        return row;
                                    }).ToList(),
                                operatorConnectionString, destinationTableName);
                return true;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "Expresso",
                    ProcedureName = "ProcPromotionalUser"
                };
                _logging.LogError();
                throw;
            }
        }
    }
}