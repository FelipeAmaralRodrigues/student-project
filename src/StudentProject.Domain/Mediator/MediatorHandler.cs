using MediatR;
using StudentProject.Domain.Mediator.Messages;

namespace StudentProject.Domain.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T e, CancellationToken cancellationToken = default) where T : Event;
        Task<bool> SendCommand<T>(T c, CancellationToken cancellationToken = default) where T : Command;
        Task<TResponse> SendCommand<TResponse>(Command<TResponse> c, CancellationToken cancellationToken = default) 
            where TResponse : class;
        Task<TResponse?> Query<TResponse>(IRequest<TResponse> query, CancellationToken cancellationToken = default);
    }

    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> SendCommand<T>(T c, CancellationToken cancellationToken = default) where T : Command
            => await _mediator.Send(c, cancellationToken);

        public async Task PublishEvent<T>(T e, CancellationToken cancellationToken = default) where T : Event
            => await _mediator.Publish(e, cancellationToken);

        public async Task<TResponse?> Query<TResponse>(IRequest<TResponse> query, CancellationToken cancellationToken = default)
        => await _mediator.Send(query, cancellationToken);

        public async Task<TResponse> SendCommand<TResponse>(Command<TResponse> c, CancellationToken cancellationToken = default)
            where TResponse : class
            => await _mediator.Send<TResponse>(c, cancellationToken);
    }
}
