using MassTransit;
using StudentProject.Contracts;

namespace StudentProject.Services.Worker.Consumers
{
    public class ReceiveResponseCreateStudentThirdPartyPlatformConsumer : IConsumer<ReceiveResponseCreateStudentThirdPartyPlatform>
    {
        public ReceiveResponseCreateStudentThirdPartyPlatformConsumer()
        {
        }

        public async Task Consume(ConsumeContext<ReceiveResponseCreateStudentThirdPartyPlatform> context)
        {
            try
            {
                // Trecho implementado para simular que 80% das vezes que se tentar obter um sucesso
                Random random = new Random();
                int chance = random.Next(1, 101);
                if (chance <= 80)
                {
                    await context.Publish<ResponseCreateStudentThirdPartyPlatformWaited>(new ResponseCreateStudentThirdPartyPlatformWaited
                    {
                        RequestUId = context.Message.RequestUId,
                        StudentUId = context.Message.StudentUId
                    });
                } else
                {
                    Guid thirdPartyPlatformUId = Guid.NewGuid();

                    await context.Publish<ResponseCreateStudentThirdPartyPlatformReceived>(new ResponseCreateStudentThirdPartyPlatformReceived
                    {
                        RequestUId = context.Message.RequestUId,
                        StudentUId = context.Message.StudentUId,
                        UId = thirdPartyPlatformUId
                    });
                }
            }
            catch (Exception e)
            {
                await context.Publish<ReceiveResponseCreateStudentThirdPartyPlatformFailed>(new ReceiveResponseCreateStudentThirdPartyPlatformFailed
                {
                    RequestUId = context.Message.RequestUId,
                    StudentUId = context.Message.StudentUId,

                    ExceptionMessage = e.Message,
                    ExceptionStackTrace = e.StackTrace,
                    ExceptionType = e.GetType().ToString()
                });
            }
        }
    }
}
