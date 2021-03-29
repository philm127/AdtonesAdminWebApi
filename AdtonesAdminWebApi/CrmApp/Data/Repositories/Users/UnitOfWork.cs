using AdtonesAdminWebApi.CrmApp.Application.Interfaces.Users;


namespace AdtonesAdminWebApi.CrmApp.Data.Repositories.Users
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IUserRepository userRepository, ISubscriberRepository subscriberRepository)
        {
            Users = userRepository;
            Subscribers = subscriberRepository;
        }
        public IUserRepository Users { get; }
        public ISubscriberRepository Subscribers { get; }
    }
}