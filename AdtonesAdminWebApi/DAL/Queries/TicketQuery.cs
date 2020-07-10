using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public class TicketQuery
    {
        public static string GetLoadTicketDatatable => @"SELECT q.Id,ISNULL(q.UserId,0) AS UserId,ISNULL(pay.Name,'-') AS PaymentMethod,
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


        public static string GetOperatorLoadTicketTable => @"SELECT q.Id,ISNULL(q.UserId,0) AS UserId,q.ClientId,qs.Name AS QuestionSubject,
                                                    ISNULL(CONCAT(u.FirstName,' ',u.LastName), '-') AS UserName,q.Description,
                                                    ISNULL(u.Email, '-') AS Email,ISNULL(QNumber,'-') AS QNumber,
                                                    camp.CampaignName,q.CreatedDate,
                                                    Title AS QuestionTitle,q.Status,LastResponseDateTime,
                                                    ISNULL(u.Organisation,'-') AS Organisation
                                                FROM Question AS q LEFT JOIN Users AS u ON u.UserId=q.UserId
                                                LEFT JOIN Client AS cl ON cl.Id=q.ClientId
                                                LEFT JOIN CampaignProfile AS camp ON camp.CampaignProfileId=q.CampaignProfileId
                                                LEFT JOIN QuestionSubject AS qs ON qs.SubjectId=q.SubjectId
                                                WHERE (u.OperatorId=@operatorId 
                                                       OR u.UserId IN 
                                                                    (SELECT UserId FROM Contacts WHERE CountryId IN
                                                                    (SELECT CountryId FROM Operators WHERE OperatorId=@operatorId)))
                                                AND q.SubjectId IN(@opadrev,@cred,@aderr,@adreview)
                                                AND q.Status < 4
                                                ORDER BY q.Id DESC;";

        public static string GetTicketDetails => @"SELECT q.Id,ISNULL(q.UserId,0) AS UserId,q.ClientId,PaymentMethodId,ISNULL(pay.Name,'-') AS PaymentMethod,
                                                    ISNULL(CONCAT(u.FirstName,' ',u.LastName), '-') AS UserName,
                                                    ISNULL(CONCAT(upd.FirstName,' ',upd.LastName), '-') AS UpdatedName,
                                                    qs.Name AS QuestionSubject,
                                                    ISNULL(u.Email, '-') AS Email,ISNULL(QNumber,'-') AS QNumber,
                                                    ISNULL(cl.Name,'-') AS ClientName,q.CampaignProfileId,camp.CampaignName,q.CreatedDate,
                                                    Title AS QuestionTitle,q.Status,LastResponseDateTime,LastResponseDateTimeByUser,
                                                    ISNULL(u.Organisation,'-') AS Organisation,q.Description
                                                FROM Question AS q LEFT JOIN Users AS u ON u.UserId=q.UserId
                                                LEFT JOIN Client AS cl ON cl.Id=q.ClientId
                                                LEFT JOIN CampaignProfile AS camp ON camp.CampaignProfileId=q.CampaignProfileId
                                                LEFT JOIN QuestionSubject AS qs ON qs.SubjectId=q.SubjectId
                                                LEFT JOIN PaymentMethod AS pay ON pay.Id=q.PaymentMethodId
                                                LEFT JOIN Users AS upd ON q.UpdatedBy=upd.UserId
                                                WHERE q.Id=@Id";

        
        public static string GetTicketComments => @"SELECT qc.Id AS CommentId,qc.UserId,QuestionId,Title AS CommentTitle,Description,
                                                CASE WHEN qci.UploadImages IS NOT NULL THEN CONCAT(@siteAddress,qci.UploadImages) 
                                                    ELSE qci.UploadImages END AS ImageFile,
                                                ResponseDatetime AS CommentDate,TicketCode,CONCAT(u.FirstName,' ',u.LastName) AS UserName
                                                FROM QuestionComment AS qc INNER JOIN Users AS u ON u.UserId=qc.UserId 
                                                LEFT JOIN QuestionCommentImages AS qci ON qci.QuestionCommentId=qc.Id
                                                WHERE qc.QuestionId=@questionId ORDER BY qc.Id DESC";
        
        
        public static string UpdateTicketStatus => "UPDATE Question SET UpdatedDate=GETDATE(),UpdatedBy=@UpdatedBy,Status=@Status,LastResponseDateTime=GETDATE() where Id= @Id";


        public static string CreateNewHelpTicket => @"INSERT INTO dbo.Question(UserId,QNumber,SubjectId,ClientId,CampaignProfileId,PaymentMethodId,
                                                        Title,Description,CreatedDate,UpdatedDate,Status,Email,AdvertId,LastResponseDateTimeByUser,
                                                        LastResponseDateTime,UpdatedBy)
                                                VALUES(@UserId,@QNumber,@SubjectId,@ClientId,@CampaignProfileId,@PaymentMethodId,@Title,
                                                        @Description,GETDATE(),GETDATE(),@Status,@Email,@AdvertId,GETDATE(),GETDATE(),@UserId);";


        // If DB UserId and current user same
        public static string UpdateTicketUpdatedByUser => @"UPDATE Question SET Status=@Status, LastResponseDateTimeByUser=GETDATE(),UpdatedDate=GETDATE(), UpdatedBy=@UpdatedBy WHERE Id=@Id";


        // If DB UserId and current user are different
        public static string UpdateTicketUpdatedByAdmin => @"UPDATE Question SET Status=@Status, LastResponseDateTime=GETDATE(),UpdatedDate=GETDATE(), UpdatedBy=@UpdatedBy WHERE Id=@Id";


        public static string AddComment => @"INSERT INTO QuestionComment(UserId,QuestionId,Description,ResponseDatetime) 
                                            VALUES(@UserId,@QuestionId,@Description,GETDATE());
                                            SELECT CAST(SCOPE_IDENTITY() AS INT);";


        public static string InsertCommentImage => @"INSERT INTO QuestionCommentImages(QuestionCommentId,UploadImages) VALUES(@QuestionCommentId,@UploadImages)";


    }

}
