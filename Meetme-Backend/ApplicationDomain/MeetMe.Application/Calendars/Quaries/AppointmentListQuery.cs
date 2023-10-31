using MediatR;
using MeetMe.Core.Dtos;
using MeetMe.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Application.Calendars.Quaries
{
    public class AppointmentListQuery:IRequest<AppointmentsPaginationResult>
    {
        public AppointmentSearchParametersDto SearchParameters { get; set; } = null!;
        public int PageNumber { get; set; }= 1;
        public int PageSize { get; set; }= 20;
    }

    public class AppointmentListQueryHandler : IRequestHandler<AppointmentListQuery, AppointmentsPaginationResult>
    {
        private readonly IPersistenceProvider persistenceProvider;

        public AppointmentListQueryHandler(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }

        public async Task<AppointmentsPaginationResult> Handle(AppointmentListQuery request, CancellationToken cancellationToken)
        {
            var (totalRecords, result) = await persistenceProvider.GetAppintmentListByParameters(request.SearchParameters,request.PageNumber,request.PageSize);

            var paginationInfo = new PaginationInfo
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize),
                IsFirstPage = request.PageNumber == 1,
                IsLastPage = request.PageNumber == (int)Math.Ceiling((double)totalRecords / request.PageSize)
            };

            var paginationResult = new AppointmentsPaginationResult
            {
                PaginationInfo = paginationInfo,
                Result = new List<AppointmentsByDate>()
            };

            if (result == null || result.Count == 0)
            {
                return paginationResult;
            }
            result.GroupBy(x => x.AppointmentDate).ToList().ForEach(x =>
            {
                paginationResult.Result.Add(new AppointmentsByDate
                {
                    Date = x.Key,
                    Appointments = x.ToList()
                });
            });

            return paginationResult;

        }
    }

}
