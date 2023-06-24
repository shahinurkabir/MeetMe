﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IAvailabilityRepository
    {
        Task<bool> AddSchedule(Availability scheduleRule);
        Task<bool> UpdateSchedule(Availability scheduleRule);
        Task<bool> DeleteSchedule(Availability scheduleRule);

        Task<Availability> GetById(Guid id);
        Task<List<Availability>> GetListByUserId(Guid userId);
        Task<bool> EditName (Guid id ,string nameToUpdate );

        Task<bool> SetDefault(Guid id, Guid userId);
    }
}
