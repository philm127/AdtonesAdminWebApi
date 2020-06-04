using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface ITicketQuery
    {
        string GetLoadTicketDatatable { get; }
        string GetOperatorLoadTicketTable { get; }
        string GetTicketDetails { get; }
        string UpdateTicketStatus { get; }
        string GetTicketComments { get; }

    }


    public class TicketQuery : ITicketQuery
    {
        public string GetLoadTicketDatatable => @"SELECT q.Id,ISNULL(q.UserId,0) AS UserId,q.ClientId,PaymentMethodId,ISNULL(pay.Name,'-') AS PaymentMethod,
                                                    ISNULL(CONCAT(u.FirstName,' ',u.LastName), '-') AS UserName,qs.Name AS QuestionSubject,
                                                    ISNULL(u.Email, '-') AS Email,ISNULL(QNumber,'-') AS QNumber,
                                                    ISNULL(cl.Name,'-') AS ClientName,q.CampaignProfileId,camp.CampaignName,q.CreatedDate,
                                                    Title AS QuestionTitle,q.Status,LastResponseDateTime,LastResponseDateTimeByUser,
                                                    ISNULL(u.Organisation,'-') AS Organisation
                                                FROM Question AS q LEFT JOIN Users AS u ON u.UserId=q.UserId
                                                LEFT JOIN Client AS cl ON cl.Id=q.ClientId
                                                LEFT JOIN CampaignProfile AS camp ON camp.CampaignProfileId=q.CampaignProfileId
                                                LEFT JOIN QuestionSubject AS qs ON qs.SubjectId=q.SubjectId
                                                LEFT JOIN PaymentMethod AS pay ON pay.Id=q.PaymentMethodId";


        public string GetOperatorLoadTicketTable => @"SELECT q.Id,ISNULL(q.UserId,0) AS UserId,q.ClientId,qs.Name AS QuestionSubject,
                                                    ISNULL(CONCAT(u.FirstName,' ',u.LastName), '-') AS UserName,q.Description,
                                                    ISNULL(CONCAT(upd.FirstName,' ',upd.LastName), '-') AS UpdatedName,
                                                    ISNULL(u.Email, '-') AS Email,ISNULL(QNumber,'-') AS QNumber,
                                                    q.CampaignProfileId,camp.CampaignName,q.CreatedDate,
                                                    Title AS QuestionTitle,q.Status,LastResponseDateTime,
                                                    ISNULL(u.Organisation,'-') AS Organisation
                                                FROM Question AS q LEFT JOIN Users AS u ON u.UserId=q.UserId
                                                LEFT JOIN Client AS cl ON cl.Id=q.ClientId
                                                LEFT JOIN CampaignProfile AS camp ON camp.CampaignProfileId=q.CampaignProfileId
                                                LEFT JOIN QuestionSubject AS qs ON qs.SubjectId=q.SubjectId
                                                LEFT JOIN Users AS upd ON q,UpdatedBy=upd.UserId
                                                WHERE (u.OperatorId=@operatorId 
                                                       OR u.UserId IN 
                                                                    (SELECT UserId FROM Contacts WHERE CountryId IN
                                                                    (SELECT CountryId FROM Operators WHERE OperatorId=@operatorId)))
                                                AND q.SubjectId IN(@opadrev,@cred,@aderr,@adreview)
                                                AND q.Status < 4
                                                ORDER BY q.Id DESC;";

        public string GetTicketDetails => GetLoadTicketDatatable + " WHERE q.Id=@Id";

        
        public string GetTicketComments => @"SELECT qc.Id AS CommentId,qc.UserId,QuestionId,Title AS CommentTitle,Description,
                                                ResponseDatetime AS CommentDate,TicketCode,CONCAT(u.FirstName,' ',u.LastName) AS UserName
                                                FROM QuestionComment AS qc INNER JOIN Users AS u ON u.UserId=qc.UserId WHERE qc.QuestionId=@questionId";
        
        
        public string UpdateTicketStatus => "UPDATE Question SET UpdatedDate=GETDATE(),UpdatedBy=@UpdatedBy,Status=@Status,LastResponseDateTime=GETDATE() where Id= @Id";
        
        
        public string AddProduct => "Insert into  [Dapper].[dbo].[Product] ([Name], Cost, CreatedDate) values (@Name, @Cost, @CreatedDate)";
        
        
        public string UpdateProduct => "Update [Dapper].[dbo].[Product] set Name = @Name, Cost = @Cost, CreatedDate = GETDATE() where Id =@Id";
        
        
        public string RemoveProduct => "Delete from [Dapper].[dbo].[Product] where Id= @Id";
    }

}
