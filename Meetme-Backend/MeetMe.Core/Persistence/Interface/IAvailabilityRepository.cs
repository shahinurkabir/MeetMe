using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IAvailabilityRepository
    {
        Task<bool> AddAvailability(Availability availability);
        Task<bool> UpdateAvailability(Availability availability);
        Task<bool> DeleteAvailability(Availability availability);

        Task<Availability> GetAvailability(Guid id);
        Task<List<Availability>> GetListByUserId(Guid userId);
        Task<bool> UpdateAvailabilityName(Guid id ,string nameToUpdate );

        Task<bool> SetDefaultAvailability(Guid id, Guid userId);
    }
}
