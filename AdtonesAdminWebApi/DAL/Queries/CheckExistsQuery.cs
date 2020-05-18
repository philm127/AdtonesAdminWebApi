

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface ICheckExistsQuery
    {
        string CheckAreaExists { get; }

    }


    public class CheckExistsQuery : ICheckExistsQuery
    {
        public string CheckAreaExists => @"SELECT COUNT(1) FROM Areas WHERE LOWER(AreaName) = @areaname AND CountryId=@countryId";
                                                                  
                

    }
}
