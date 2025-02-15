using MassTransit;
using StudentProject.Contracts;
using StudentProject.Domain.Data;
using StudentProject.Domain.Entities;

namespace StudentProject.Services.Worker.Consumers
{
    public class UpdateStudentThirdPartyUIdConsumer : IConsumer<UpdateStudentThirdPartyUId>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStudentThirdPartyUIdConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<UpdateStudentThirdPartyUId> context)
        {
            try
            {
                var studentUpdated = StudentFactory.UpdateThirPartyStudentUId(new Student { UId = context.Message.StudentUId }, context.Message.ThirdPlatformStudentUId);
                await _unitOfWork.StudentRepository.UpdateThirdPartyStudentUIdByStudentIdAsync(studentUpdated.Id, (Guid) studentUpdated.ThirdPartyStudentUId, context.CancellationToken);
                await _unitOfWork.SaveAsync(context.CancellationToken);

                await context.Publish<StudentThirdPartyUIdUpdated>(new StudentThirdPartyUIdUpdated
                {
                    RequestUId = context.Message.StudentUId,
                    StudentUId = context.Message.StudentUId,
                    ThirdPlatformStudentUId = context.Message.ThirdPlatformStudentUId
                });
            }
            catch (Exception e)
            {
                await context.Publish<UpdateStudentThirdPartyUIdFailed>(new UpdateStudentThirdPartyUIdFailed
                {
                    RequestUId = context.Message.StudentUId,
                    StudentUId = context.Message.StudentUId,
                    ThirdPlatformStudentUId = context.Message.ThirdPlatformStudentUId,

                    ExceptionMessage = e.Message,
                    ExceptionStackTrace = e.StackTrace,
                    ExceptionType = e.GetType().ToString()
                });
            }
        }
    }
}
