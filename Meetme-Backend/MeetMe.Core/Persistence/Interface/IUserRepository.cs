using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetUserList();
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByLoginId(string userId);
        Task<User?> GetUserBySlug(string slug);
        Task<bool> UpdateUser(User userEntity);
        Task<bool> IsUserSlugAvailable(string slug, Guid id);
    }
}
