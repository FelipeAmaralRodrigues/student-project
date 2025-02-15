using MassTransit;
using MediatR;
using StudentProject.Contracts;
using StudentProject.Domain.Data;
using StudentProject.Domain.Entities;
using StudentProject.Domain.Mediator.Notifications;
using System.Threading;

namespace StudentProject.Services.Worker.Consumers
{
    public class CreateStudentConsumer : IConsumer<CreateStudent>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateStudentConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<CreateStudent> context)
        {
            try
            {
                var student = StudentFactory.CreateStudent(context.Message.FirstName, context.Message.LastName, context.Message.BirthDate, context.Message.Email);
                await _unitOfWork.StudentRepository.CreateAsync(student, context.CancellationToken);
                await _unitOfWork.SaveAsync(context.CancellationToken);

                await context.Publish<StudentCreated>(new StudentCreated
                {
                    Id = student.Id,
                    UId = student.UId,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    BirthDate = student.BirthDate,
                    Email = student.Email
                });
            }
            catch (Exception e)
            {
                await context.Publish<CreateStudentFailed>(new CreateStudentFailed
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
