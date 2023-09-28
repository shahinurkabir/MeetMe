using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.EntityFramework.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookingDbContext bookingDbContext;

        public UserRepository(BookingDbContext bookingDbContext)
        {
            this.bookingDbContext = bookingDbContext;
        }

        public async Task<User?> GetUserByLoginId(string userId)
        {
            var user = await this.bookingDbContext.Set<User>()
                .FirstOrDefaultAsync(x => x.UserID == userId);

            return user;
        }

        public async Task<User?> GetUserBySlug(string URI)
        {
            var entity = await bookingDbContext.Set<User>()
                .FirstOrDefaultAsync(e => e.BaseURI.ToLower() == URI.ToLower());
            return entity;
        }

        public async Task<bool> IsUserSlugAvailable(string URI, Guid id)
        {
            var entity = await bookingDbContext.Set<User>()
                            .FirstOrDefaultAsync(e => e.BaseURI.ToLower() == URI.ToLower());

            if (entity == null) return true;

            return entity.Id == id;
        }
        public async Task<List<User>> GetUserList()
        {
            var list = await this.bookingDbContext.Set<User>().ToListAsync();
            return list;
        }

        public async Task<bool> UpdateUser(User userEntity)
        {
            this.bookingDbContext.Set<User>().Update(userEntity);
            await bookingDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            var userEntity = await this.bookingDbContext.Set<User>().FindAsync(id);
            return userEntity;
        }
    }
}
