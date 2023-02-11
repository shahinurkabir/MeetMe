using MeetMe.Core.Persistence.Entities;
using MediatR;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Threading;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;

namespace MeetMe.Application.EventTypes.Queries
{

    public class GetEventTypeQuestionsQuery : IRequest<List<EventTypeQuestion>>
    {
        public Guid EventTypeId { get; set; }
    };

    public class GetEventTypeQuestionsQueryHandler : IRequestHandler<GetEventTypeQuestionsQuery, List<EventTypeQuestion>>
    {
        private readonly IEventQuestionRepository eventQuestionRepository;
        private readonly IUserInfo userInfo;

        public GetEventTypeQuestionsQueryHandler(IEventQuestionRepository eventQuestionRepository, IUserInfo userInfo)
        {
            this.eventQuestionRepository = eventQuestionRepository;
            this.userInfo = userInfo;
        }
        public async Task<List<EventTypeQuestion>> Handle(GetEventTypeQuestionsQuery request, CancellationToken cancellationToken)
        {
            var listQuestion =await eventQuestionRepository.GetEventQuestionsByEventTypeId(request.EventTypeId);
            return listQuestion;
        }
    }

}
