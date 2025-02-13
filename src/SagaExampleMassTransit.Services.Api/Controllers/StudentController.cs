using Asp.Versioning;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SagaExampleMassTransit.Domain.Mediator;
using SagaExampleMassTransit.Domain.Mediator.Notifications;
using SagaExampleMassTransit.Domain.Student.Commands;

namespace SagaExampleMassTransit.Services.Api.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public class StudentController : ApiController
    {
        private readonly IMediatorHandler _mediator;

        public StudentController(
            ILogger<StudentController> logger,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(logger, notifications)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("student/create")]
        public async Task<IActionResult> Register([FromBody] CreateStudentCommand command, CancellationToken cancellationToken)
        {
            await _mediator.SendCommand(command, cancellationToken);
            return ResponseApi();
        }
    }
}
