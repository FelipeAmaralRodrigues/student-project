using MassTransit;
using Microsoft.Extensions.Logging;
using Serilog.Core;

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

        private readonly ILogger<StudentCreatedThirdPartyRegistrationSaga> _logger;

        public StudentCreatedThirdPartyRegistrationSaga(ILogger<StudentCreatedThirdPartyRegistrationSaga> logger)
        {
            _logger = logger;

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
                HandleStudentCreated());

            During(RequestingCreateStudentThirdPartyUId,
               HandleRequestCreateStudentThirdPartyUIdSended()
            );

            During(ReceivingResponseCreateStudentThirdPartyUId,
                HandleResponseCreateStudentThirdPartyUIdNotReceived(),
                HandleReceiveResponseCreateStudentThirdPartyUIdScheduleReceived(),
                HandleResponseCreateStudentThirdPartyUIdReceived()
            );

            During(UpdatingStudentThirdPartyUId,
                HandleStudentThirdPartyUIdUpdated().Finalize()
            );
        }

        private EventActivityBinder<StudentCreatedThirdPartyRegistrationSagaData, StudentCreated> HandleStudentCreated()
        {
            return When(StudentCreated)
                .Then(a =>
                {
                    a.Saga.StudentUId = a.Message.UId;
                    a.Saga.SagaInitAt = DateTime.UtcNow;
                    _logger.LogInformation("Saga initialized for student with UId: {UId}", a.Message.UId);
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
                });
        }

        private EventActivityBinder<StudentCreatedThirdPartyRegistrationSagaData, RequestCreateStudentThirdPartyUIdSended> HandleRequestCreateStudentThirdPartyUIdSended()
        {
            return When(RequestCreateStudentThirdPartyUIdSended)
                .Then(context =>
                {
                    context.Saga.RequestCreateStudentThirdPartyUIdSendedAt = DateTime.UtcNow;
                    context.Saga.RequestUId = context.Message.RequestUId;

                    _logger.LogInformation("Request to create student third-party UId sent for student with UId: {UId}", context.Message.UId);
                })
                .TransitionTo(ReceivingResponseCreateStudentThirdPartyUId)
                .Publish(context => new ReceiveResponseCreateStudentThirdPartyUId
                {
                    RequestUId = context.Message.RequestUId,
                    StudentUId = context.Message.UId,

                    CorrelationId = context.Message.CorrelationId
                });
        }

        private EventActivityBinder<StudentCreatedThirdPartyRegistrationSagaData, ResponseCreateStudentThirdPartyUIdNotReceived> HandleResponseCreateStudentThirdPartyUIdNotReceived()
        {
            return When(ResponseCreateStudentThirdPartyUIdNotReceived)
                    .Then(context =>
                    {
                        context.Saga.ResponseCreateStudentThirdPartyUIdNotReceivedLastAt = DateTime.UtcNow;
                        if (context.Saga.ResponseCreateStudentThirdPartyUIdNotReceivedRetryCount == null)
                            context.Saga.ResponseCreateStudentThirdPartyUIdNotReceivedRetryCount = 1;
                        else
                            context.Saga.ResponseCreateStudentThirdPartyUIdNotReceivedRetryCount++;

                        _logger.LogInformation("Response from third-party platform not received for student with UId: {UId}. Attempt: {Attempt}",
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
                            _logger.LogError("Failed to receive response from third-party platform for student with UId: {UId} after {RetryCount} attempts.",
                                a.Message.StudentUId, a.Saga.ResponseCreateStudentThirdPartyUIdNotReceivedRetryCount);
                        }).Finalize()
                    );
        }

        private EventActivityBinder<StudentCreatedThirdPartyRegistrationSagaData, ReceiveResponseCreateStudentThirdPartyUId> HandleReceiveResponseCreateStudentThirdPartyUIdScheduleReceived()
        {
            return When(ReceiveResponseCreateStudentThirdPartyUIdSchedule.Received)
                .Then(a =>
                {
                    _logger.LogInformation("Scheduled response received for student with UId: {UId}", a.Message.StudentUId);
                })
                .TransitionTo(ReceivingResponseCreateStudentThirdPartyUId)
                .Publish(a => a.Message);
        }

        private EventActivityBinder<StudentCreatedThirdPartyRegistrationSagaData, ResponseCreateStudentThirdPartyUIdReceived> HandleResponseCreateStudentThirdPartyUIdReceived()
        {
            return When(ResponseCreateStudentThirdPartyUIdReceived)
                .Then(context =>
                {
                    context.Saga.ResponseCreateStudentThirdPartyUIdReceivedAt = DateTime.UtcNow;
                    _logger.LogInformation("Response from third-party platform received for student with UId: {UId}", context.Message.StudentUId);
                })
                .TransitionTo(UpdatingStudentThirdPartyUId)
                .Publish(context => new UpdateStudentThirdPartyUId
                {
                    RequestUId = context.Message.RequestUId,
                    StudentUId = context.Message.StudentUId,
                    ThirdPartyUId = context.Message.ThirdPartyUId,

                    CorrelationId = context.Message.CorrelationId
                });
        }

        private EventActivityBinder<StudentCreatedThirdPartyRegistrationSagaData, StudentThirdPartyUIdUpdated> HandleStudentThirdPartyUIdUpdated()
        {
            return When(StudentThirdPartyUIdUpdated)
                .Then(context =>
                {
                    context.Saga.StudentThirdPartyUIdUpdatedAt = DateTime.UtcNow;
                    _logger.LogInformation("Student third-party UId updated for student with UId: {UId}", context.Message.StudentUId);
                });
        }
    }
}
