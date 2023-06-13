using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetList();
        Task<User?> GetById(string userId);

    }
}
