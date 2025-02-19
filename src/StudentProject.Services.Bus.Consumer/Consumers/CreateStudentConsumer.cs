using MassTransit;
using MediatR;
using StudentProject.Contracts;
using StudentProject.Domain.Mediator;
using StudentProject.Domain.Mediator.Notifications;
using StudentProject.Domain.Students.Commands;

namespace StudentProject.Contracts
{
    public class CreateStudentConsumer : IConsumer<CreateStudent>
    {
        private readonly IMediatorHandler _mediator;
        private readonly DomainNotificationHandler _notifications;

        public CreateStudentConsumer(IMediatorHandler mediator, INotificationHandler<DomainNotification> notifications)
        {
            _mediator = mediator;
            _notifications = (DomainNotificationHandler)notifications;
        }

        public async Task Consume(ConsumeContext<CreateStudent> context)
        {
            try
            {
                var command = new CreateStudentCommand
                {
                    FirstName = context.Message.FirstName,
                    LastName = context.Message.LastName,
                    BirthDate = context.Message.BirthDate,
                    Email = context.Message.Email
                };
                await _mediator.SendCommand(command, context.CancellationToken);

                if (_notifications.HasNotifications())
                {
                    await context.Publish(new CreateStudentValidationFailed
                    {
                        FirstName = context.Message.FirstName,
                        LastName = context.Message.LastName,
                        BirthDate = context.Message.BirthDate,
                        Email = context.Message.Email,
                        ValidationErrors = _notifications.GetNotifications().ToDictionary(m => m.Key, m => m.Value)
                    });
                }
                else
                {
                    await context.Publish(new StudentCreated
                    {
                        UId = command.UId,
                        FirstName = context.Message.FirstName,
                        LastName = context.Message.LastName,
                        BirthDate = context.Message.BirthDate,
                        Email = context.Message.Email,
                    });
                }
            }
            catch (Exception e)
            {
                await context.Publish(new CreateStudentFailed
                {
                    FirstName = context.Message.FirstName,
                    LastName = context.Message.LastName,
                    BirthDate = context.Message.BirthDate,
                    Email = context.Message.Email,

                    ExceptionMessage = e.Message,
                    ExceptionStackTrace = e.StackTrace,
                    ExceptionType = e.GetType().ToString()
                });
            }
        }
    }
}
