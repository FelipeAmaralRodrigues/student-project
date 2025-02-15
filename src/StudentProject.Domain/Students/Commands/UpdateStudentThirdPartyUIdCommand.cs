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
        public Guid ThirdPartyStudentUId { get; set; }

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
                Student student = StudentFactory.UpdateThirPartyStudentUId(request.GetStudent(), request.ThirdPartyStudentUId);
                await _unitOfWork.StudentRepository.UpdateThirdPartyStudentUIdByStudentIdAsync(student.Id, (Guid)student.ThirdPartyStudentUId, cancellationToken);
                await _unitOfWork.SaveAsync(cancellationToken);

                return student;
            }
            catch (Exception e)
            {
                // Em caso de exceção, gera uma notificação de domínio indicando erro interno do servidor.
                await _notifications.Handle(new DomainNotification("request", "Internal server error. Please try again later"), cancellationToken);
                throw;
            }
        }
    }
}
