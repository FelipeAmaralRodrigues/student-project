using MassTransit;
using MediatR;
using StudentProject.Contracts;
using StudentProject.Domain.Data;
using StudentProject.Domain.Entities;
using StudentProject.Domain.Mediator;
using StudentProject.Domain.Mediator.Messages;
using StudentProject.Domain.Mediator.Notifications;

namespace StudentProject.Domain.Students.Commands
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

        public async Task<bool> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Cria um novo objeto 'student' usando a fábrica de estudantes com as informações fornecidas na 'request'
                var student = StudentFactory.CreateStudent(request.FirstName, request.LastName, request.BirthDate, request.Email);
                // Adiciona o novo estudante no repositório de estudantes de forma assíncrona.
                await _unitOfWork.StudentRepository.CreateAsync(student, cancellationToken);
                // Salva as alterações no banco de dados de forma assíncrona.
                await _unitOfWork.SaveAsync(cancellationToken);

                // Define o UId do 'request' com o UId do novo estudante.
                request.SetUId(student.UId);

                // Publica um evento 'StudentCreated' com os dados do novo estudante.
                await _bus.Publish<StudentCreated>(new StudentCreated
                {
                    Id = student.Id,
                    UId = student.UId,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    BirthDate = student.BirthDate,
                    Email = student.Email
                });

                return true;
            }
            catch (Exception e)
            {
                // Em caso de exceção, gera uma notificação de domínio indicando erro interno do servidor.
                await _notifications.Handle(new DomainNotification("request", "Internal server error. Please try again later"), cancellationToken);

                // Publica um evento 'CreateStudentFailed' com os dados da solicitação e informações da exceção.
                await _bus.Publish<CreateStudentFailed>(new CreateStudentFailed
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate,
                    Email = request.Email,

                    ExceptionMessage = e.Message,
                    ExceptionStackTrace = e.StackTrace,
                    ExceptionType = e.GetType().ToString()
                });
            }
            return false;
        }
    }
}
