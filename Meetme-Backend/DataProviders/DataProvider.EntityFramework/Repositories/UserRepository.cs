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

        public async Task<User?> GetById(Guid id)
        {
            var entity = this.bookingDbContext.Set<User>().Find(id);

            return await Task.FromResult(entity);
        }

        public async Task<User?> GetByName(string name)
        {
            var user = await this.bookingDbContext.Set<User>()
                .FirstOrDefaultAsync(x => x.Name==name);

            return user;
        }

        public async Task<List<User>> GetList()
        {
            var list = await this.bookingDbContext.Set<User>().ToListAsync();
            return list;
        }
    }
}
