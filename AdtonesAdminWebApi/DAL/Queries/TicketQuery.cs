using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface ITicketQuery
    {
        string GetLoadTicketDatatable { get; }
        string GetTicketDetails { get; }
        string CloseTicket { get; }
    }


    public class TicketQuery : ITicketQuery
    {
        public string GetLoadTicketDatatable => @"SELECT q.Id,ISNULL(q.UserId,0) AS UserId,q.ClientId,PaymentMethodId,ISNULL(pay.Name,'-') AS PaymentMethod,
                                                    ISNULL(CONCAT(u.FirstName,' ',u.LastName), '-') AS UserName,
                                                    ISNULL(u.Email, '-') AS Email,QNumber,
                                                    ISNULL(cl.Name,'-') AS ClientName,q.CampaignProfileId,camp.CampaignName,q.CreatedDate,
                                                    Title,q.Status,LastResponseDateTime,LastResponseDateTimeByUser,
                                                    ISNULL(u.Organisation,'-') AS Organisation
                                                FROM Question AS q LEFT JOIN Users AS u ON u.UserId=q.UserId
                                                LEFT JOIN Client AS cl ON cl.Id=q.ClientId
                                                LEFT JOIN CampaignProfile AS camp ON camp.CampaignProfileId=q.CampaignProfileId
                                                LEFT JOIN QuestionSubject AS qs ON qs.SubjectId=q.SubjectId
                                                LEFT JOIN PaymentMethod AS pay ON pay.Id=q.PaymentMethodId";
        public string GetTicketDetails => GetLoadTicketDatatable + " WHERE q.Id=@Id";
        public string CloseTicket => "UPDATE Question SET UpdatedDate=GETDATE(),UpdatedBy=@UpdatedBy,Status=@Status,LastResponseDateTime=GETDATE() where Id= @Id";
        public string AddProduct => "Insert into  [Dapper].[dbo].[Product] ([Name], Cost, CreatedDate) values (@Name, @Cost, @CreatedDate)";
        public string UpdateProduct => "Update [Dapper].[dbo].[Product] set Name = @Name, Cost = @Cost, CreatedDate = GETDATE() where Id =@Id";
        public string RemoveProduct => "Delete from [Dapper].[dbo].[Product] where Id= @Id";
    }

}
