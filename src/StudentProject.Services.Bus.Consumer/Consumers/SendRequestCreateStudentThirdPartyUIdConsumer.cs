﻿using MassTransit;
using StudentProject.Contracts;

namespace StudentProject.Contracts
{
    public class SendRequestCreateStudentThirdPartyUIdConsumer : IConsumer<SendRequestCreateStudentThirdPartyUId>
    {
        public SendRequestCreateStudentThirdPartyUIdConsumer()
        {

        }

        public async Task Consume(ConsumeContext<SendRequestCreateStudentThirdPartyUId> context)
        {
            try
            {
                Guid requestUId = NewId.NextGuid();

                // simula um request para uma plataforma de terceiro gerando um id de request
                await context.Publish(new RequestCreateStudentThirdPartyUIdSended
                {
                    RequestUId = requestUId,
                    UId = context.Message.UId,
                    FirstName = context.Message.FirstName,
                    LastName = context.Message.LastName,
                    BirthDate = context.Message.BirthDate,
                    Email = context.Message.Email,

                    CorrelationId = context.Message.CorrelationId
                });
            }
            catch (Exception e)
            {
                await context.Publish(new SendRequestCreateStudentThirdPartyUIdFailed
                {
                    UId = context.Message.UId,
                    FirstName = context.Message.FirstName,
                    LastName = context.Message.LastName,
                    BirthDate = context.Message.BirthDate,
                    Email = context.Message.Email,

                    ExceptionMessage = e.Message,
                    ExceptionStackTrace = e.StackTrace,
                    ExceptionType = e.GetType().Name,

                    CorrelationId = context.Message.CorrelationId
                });
            }
        }
    }
}
