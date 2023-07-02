using FluentValidation;
using MediatR;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MeetMe.Application.EventTypes.Update
{
    public class UpdateQuestionCommand : IRequest<bool>
    {
        public Guid EventTypeId { get; set; }
        public List<EventTypeQuestion> Questions { get; set; }

        public UpdateQuestionCommand()
        {
            Questions = new List<EventTypeQuestion>();
        }
    }

    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, bool>
    {
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly IEventQuestionRepository eventQuestionRepository;

        public UpdateQuestionCommandHandler(IEventTypeRepository eventTypeRepository, IEventQuestionRepository  eventQuestionRepository)
        {
            this.eventTypeRepository = eventTypeRepository;
            this.eventQuestionRepository = eventQuestionRepository;
        }
        public async Task<bool> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var eventTypeEntity = await eventTypeRepository.GetEventTypeById(request.EventTypeId);

            if (eventTypeEntity == null)
                throw new MeetMeException("Invalid event");

            var listQuestionEntities = ConvertToEventTypeQuestions(request);

            listQuestionEntities=listQuestionEntities.FindAll(x => x.SystemDefinedYN ==false);

            await eventQuestionRepository.ResetEventQuestions(request.EventTypeId, listQuestionEntities);

            return true;
        }

        private List<EventTypeQuestion> ConvertToEventTypeQuestions(UpdateQuestionCommand request)
        {
            var listEntities = new List<EventTypeQuestion>();

            short displayOrder = 0;

            foreach (var item in request.Questions)
            {
                var questionEntity = new EventTypeQuestion
                {
                    Id = Guid.NewGuid(),
                    EventTypeId = request.EventTypeId,
                    Name = item.Name,
                    QuestionType = item.QuestionType,
                    OtherOptionYN = item.OtherOptionYN,
                    Options = item.Options,
                    RequiredYN = item.RequiredYN,
                    ActiveYN = item.ActiveYN,
                    DisplayOrder = displayOrder,
                    SystemDefinedYN = item.SystemDefinedYN
                };

                listEntities.Add(questionEntity);
                displayOrder++;
            }

            return listEntities;
        }
    }

}
