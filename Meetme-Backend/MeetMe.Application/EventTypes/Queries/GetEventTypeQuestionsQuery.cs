using MeetMe.Core.Persistence.Entities;
using MediatR;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Threading;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;
using FluentValidation.Validators;

namespace MeetMe.Application.EventTypes.Queries
{

    public class GetEventTypeQuestionsQuery : IRequest<List<EventTypeQuestion>>
    {
        public Guid EventTypeId { get; set; }
    };

    public class GetEventTypeQuestionsQueryHandler : IRequestHandler<GetEventTypeQuestionsQuery, List<EventTypeQuestion>>
    {
        private readonly IPersistenceProvider persistenceProvider;
        private readonly ILoginUserInfo _userInfo;

        public GetEventTypeQuestionsQueryHandler(IPersistenceProvider persistenceProvider, ILoginUserInfo userInfo)
        {
            this.persistenceProvider = persistenceProvider;
            _userInfo = userInfo;
        }
        public async Task<List<EventTypeQuestion>> Handle(GetEventTypeQuestionsQuery request, CancellationToken cancellationToken)
        {
            var listQuestion =await persistenceProvider.GetQuestionsByEventId(request.EventTypeId);
            return listQuestion;
        }
    }

}
