using MassTransit;
using Microsoft.Extensions.Logging;

namespace StudentProject.Contracts
{
    public class StudentCreatedThirdPartyRegistrationSaga
        : MassTransitStateMachine<StudentCreatedThirdPartyRegistrationSagaData>
    {
        public State RequestingCreateStudentThirdPartyUId { get; set; }
        public State ReceivingResponseCreateStudentThirdPartyUId { get; set; }
        public State UpdatingStudentThirdPartyUId { get; set; }

        public Event<StudentCreated> StudentCreated { get; set; }
        public Event<RequestCreateStudentThirdPartyUIdSended> RequestCreateStudentThirdPartyUIdSended { get; set; }
        public Event<ResponseCreateStudentThirdPartyUIdNotReceived> ResponseCreateStudentThirdPartyUIdNotReceived { get; set; }
        public Event<ResponseCreateStudentThirdPartyUIdReceived> ResponseCreateStudentThirdPartyUIdReceived { get; set; }
        public Event<StudentThirdPartyUIdUpdated> StudentThirdPartyUIdUpdated { get; set; }
        public Schedule<StudentCreatedThirdPartyRegistrationSagaData, ReceiveResponseCreateStudentThirdPartyUId> ReceiveResponseCreateStudentThirdPartyUIdSchedule { get; set; }

        public StudentCreatedThirdPartyRegistrationSaga(ILogger<StudentCreatedThirdPartyRegistrationSaga> logger)
        {
            InstanceState(x => x.CurrentState);

            Event(() => StudentCreated, e => e.CorrelateById(m => m.Message.CorrelationId));
            Event(() => RequestCreateStudentThirdPartyUIdSended, e => e.CorrelateById(m => m.Message.CorrelationId));
            Event(() => ResponseCreateStudentThirdPartyUIdNotReceived, e => e.CorrelateById(m => m.Message.CorrelationId));
            Event(() => ResponseCreateStudentThirdPartyUIdReceived, e => e.CorrelateById(m => m.Message.CorrelationId));
            Event(() => StudentThirdPartyUIdUpdated, e => e.CorrelateById(m => m.Message.CorrelationId));
            Schedule(() => ReceiveResponseCreateStudentThirdPartyUIdSchedule, x => x.ResponseCreateStudentThirdPartyUIdNotReceivedScheduleTokenId, x =>
            {
                x.Delay = TimeSpan.FromSeconds(5);
                x.Received = e =>
                {
                    e.ConfigureConsumeTopology = false;
                    e.CorrelateById(m => m.Message.CorrelationId);
                };
            });

            Initially(
                When(StudentCreated)
                    .Then(a =>
                    {
                        a.Saga.StudentUId = a.Message.UId;
                        a.Saga.SagaInitAt = DateTime.UtcNow;

                        logger.LogInformation("Saga initialized for student with UId: {UId}", a.Message.UId);
                    })
                    .TransitionTo(RequestingCreateStudentThirdPartyUId)
                    .Publish(context => new SendRequestCreateStudentThirdPartyUId
                    {
                        UId = context.Message.UId,
                        FirstName = context.Message.FirstName,
                        LastName = context.Message.LastName,
                        BirthDate = context.Message.BirthDate,
                        Email = context.Message.Email,

                        CorrelationId = context.Message.CorrelationId
                    }));

            During(RequestingCreateStudentThirdPartyUId,
                When(RequestCreateStudentThirdPartyUIdSended)
                    .Then(context =>
                    {
                        context.Saga.RequestCreateStudentThirdPartyUIdSendedAt = DateTime.UtcNow;
                        context.Saga.RequestUId = context.Message.RequestUId;

                        logger.LogInformation("Request to create student third-party UId sent for student with UId: {UId}", context.Message.UId);
                    })
                    .TransitionTo(ReceivingResponseCreateStudentThirdPartyUId)
                    .Publish(context => new ReceiveResponseCreateStudentThirdPartyUId
                    {
                        RequestUId = context.Message.RequestUId,
                        StudentUId = context.Message.UId,

                        CorrelationId = context.Message.CorrelationId
                    })
            );

            During(ReceivingResponseCreateStudentThirdPartyUId,
                When(ResponseCreateStudentThirdPartyUIdNotReceived)
                    .Then(context =>
                    {
                        context.Saga.ResponseCreateStudentThirdPartyUIdNotReceivedLastAt = DateTime.UtcNow;
                        if (context.Saga.ResponseCreateStudentThirdPartyUIdNotReceivedRetryCount == null)
                            context.Saga.ResponseCreateStudentThirdPartyUIdNotReceivedRetryCount = 1;
                        else
                            context.Saga.ResponseCreateStudentThirdPartyUIdNotReceivedRetryCount++;

                        logger.LogInformation("Response from third-party platform not received for student with UId: {UId}. Attempt: {Attempt}",
                            context.Message.StudentUId, context.Saga.ResponseCreateStudentThirdPartyUIdNotReceivedRetryCount);
                    })
                    .IfElse(context => context.Saga.ResponseCreateStudentThirdPartyUIdNotReceivedRetryCount < 5,
                        thenBinder => thenBinder
                        .TransitionTo(ReceivingResponseCreateStudentThirdPartyUId)
                        .Schedule(
                            ReceiveResponseCreateStudentThirdPartyUIdSchedule,
                            context => new ReceiveResponseCreateStudentThirdPartyUId
                            {
                                RequestUId = context.Message.RequestUId,
                                StudentUId = context.Message.StudentUId,
                                CorrelationId = context.Message.CorrelationId
                            }
                        ),
                        elseBinder => elseBinder
                        .Then(a =>
                        {
                            logger.LogError("Failed to receive response from third-party platform for student with UId: {UId} after {RetryCount} attempts.",
                                a.Message.StudentUId, a.Saga.ResponseCreateStudentThirdPartyUIdNotReceivedRetryCount);
                        }).Finalize()
                    ),

                When(ReceiveResponseCreateStudentThirdPartyUIdSchedule.Received)
                    .Then(a =>
                    {
                        logger.LogInformation("Scheduled response received for student with UId: {UId}", a.Message.StudentUId);
                    })
                    .TransitionTo(ReceivingResponseCreateStudentThirdPartyUId)
                    .Publish(a => a.Message),

                When(ResponseCreateStudentThirdPartyUIdReceived)
                    .Then(context =>
                    {
                        context.Saga.ResponseCreateStudentThirdPartyUIdReceivedAt = DateTime.UtcNow;
                        logger.LogInformation("Response from third-party platform received for student with UId: {UId}", context.Message.StudentUId);
                    })
                    .TransitionTo(UpdatingStudentThirdPartyUId)
                    .Publish(context => new UpdateStudentThirdPartyUId
                    {
                        RequestUId = context.Message.RequestUId,
                        StudentUId = context.Message.StudentUId,
                        ThirdPartyUId = context.Message.ThirdPartyUId,

                        CorrelationId = context.Message.CorrelationId
                    }));

            During(UpdatingStudentThirdPartyUId,
                When(StudentThirdPartyUIdUpdated)
                    .Then(context =>
                    {
                        context.Saga.StudentThirdPartyUIdUpdatedAt = DateTime.UtcNow;
                        logger.LogInformation("Student third-party UId updated for student with UId: {UId}", context.Message.StudentUId);
                    })
                    .Finalize());
        }
    }
}
