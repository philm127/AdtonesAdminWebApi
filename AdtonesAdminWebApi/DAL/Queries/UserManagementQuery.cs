

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class UserManagementQuery
    {

        public static string CheckUserExists => @"SELECT COUNT(1) FROM Users WHERE LOWER(Email) = @email;";

        public static string CheckContactExists => @"SELECT COUNT(1) FROM Contacts WHERE MobileNumber=@mobile;";



        public static string AddNewUser => @"INSERT INTO Users(Email,FirstName,LastName,PasswordHash,DateCreated,Organisation,LastLoginTime,RoleId,
                                                Activated,VerificationStatus,Outstandingdays,OperatorId,IsMsisdnMatch,IsEmailVerfication,
                                                PhoneticAlphabet,IsMobileVerfication,OrganisationTypeId,UserMatchTableName,Permissions,LastPasswordChangedDate)
                                             VALUES(@Email,@FirstName,@LastName,@PasswordHash,GETDATE(),@Organisation,GETDATE(),@RoleId,
                                                    @Activated,@VerificationStatus,@Outstandingdays,@OperatorId,@IsMsisdnMatch,@IsEmailVerfication,
                                                    @PhoneticAlphabet,@IsMobileVerfication,@OrganisationTypeId,@UserMatchTableName,@Permissions,GETDATE());
                                      SELECT CAST(SCOPE_IDENTITY() AS INT);";


        /// <summary>
        /// Doesn't have permissions as remote may not have and does have adtone serverid
        /// </summary>
        public static string AddNewUserToOperator => @"INSERT INTO Users(Email,FirstName,LastName,PasswordHash,DateCreated,Organisation,LastLoginTime,RoleId,
                                                Activated,VerificationStatus,Outstandingdays,OperatorId,IsMsisdnMatch,IsEmailVerfication,
                                                PhoneticAlphabet,IsMobileVerfication,OrganisationTypeId,UserMatchTableName,AdtoneServerUserId,LastPasswordChangedDate)
                                             VALUES(@Email,@FirstName,@LastName,@PasswordHash,GETDATE(),@Organisation,GETDATE(),@RoleId,
                                                    @Activated,@VerificationStatus,@Outstandingdays,@OperatorId,@IsMsisdnMatch,@IsEmailVerfication,
                                                    @PhoneticAlphabet,@IsMobileVerfication,@OrganisationTypeId,@UserMatchTableName,@AdtoneServerUserId,GETDATE());
                                      SELECT CAST(SCOPE_IDENTITY() AS INT);";


        public static string AddNewContact => @"INSERT INTO Contacts(UserId,MobileNumber,FixedLine,Email,PhoneNumber,Address,CountryId,CurrencyId,AdtoneServerContactId)
                                               VALUES(@UserId,@MobileNumber,@FixedLine,@Email,@PhoneNumber,@Address,@CountryId,@CurrencyId,@AdtoneServerContactId);
                                                SELECT CAST(SCOPE_IDENTITY() AS INT);";
        
        
        public static string UpdateUserStatus => @"UPDATE Users SET Activated=@Activated,LastPasswordChangedDate=GETDATE() WHERE ";
                                                                  
        public static string GetUserDetails => @"SELECT UserId,u.OperatorId,Email,FirstName,LastName,DateCreated,Organisation,op.CountryId,
                                        Activated,RoleId,OrganisationTypeId,Outstandingdays,AdtoneServerUserId,VerificationStatus,PasswordHash 
                                        FROM Users AS u LEFT JOIN Operators AS op ON op.OperatorId=u.OperatorId 
                                        WHERE ";


        public static string getContactByUserId => @"SELECT Id, UserId,ISNULL(MobileNumber,'-') AS MobileNumber,FixedLine,Email, PhoneNumber,Address,CountryId,CurrencyId 
                                                                                                FROM Contacts WHERE UserId = @userid ";

        // Comes from SoapApiService
        public static string UpdateCorpUser => "UPDATE Users SET Activated=3, IsMsisdnMatch=false WHERE UserId=@Id;";


        public static string GetCompanyDetails => @"SELECT Id, UserId,ISNULL(CompanyName,'-') AS CompanyName,Address,AdditionalAddress,
                                            Town,PostCode,CountryId FROM CompanyDetails WHERE UserId = @userid ";


        public static string GetOperatorAdmin => @"SELECT u.UserId,FirstName,LastName,Email,Organisation,u.OperatorId,o.CountryId,
                                            c.Name AS CountryName,o.OperatorName,u.Activated,u.DateCreated,con.MobileNumber,
                                            con.PhoneNumber, con.Address,con.Id
                                            FROM Users AS u LEFT JOIN Operators AS o ON u.OperatorId=o.OperatorId
                                            LEFT JOIN Country AS c ON o.CountryId=c.Id
                                            LEFT JOIN Contacts AS con ON con.UserId=u.UserId
                                            WHERE u.UserId=@userId";

        public static string UpdateContacts => @"UPDATE Contacts SET MobileNumber=@MobileNumber,FixedLine=@FixedLine,Email=@Email,
                                            PhoneNumber=@PhoneNumber,Address=@Address,CountryId=@CountryId
                                            WHERE Id = @Id";


        public static string UpdateUser => @"UPDATE Users SET FirstName=@FirstName, LastName = @LastName, Organisation = @Organisation
                                        ,Outstandingdays=@Outstandingdays, RoleId=@RoleId ";


        public static string UpdateUserPermissions => @"Update Users SET Permissions=@perm WHERE UserId=@userId";


        public static string DeleteAddedUser => @"DELETE FROM Users WHERE UserId=@Id";

        public static string InsertManagerToSalesExec => @"INSERT INTO SalesManager_SalesExec(ManId,ExecId,Active,CreatedDate,UpdatedDate)
                                                            VALUES(@manId,@execId,1,GETDATE(),GETDATE());";
    }
}
