using MediatR;
using SagaExampleMassTransit.Domain.Mediator;
using SagaExampleMassTransit.Domain.Mediator.Messages;
using SagaExampleMassTransit.Domain.Mediator.Notifications;

namespace SagaExampleMassTransit.Domain.Student.Commands
{
    public class CreateStudentCommand : Command
    {
        public Guid UId { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }

        public void SetUId(Guid uid) => UId = uid;
    }

    public class CreateStudentCommandHandler : 
        CommandHandler, 
        IRequest<CreateStudentCommand>
    {
        public CreateStudentCommandHandler(
            IMediatorHandler mediator, 
            INotificationHandler<DomainNotification> notifications) 
            : base(mediator, notifications)
        {
        }

        public Task Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
