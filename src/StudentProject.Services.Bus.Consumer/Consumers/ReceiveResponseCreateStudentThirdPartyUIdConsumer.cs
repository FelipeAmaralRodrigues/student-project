using MassTransit;
using StudentProject.Contracts;

namespace StudentProject.Services.Bus.Consumer.Consumers
{
    public class ReceiveResponseCreateStudentThirdPartyUIdConsumer : IConsumer<ReceiveResponseCreateStudentThirdPartyUId>
    {
        public ReceiveResponseCreateStudentThirdPartyUIdConsumer()
        {
        }

        public async Task Consume(ConsumeContext<ReceiveResponseCreateStudentThirdPartyUId> context)
        {
            try
            {
                // Trecho implementado para simular que 80% das vezes que se tentar obter um sucesso
                Random random = new Random();
                int chance = random.Next(1, 101);
                if (chance <= 80)
                {
                    await context.Publish(new ResponseCreateStudentThirdPartyUIdWaited
                    {
                        RequestUId = context.Message.RequestUId,
                        StudentUId = context.Message.StudentUId
                    });
                }
                else
                {
                    Guid thirdPartyPlatformUId = Guid.NewGuid();

                    await context.Publish(new ResponseCreateStudentThirdPartyUIdReceived
                    {
                        RequestUId = context.Message.RequestUId,
                        StudentUId = context.Message.StudentUId,
                        ThirdPartyUId = thirdPartyPlatformUId
                    });
                }
            }
            catch (Exception e)
            {
                await context.Publish(new ReceiveResponseCreateStudentThirdPartyUIdFailed
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
