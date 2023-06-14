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

        public async Task<User?> GetById(string userId)
        {
            var user = await this.bookingDbContext.Set<User>()
                .FirstOrDefaultAsync(x => x.UserID== userId);

            return user;
        }

        public async Task<User?> GetByBaseURI(string URI)
        {
            var entity = await bookingDbContext.Set<User>()
                .FirstOrDefaultAsync(e=>e.BaseURI.Equals( URI, StringComparison.InvariantCultureIgnoreCase)==true);
            return entity;
        }

        public async Task<List<User>> GetList()
        {
            var list = await this.bookingDbContext.Set<User>().ToListAsync();
            return list;
        }

        public Task Update(User userEntity)
        {
            this.bookingDbContext.Set<User>().Update(userEntity);
            bookingDbContext.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
