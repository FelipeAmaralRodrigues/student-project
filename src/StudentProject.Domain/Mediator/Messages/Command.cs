using FluentValidation.Results;
using MediatR;

namespace StudentProject.Domain.Mediator.Messages
{
    public abstract class Command : Message, IRequest<bool>
    {
        protected Command()
        {
            SetTimestamp(DateTime.UtcNow);
        }
    }

    public abstract class Command<TResult> : Message, IRequest<TResult> where TResult : class
    {
        protected Command()
        {
            SetTimestamp(DateTime.UtcNow);
        }
    }
}
