using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetList();
        Task<User?> GetById(Guid id);
        Task<User?> GetByUserId(string userId);
        Task<User?> GetByBaseURI(string URI);
        Task Update(User userEntity);
        Task<bool> IsLinkAvailable(string link, Guid id);
    }
}
