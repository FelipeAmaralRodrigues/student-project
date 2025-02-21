using MassTransit;
using StudentProject.Contracts;

namespace StudentProject.Contracts
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
                    await context.Publish(new ResponseCreateStudentThirdPartyUIdNotReceived
                    {
                        RequestUId = context.Message.RequestUId,
                        StudentUId = context.Message.StudentUId,

                        CorrelationId = context.Message.CorrelationId
                    });
                }
                else
                {
                    Guid thirdPartyPlatformUId = Guid.NewGuid();

                    await context.Publish(new ResponseCreateStudentThirdPartyUIdReceived
                    {
                        RequestUId = context.Message.RequestUId,
                        StudentUId = context.Message.StudentUId,
                        ThirdPartyUId = thirdPartyPlatformUId,

                        CorrelationId = context.Message.CorrelationId
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
                    ExceptionType = e.GetType().ToString(),

                    CorrelationId = context.Message.CorrelationId
                });
            }
        }
    }
}
