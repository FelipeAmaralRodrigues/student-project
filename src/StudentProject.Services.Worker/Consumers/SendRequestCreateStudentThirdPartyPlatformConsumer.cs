using MassTransit;
using StudentProject.Contracts;

namespace StudentProject.Services.Worker.Consumers
{
    public class SendRequestCreateStudentThirdPartyPlatformConsumer : IConsumer<SendRequestCreateStudentThirdPartyPlatform>
    {
        public SendRequestCreateStudentThirdPartyPlatformConsumer()
        {
            
        }

        public async Task Consume(ConsumeContext<SendRequestCreateStudentThirdPartyPlatform> context)
        {
            try
            {
                Guid requestUId = Guid.NewGuid();

                // simula um request para uma plataforma de terceiro gerando um id de request
                await context.Publish<RequestCreateStudentThirdPartyPlatformSended>(new RequestCreateStudentThirdPartyPlatformSended
                {
                    RequestUId = requestUId,
                    UId = context.Message.UId,
                    FirstName = context.Message.FirstName,
                    LastName = context.Message.LastName,
                    BirthDate = context.Message.BirthDate,
                    Email = context.Message.Email
                });
            }
            catch (Exception e)
            {
                await context.Publish<SendRequestCreateStudentThirdPartyPlatformFailed>(new SendRequestCreateStudentThirdPartyPlatformFailed
                {
                    UId = context.Message.UId,
                    FirstName = context.Message.FirstName,
                    LastName = context.Message.LastName,
                    BirthDate = context.Message.BirthDate,
                    Email = context.Message.Email,

                    ExceptionMessage = e.Message,
                    ExceptionStackTrace = e.StackTrace,
                    ExceptionType = e.GetType().Name
                });
            }
        }
    }
}
