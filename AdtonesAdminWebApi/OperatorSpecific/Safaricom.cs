
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.OperatorSpecific
{
    public class Safaricom : ISafaricom
    {
        private const int ProvisionBatchSize = 1500;
        ReturnResult result = new ReturnResult();
        public async Task<ReturnResult> ProcPromotionalUser(HashSet<string> promoMsisdns, string DestinationTableName,
                                                            string operatorConnectionString, PromotionalUserFormModel model)
        {
            try
            {
                SavePUToDatabase pu = new SavePUToDatabase();
                await pu.DoSaveToDatabase(promoMsisdns,
                                    (source, table) => source.Select(isdn => // this is a converter method that transforms MSISDNs to 
                                                                             // a List of DataRows.
                                    {
                                        var row = table.NewRow();
                                        row.BeginEdit();
                                        row["MSISDN"] = isdn;
                                        row["WeeklyPlay"] = 0;
                                        row["DailyPlay"] = 0;
                                        row["Status"] = (int)PromotionalUserStatus.Active;
                                        row["BatchID"] = model.BatchID;
                                        row["DeliveryServerConnectionString"] = model.DeliveryServerConnectionString;
                                        row["DeliveryServerIpAddress"] = model.DeliveryServerIpAddress;
                                        row.EndEdit();
                                        return row;
                                    }).ToList(),
                                    operatorConnectionString, DestinationTableName);
                result.body = "User(s) added successfully for Safaricom ";
                return result;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "Ssafaricom",
                    ProcedureName = "ProcPromotionalUser"
                };
                _logging.LogError();
                result.result = 0;
                result.error = "Failed to process users";
                return result;
            }
        }
    }
}
