

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface IProvisionServerQuery
    {
        string CheckExistingMSISDN { get; }
        string CheckIfBatchExists { get; }
    }


    public class ProvisionServerQuery : IProvisionServerQuery
    {
        

    }
}
