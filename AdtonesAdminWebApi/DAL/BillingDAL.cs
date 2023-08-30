using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
// using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.Command;
using AdtonesAdminWebApi.ViewModels.DTOs;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class BillingDAL : BaseDAL, IBillingDAL
    {


        public BillingDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }


        public async Task<int> AddBillingRecord(UserPaymentCommand command)
        {
            string insertIntoBilling = @"INSERT INTO Billing(UserId, CampaignProfileId,PaymentMethodId,InvoiceNumber,
                                                    PONumber,FundAmount,TaxPercantage,TotalAmount,PaymentDate,SettledDate,Status,CurrencyCode,AdtoneServerBillingId)
                                                    VALUES(@UserId, @CampaignProfileId,@PaymentMethodId,@InvoiceNumber,
                                                    @PONumber, @Fundamount, @TaxPercantage, @TotalAmount, GETDATE(),@SettledDate,@Status,@CurrencyCode,@AdtoneServerBillingId);
                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
            int billId = 0;
            try
            {
                billId = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(insertIntoBilling, new
                                    {
                                        UserId = command.AdvertiserId,
                                        CampaignProfileId = command.CampaignProfileId,
                                        PaymentMethodId = command.PaymentMethodId,
                                        Status = command.Status,
                                        TaxPercantage = command.TaxPercantage,
                                        InvoiceNumber = command.InvoiceNumber,
                                        PONumber = command.PONumber,
                                        Fundamount = command.Fundamount,
                                        TotalAmount = command.TotalAmount,
                                        SettledDate = command.SettledDate,
                                        CurrencyCode = command.CurrencyCode,
                                        AdtoneServerBillingId = command.AdtoneServerBillingId
                                    }));

                var strLst = await _connService.GetConnectionStringsByUserId(command.AdvertiserId);
                if (strLst != null && strLst.Count > 0)
                {
                    try
                    {
                        foreach (var constr in strLst)
                        {
                            var campId = await _connService.GetCampaignProfileIdFromAdtoneIdByConn(command.CampaignProfileId, constr);

                            var userId = await _executers.ExecuteCommand(constr,
                                                                        conn => conn.ExecuteScalar<int>("SELECT UserId FROM Users WHERE AdtoneServerUserId=@Id",
                                                                                                            new { Id = command.AdvertiserId }));

                            var x = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(insertIntoBilling, new
                                        {
                                            UserId = userId,
                                            CampaignProfileId = campId,
                                            PaymentMethodId = command.PaymentMethodId,
                                            Status = command.Status,
                                            InvoiceNumber = command.InvoiceNumber,
                                            PONumber = command.PONumber,
                                            Fundamount = command.Fundamount,
                                            TaxPercantage = command.TaxPercantage,
                                            TotalAmount = command.TotalAmount,
                                            SettledDate = command.SettledDate,
                                            CurrencyCode = command.CurrencyCode,
                                            AdtoneServerBillingId = billId
                                        }));
                        }
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


        public async Task<InvoiceForPDFDto> GetInvoiceDetailsForPDF(int billingId)
        {
            string getInvoiceDetailsForPDF = @"SELECT bil.InvoiceNumber, camp.CampaignName,pay.Description AS MethodOfPayment,
                                                        CONCAT(usr.FirstName,' ',usr.LastName) AS FullName,
                                                        bil.PaymentMethodId,bil.CampaignProfileId,comp.CompanyName,comp.Address AS AddressLine1,comp.AdditionalAddress AS AddressLine2,
                                                        comp.Town AS City,comp.PostCode,ct.Name AS InvoiceCountry,ct.ShortName, bil.FundAmount,bil.PONumber,ccred.CreditPeriod, con.PhoneNumber,
                                                        usr.Email,tx.TaxPercantage AS InvoiceTax
                                                        FROM Billing AS bil
                                                        INNER JOIN Users AS usr ON usr.UserId=bil.UserId
                                                        LEFT JOIN CompanyDetails AS comp ON comp.UserId=usr.UserId
                                                        LEFT JOIN Contacts AS con ON con.UserId=usr.UserId
                                                        LEFT JOIN CampaignProfile AS camp ON camp.CampaignProfileId=bil.CampaignProfileId
                                                        LEFT JOIN CampaignCreditPeriods AS ccred ON ccred.CampaignProfileId=camp.CampaignProfileId
                                                        LEFT JOIN PaymentMethod AS pay ON pay.Id=bil.PaymentMethodId
                                                        LEFT JOIN Country AS ct ON ct.Id=comp.CountryId
                                                        LEFT JOIN CountryTax AS tx ON tx.CountryId=ct.Id
                                                        WHERE bil.Id=@Id";
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<InvoiceForPDFDto>(getInvoiceDetailsForPDF, new { Id = billingId }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> GetCreditPeriod(int campaignId)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<int>("SELECT CreditPeriod FROM CampaignCreditPeriods WHERE CampaignProfileId=@Id",
                                                                                new { Id = campaignId }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<BillingPaymentDto> GetCampaignBillingData(int campaignId)
        {
                string GetCampaignBillingData = @"SELECT camp.CampaignProfileId,camp.UserId AS AdvertiserId,u.UserId,cred.AssignCredit AS AssignedCredit,
                                                    cred.AvailableCredit,camp.CountryId,tx.TaxPercantage,camp.TotalBudget AS TotalFundAmount,
                                                    camp.CurrencyCode,cur.CurrencyId,ISNULL(con.PhoneNumber,con.MobileNumber) AS PhoneNumber,
                                                    u.Email,u.Outstandingdays
                                                    FROM CampaignProfile AS camp
                                                    INNER JOIN Users As u ON u.UserId=camp.UserId
                                                    INNER JOIN UsersCredit AS cred ON cred.UserId=camp.UserId
                                                    INNER JOIN CountryTax AS tx ON tx.CountryId=camp.CountryId
                                                    INNER JOIN Currencies AS cur ON camp.CurrencyCode=cur.CurrencyCode
                                                    INNER JOIN Contacts con ON con.UserId=u.UserId
                                                    WHERE camp.CampaignProfileId=@Id";

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<BillingPaymentDto>(GetCampaignBillingData,
                                                                                new { Id = campaignId }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<BillingPaymentDto> GetAdvertiserBillingData(int userId)
        {
            string GetCampaignBillingData = @"SELECT camp.CampaignProfileId,camp.UserId AS AdvertiserId,u.UserId,cred.AssignCredit AS AssignedCredit,
                                                    cred.AvailableCredit,camp.CountryId,tx.TaxPercantage,camp.TotalBudget AS TotalFundAmount,
                                                    camp.CurrencyCode,cur.CurrencyId,ISNULL(con.PhoneNumber,con.MobileNumber) AS PhoneNumber,
                                                    u.Email,u.Outstandingdays
                                                    FROM CampaignProfile AS camp
                                                    INNER JOIN Users As u ON u.UserId=camp.UserId
                                                    INNER JOIN UsersCredit AS cred ON cred.UserId=camp.UserId
                                                    INNER JOIN CountryTax AS tx ON tx.CountryId=camp.CountryId
                                                    INNER JOIN Currencies AS cur ON camp.CurrencyCode=cur.CurrencyCode
                                                    INNER JOIN Contacts con ON con.UserId=u.UserId
                                                    WHERE camp.UserId=@Id";

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<BillingPaymentDto>(GetCampaignBillingData,
                                                                                new { Id = userId }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<InvoicePDFEmailDto> GetInvoiceToPDF(int billingId, int UsersCreditPaymentID)
        {
            string getInvoiceToPDF = @"SELECT pay.Description,bil.InvoiceNumber,bil.SettledDate,co.CountryId,ucp.Amount,usr.Email,
                                                    usr.FirstName, usr.LastName
                                                    FROM Billing AS bil 
                                                    INNER JOIN PaymentMethod AS pay ON bil.PaymentMethodId=pay.Id
                                                    INNER JOIN Users AS usr ON bil.UserId=usr.UserId
                                                    LEFT JOIN CompanyDetails AS co ON usr.UserId=co.UserId
                                                    LEFT JOIN UsersCreditPayment AS ucp ON ucp.UserId=bil.UserId
                                                    WHERE bil.Id=@billingId AND ucp.Id=@ucpId;";

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<InvoicePDFEmailDto>(getInvoiceToPDF,
                                                                                new { billingId = billingId, ucpId = UsersCreditPaymentID }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<AdvertiserCreditPaymentDto> GetToPayDetails(int billingId)
        {
            string getPrePaymentDetails = @"SELECT bil.Id AS billingId,bil.InvoiceNumber,camp.CampaignName,bil.UserId,
                                                        CONCAT(usr.FirstName,' ',usr.LastName) AS FullName,bil.CampaignProfileId
                                                        FROM Billing AS bil 
                                                        INNER JOIN Users AS usr ON bil.UserId=usr.UserId
                                                        INNER JOIN CampaignProfile AS camp ON bil.CampaignProfileId=camp.CampaignProfileId
                                                        LEFT JOIN UsersCreditPayment AS ucp ON ucp.UserId=bil.UserId
                                                        WHERE bil.Id=@billingId;";

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<AdvertiserCreditPaymentDto>(getPrePaymentDetails,
                                                                                new { billingId = billingId }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> InsertPaymentFromUser(AdvertiserCreditFormCommand model)
        {
            string UpdateUserCreditPayment = @"INSERT INTO UsersCreditPayment(UserId,BillingId,Amount,Description,Status,CreatedDate,UpdatedDate,
                                                            CampaignprofileId)
                                                          VALUES(@UserId,@BillingId,@Amount,@Description,@Status,GETDATE(),GETDATE(),
                                                            @CampaignprofileId);";
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(UpdateUserCreditPayment);
            try
            {
                builder.AddParameters(new { UserId = model.UserId });
                builder.AddParameters(new { BillingId = model.BillingId });
                builder.AddParameters(new { Amount = model.Amount });
                builder.AddParameters(new { Description = model.Description });
                builder.AddParameters(new { Status = model.Status });
                builder.AddParameters(new { CampaignprofileId = model.CampaignProfileId });


                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }




        
        public async Task<int> UpdateInvoiceSettledDate(int billingId)
        {
            string updateInvoiceSettledDate = @"UPDATE Billing SET SettledDate=GETDATE() WHERE Id=@Id";
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(updateInvoiceSettledDate, new { Id = billingId }));

            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<CreditPaymentHistoryDto>> GetUserCreditPaymentHistory(int id)
        {
            string getPaymentHistory = @"SELECT ucp.Id, ucp.Amount, ucp.CreatedDate, bil.InvoiceNumber 
                                                    FROM UsersCreditPayment AS ucp  LEFT JOIN Billing AS bil ON bil.Id=ucp.BillingId 
                                                    WHERE ucp.UserId=@userid ORDER BY CreatedDate Desc";
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(getPaymentHistory);
            try
            {
                builder.AddParameters(new { userid = id });

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<CreditPaymentHistoryDto>(getPaymentHistory, new { userid = id }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<decimal> GetCreditBalance(int id)
        {
            string GetTotalPaymentsByUser = @"SELECT sum(Amount) FROM UsersCreditPayment WHERE UserId=@UserId GROUP BY UserId";
            string GetTotalBilledByUser = @"SELECT sum(TotalAmount) FROM Billing WHERE PaymentMethodId=1 AND UserId=@UserId 
                                                GROUP BY UserId";
            try
            {
                Task<decimal> credit = _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<decimal>(GetTotalPaymentsByUser, new { UserId = id }));

                Task<decimal> bill = _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<decimal>(GetTotalBilledByUser, new { UserId = id }));

                await Task.WhenAll(credit, bill);

                return credit.Result - bill.Result;
            }
            catch
            {
                throw;
            }
        }


        public async Task<decimal> GetCreditBalanceForInvoicePayment(int billingId)
        {
            string getOutstandingBalanceInvoice = @"SELECT ISNULL((bilit.TotalAmount - ISNULL(CAST(payit.Amount AS decimal(18,2)),0)),0) AS OutstandingAmount 
                                                                FROM
                                                                    (SELECT ISNULL(CAST(bil.TotalAmount AS decimal(18,2)),0) AS TotalAmount,Id  
                                                                    FROM Billing AS bil
                                                                    WHERE bil.Id=@Id) bilit
                                                                LEFT JOIN
                                                                    (SELECT SUM(ISNULL(CAST(ucp.Amount AS decimal(18,2)),0)) AS Amount,BillingId  
                                                                    FROM UsersCreditPayment ucp
                                                                    WHERE BillingId=@Id
                                                                    GROUP BY BillingId) payit
                                                                ON payit.BillingId=bilit.Id";
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<decimal>(getOutstandingBalanceInvoice, new { Id = billingId }));

            }
            catch
            {
                throw;
            }
        }



        public async Task<IEnumerable<int>> GetCampaignResultSetById(int id)
        {
            string selectCampaignCountry = @"SELECT CountryId FROM CampaignProfile WHERE CampaignProfileId=@Id";
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<int>(selectCampaignCountry, new { Id = id }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> InsertCampaignCreditPeriod(CampaignCreditPeriodCommand model)
        {
            string InsertCampaignCredit = @"INSERT INTO CampaignCreditPeriods(CreditPeriod,UserId,CampaignProfileId,UpdatedDate,CreatedDate,AdtoneServerCampaignCreditPeriodId)
                                                                VALUES(@CreditPeriod,@UserId,@CampaignProfileId, GETDATE(),GETDATE(),@AdtoneServerCampaignCreditPeriodId);
                                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
            var campaignCountryId = await GetCampaignResultSetById(model.CampaignProfileId);

            try
            {
                foreach (var countryId in campaignCountryId)
                {
                    model.AdtoneServerCampaignCreditPeriodId = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(InsertCampaignCredit, model));

                    var lst = await _connService.GetConnectionStringsByCountry(countryId);
                    List<string> conns = lst.ToList();

                    foreach (string constr in conns)
                    {
                        model.UserId = await _connService.GetUserIdFromAdtoneIdByConnString(model.UserId, constr);
                        model.CampaignProfileId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                        var x = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(InsertCampaignCredit, model));
                    }
                }
            }
            catch
            {
                throw;
            }
            return campaignCountryId.FirstOrDefault();
        }


        public async Task<int> UpdateCampaignCreditPeriod(CampaignCreditPeriodCommand model)
        {
            string UpdateCampaignCredit = @"UPDATE CampaignCreditPeriods SET CreditPeriod=@CreditPeriod WHERE ";
            var sb = new StringBuilder();
            sb.Append(UpdateCampaignCredit);
            var sbOp = new StringBuilder();
            sbOp.Append(UpdateCampaignCredit);
            sb.Append(" CampaignCreditPeriodId=@Id;");
            sbOp.Append(" AdtoneServerCampaignCreditPeriodId=@Id;");


            try
            {
                var x = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(sb.ToString(), new { Id = model.CampaignCreditPeriodId, CreditPeriod = model.CreditPeriod }));


                var campaignCountryId = await GetCampaignResultSetById(model.CampaignProfileId);
                    foreach (var countryId in campaignCountryId)
                    {

                    var lst = await _connService.GetConnectionStringsByCountry(countryId);
                    List<string> conns = lst.ToList();

                    foreach (string constr in conns)
                    {
                        x = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(sbOp.ToString(), new { Id = model.CampaignCreditPeriodId, CreditPeriod = model.CreditPeriod }));
                    }
                }
            }
            catch
            {
                throw;
            }
            return 0;
        }

    }
}
