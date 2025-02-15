using Asp.Versioning;
using MassTransit;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProject.Contracts;
using StudentProject.Domain.Mediator;
using StudentProject.Domain.Mediator.Notifications;
using StudentProject.Domain.Students.Commands;
using StudentProject.Domain.Students.Queries;

namespace StudentProject.Services.Api.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public class StudentController : ApiController
    {
        private readonly IMediatorHandler _mediator;
        private readonly IBus _bus;

        public StudentController(
            ILogger<StudentController> logger,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator,
            IBus bus) : base(logger, notifications)
        {
            _mediator = mediator;
            _bus = bus;
        }

        [AllowAnonymous]
        [HttpGet("students")]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var students = await _mediator.Query(new GetAllQuery(), cancellationToken);
            return ResponseApi(students);
        }

        [AllowAnonymous]
        [HttpGet("student/{uid}")]
        public async Task<IActionResult> GetByUIdAsync(Guid uid,CancellationToken cancellationToken)
        {
            var student = await _mediator.Query(new GetByUIdQuery { UId = uid}, cancellationToken);
            return ResponseApi(student);
        }

        [AllowAnonymous]
        [HttpPost("student/create")]
        public async Task<IActionResult> CreateStudentAsync([FromBody] CreateStudentCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                await _notifications.Handle(new DomainNotification("request", "The request body cannot be null"), cancellationToken);

            try
            {
                await _mediator.SendCommand(command, cancellationToken);

                if (!_notifications.HasNotifications())
                {
                    await _bus.Publish<StudentCreated>(new StudentCreated
                    {
                        UId = command.UId,
                        FirstName = command.FirstName,
                        LastName = command.LastName,
                        BirthDate = command.BirthDate,
                        Email = command.Email
                    });
                }
                else
                {
                    await _bus.Publish<CreateStudentValidationFailed>(new CreateStudentValidationFailed
                    {
                        FirstName = command.FirstName,
                        LastName = command.LastName,
                        BirthDate = command.BirthDate,
                        Email = command.Email,
                        ValidationErrors = _notifications.GetNotifications().ToDictionary(a => a.Key, b => b.Value)
                    });
                }
            }
            catch (Exception e)
            {
                await _bus.Publish<CreateStudentFailed>(new CreateStudentFailed
                {
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    BirthDate = command.BirthDate,
                    Email = command.Email,

                    ExceptionMessage = e.Message.ToString(),
                    ExceptionStackTrace = e.StackTrace,
                    ExceptionType = e.GetType().ToString(),
                });
            }
            return ResponseApi(command);
        }

    }
}
