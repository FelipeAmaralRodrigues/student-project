using MediatR;
using StudentProject.Domain.Data;
using StudentProject.Domain.Entities;
using StudentProject.Domain.Mediator;
using StudentProject.Domain.Mediator.Messages;
using StudentProject.Domain.Mediator.Notifications;

namespace StudentProject.Domain.Students.Commands
{
    public class UpdateStudentThirdPartyUIdCommand : Command<Student>
    {
        public Guid StudentUId { get; set; }
        public Guid ThirdPartyUId { get; set; }

        private Student _student;
        public Student GetStudent() => _student;
        public void SetStudent(Student student) => _student = student;
    }

    public class UpdateStudentThirdPartyUIdCommandHandler : CommandHandler, IRequestHandler<UpdateStudentThirdPartyUIdCommand, Student>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateStudentThirdPartyUIdCommandHandler(
            IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications,
            IUnitOfWork unitOfWork)
            : base(mediator, notifications)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Student> Handle(UpdateStudentThirdPartyUIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Student student = StudentFactory.UpdateThirPartyUId(request.GetStudent(), request.ThirdPartyUId);
                await _unitOfWork.StudentRepository.UpdateThirdPartyUIdByIdAsync(student.Id, (Guid)student.ThirdPartyUId, cancellationToken);
                await _unitOfWork.SaveAsync(cancellationToken);

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
