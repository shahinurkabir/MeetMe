using MeetMe.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.EntityFramework
{
    public class EFUnitOfWork:IUnitOfWork
    {
        private readonly BookingDbContext bookingDbContext;

        public EFUnitOfWork(BookingDbContext bookingDbContext)
        {
            this.bookingDbContext = bookingDbContext;
        }

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
          return await bookingDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

