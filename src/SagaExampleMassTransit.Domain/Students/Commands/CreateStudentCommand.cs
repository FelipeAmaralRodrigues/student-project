using MediatR;
using SagaExampleMassTransit.Domain.Data;
using SagaExampleMassTransit.Domain.Entities;
using SagaExampleMassTransit.Domain.Mediator;
using SagaExampleMassTransit.Domain.Mediator.Messages;
using SagaExampleMassTransit.Domain.Mediator.Notifications;

namespace SagaExampleMassTransit.Domain.Students.Commands
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

    public class CreateStudentCommandHandler :  CommandHandler, IRequestHandler<CreateStudentCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateStudentCommandHandler(
            IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications,
            IUnitOfWork unitOfWork)
            : base(mediator, notifications)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var student = StudentFactory.CreateStudent(request.FirstName, request.LastName, request.BirthDate, request.Email);

                await _unitOfWork.StudentRepository.CreateAsync(student, cancellationToken);
                await _unitOfWork.SaveAsync(cancellationToken);

                request.SetUId(student.UId);

                return true;
            }
            catch (Exception e)
            {
                await _notifications.Handle(new DomainNotification("request", "Internal server error. Please try again later"), cancellationToken);
            }
            return false;
        }
    }
}
