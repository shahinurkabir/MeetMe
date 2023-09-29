using FluentValidation;
using MediatR;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MeetMe.Application.EventTypes.Commands
{
    public class UpdateInfoCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string Slug { get; set; } = null!;

        public string EventColor { get; set; } = null!;
    }

    public class UpdateInfoCommandHandler : IRequestHandler<UpdateInfoCommand, bool>
    {
        private readonly IPersistenceProvider persistenceProvider;
        private readonly ILoginUserInfo _applicationUser;

        public UpdateInfoCommandHandler(IPersistenceProvider persistenceProvider, ILoginUserInfo applicationUser)
        {
            this.persistenceProvider = persistenceProvider;
            _applicationUser = applicationUser;
        }
        public async Task<bool> Handle(UpdateInfoCommand request, CancellationToken cancellationToken)
        {
            var entity = await persistenceProvider.GetEventTypeById(request.Id);

            if (entity == null) throw new MeetMeException("Event Type not found");

            if (entity.OwnerId != _applicationUser.Id) throw new MeetMeException("Event Type not found");

            entity.Name = request.Name;
            entity.EventColor = request.EventColor;
            entity.Slug = request.Slug;
            entity.Location = request.Location;
            entity.Description = request.Description;

            await persistenceProvider.UpdateEventType(entity);

            return true;
        }
    }

    public class UpdateInfoCommandValidator : AbstractValidator<UpdateInfoCommand>
    {
        private readonly IPersistenceProvider persistenceProvider;
        private readonly ILoginUserInfo applicationUser;

        public UpdateInfoCommandValidator(IPersistenceProvider persistenceProvider, ILoginUserInfo applicationUser)
        {
            this.persistenceProvider = persistenceProvider;
            this.applicationUser = applicationUser;

            RuleFor(m => m.Name).NotEmpty();

            RuleFor(m => m.EventColor).NotEmpty();

            RuleFor(m => m.Slug)
                .MustAsync(async (model, slug, token) =>
                {
                    return await CheckNotUsed(model, token);
                })
                .WithMessage("Slug already used.");
        }

        private async Task<bool> CheckNotUsed(UpdateInfoCommand command, CancellationToken cancellationToken)
        {
            var listEvents = await persistenceProvider.GetEventTypeListByUserId(applicationUser.Id);

            var isUsed = listEvents.Count(e =>
            e.Id != command.Id &&
            e.Slug.Equals(command.Slug, StringComparison.InvariantCultureIgnoreCase)) > 0;

            return isUsed == false;
        }
    }


}
