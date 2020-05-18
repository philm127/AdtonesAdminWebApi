using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ICheckExistsDAL
    {
        Task<bool> CheckAreaExists(string command, AreaResult areamodel);
    }
}
