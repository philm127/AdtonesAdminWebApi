using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class BillingDAL : BaseDAL, IBillingDAL
    {


        public BillingDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }


        public async Task<int> AddBillingRecord(BillingPaymentModel command)
        {
            int billId = 0;
            //var builder = new SqlBuilder();
            //var select = builder.AddTemplate(BillingQuery.InsertIntoBilling );
            try
            {
                //builder.AddParameters(new { UserId = command.AdvertiserId });
                //builder.AddParameters(new { ClientId = command.ClientId });
                //builder.AddParameters(new { CampaignProfileId = command.CampaignProfileId });
                //builder.AddParameters(new { PaymentMethodId = command.PaymentMethodId });
                //builder.AddParameters(new { Status = command.Status });
                //builder.AddParameters(new { InvoiceNumber = command.InvoiceNumber });

                //builder.AddParameters(new { PONumber = command.PONumber });
                //builder.AddParameters(new { Fundamount = command.Fundamount });
                //builder.AddParameters(new { TotalAmount = command.TotalAmount });
                //builder.AddParameters(new { SettledDate = command.SettledDate });
                //builder.AddParameters(new { CurrencyCode = command.CurrencyCode });
                //builder.AddParameters(new { AdtoneServerBillingId = command.AdtoneServerBillingId });


                billId = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(BillingQuery.InsertIntoBilling, command));

                var constr = await _connService.GetOperatorConnectionByUserId(command.AdvertiserId);
                if (constr != null && constr.Length > 10)
                {
                    command.AdtoneServerBillingId = billId;
                    try
                    {
                        if (command.ClientId != null)
                            command.ClientId = await _executers.ExecuteCommand(constr,
                                                                        conn => conn.ExecuteScalar<int>("SELECT Id FROM Client WHERE AdtoneServerClientId=@Id",
                                                                                                            new { Id = command.ClientId }));
                        command.AdvertiserId = await _executers.ExecuteCommand(constr,
                                                                    conn => conn.ExecuteScalar<int>("SELECT UserId FROM Users WHERE AdtoneServerUserId=@Id",
                                                                                                        new { Id = command.ClientId }));

                        var x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(BillingQuery.InsertIntoBilling, command));
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }

            return billId;
        }
    }
}
