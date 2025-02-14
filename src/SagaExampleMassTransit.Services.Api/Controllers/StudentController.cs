using Asp.Versioning;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SagaExampleMassTransit.Domain.Mediator;
using SagaExampleMassTransit.Domain.Mediator.Notifications;
using SagaExampleMassTransit.Domain.Students.Commands;
using SagaExampleMassTransit.Domain.Students.Queries;

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
            await _mediator.SendCommand(command, cancellationToken);
            return ResponseApi(command);
        }

    }
}
