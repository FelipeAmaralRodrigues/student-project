using MassTransit;
using MediatR;
using StudentProject.Domain.Data;
using StudentProject.Domain.Entities;
using StudentProject.Domain.Mediator;
using StudentProject.Domain.Mediator.Messages;
using StudentProject.Domain.Mediator.Notifications;

namespace StudentProject.Domain.Students.Commands
{
    public class CreateStudentCommand : Command<Student>
    {
        public Guid UId { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }

        public void SetUId(Guid uid) => UId = uid;
    }

    public class CreateStudentCommandHandler :  CommandHandler, IRequestHandler<CreateStudentCommand, Student>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBus _bus;

        public CreateStudentCommandHandler(
            IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications,
            IUnitOfWork unitOfWork,
            IBus bus)
            : base(mediator, notifications)
        {
            _unitOfWork = unitOfWork;
            _bus = bus;
        }

        public async Task<Student> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var student = StudentFactory.CreateStudent(request.FirstName, request.LastName, request.BirthDate, request.Email);
                await _unitOfWork.StudentRepository.CreateAsync(student, cancellationToken);
                await _unitOfWork.SaveAsync(cancellationToken);

                request.SetUId(student.UId);

                return student;
            }
            catch (Exception e)
            {
                await _notifications.Handle(new DomainNotification("request", "Internal server error. Please try again later"), cancellationToken);
                throw;
            }
        }
    }
}
