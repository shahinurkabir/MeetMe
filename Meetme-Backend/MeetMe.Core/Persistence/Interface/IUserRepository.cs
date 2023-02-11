using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetList();
        Task<User?> GetById(Guid id);
        Task<User?> GetByName(string name);

    }
}
