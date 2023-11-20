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
    public class AppointmentListQuery : IRequest<AppointmentsPaginationResult>
    {
        public AppointmentSearchParametersDto SearchParameters { get; set; } = null!;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }
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
            var (totalRecords, result) = await persistenceProvider.GetAppintmentListByParameters(request.SearchParameters, request.PageNumber, request.PageSize);

            var totalPages = totalRecords == 0 ? 0 : (int)Math.Ceiling((double)totalRecords / request.PageSize);
            var paginationInfo = new PaginationInfo
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                IsFirstPage = request.PageNumber == 1,
                IsLastPage = totalRecords == 0 ? true : (request.PageNumber == (int)Math.Ceiling((double)totalRecords / request.PageSize)),
                CurrentPageDataRangeText = totalRecords == 0 ? "0-0" : $"{((request.PageNumber - 1) * request.PageSize) + 1}-{((request.PageNumber * request.PageSize) > totalRecords ? totalRecords : (request.PageNumber * request.PageSize))} of {totalRecords} Events",
                PagerLinks = Enumerable.Range(1, totalPages)
                .Skip(request.PageNumber > 4 ? request.PageNumber - 4 : 0)
                .Take(4)
                .ToList()

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
