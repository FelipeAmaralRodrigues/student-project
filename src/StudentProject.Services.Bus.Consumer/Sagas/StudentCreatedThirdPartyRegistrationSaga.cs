using MassTransit;

namespace StudentProject.Contracts
{
    public class StudentCreatedThirdPartyRegistrationSaga 
        : MassTransitStateMachine<StudentCreatedThirdPartyRegistrationSagaData>
    {
        public State RequestingCreateStudentThirdPartyUId {  get; set; }
        public State ReceivingResponseCreateStudentThirdPartyUId {  get; set; }
        public State UpdatingStudentThirdPartyUId {  get; set; }

        public Event<StudentCreated> StudentCreated { get; set; }
        public Event<RequestCreateStudentThirdPartyUIdSended> RequestCreateStudentThirdPartyUIdSended { get; set; }
        public Event<ResponseCreateStudentThirdPartyUIdNotReceived> ResponseCreateStudentThirdPartyUIdNotReceived { get; set; }
        public Event<ResponseCreateStudentThirdPartyUIdReceived> ResponseCreateStudentThirdPartyUIdReceived { get; set; }
        public Event<StudentThirdPartyUIdUpdated> StudentThirdPartyUIdUpdated { get; set; }

        public StudentCreatedThirdPartyRegistrationSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => StudentCreated, e => e.CorrelateById(m => m.Message.CorrelationId));
            Event(() => RequestCreateStudentThirdPartyUIdSended, e => e.CorrelateById(m => m.Message.CorrelationId));
            Event(() => ResponseCreateStudentThirdPartyUIdNotReceived, e => e.CorrelateById(m => m.Message.CorrelationId));
            Event(() => ResponseCreateStudentThirdPartyUIdReceived, e => e.CorrelateById(m => m.Message.CorrelationId));
            Event(() => StudentThirdPartyUIdUpdated, e => e.CorrelateById(m => m.Message.CorrelationId));

            Initially(
                When(StudentCreated)
                    .Then(a =>
                    {
                        a.Saga.StudentUId = a.Message.UId;
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
                    })
                    .TransitionTo(ReceivingResponseCreateStudentThirdPartyUId)
                    .Publish(context => new ReceiveResponseCreateStudentThirdPartyUId
                    {
                       RequestUId = context.Message.RequestUId,
                       StudentUId = context.Message.UId,

                       CorrelationId = context.Message.CorrelationId
                    }));

            During(ReceivingResponseCreateStudentThirdPartyUId,
                When(ResponseCreateStudentThirdPartyUIdNotReceived)
                    .Then(context =>
                    {
                        context.Saga.ResponseCreateStudentThirdPartyUIdNotReceivedLastAt = DateTime.UtcNow;                        
                    })
                    .TransitionTo(ReceivingResponseCreateStudentThirdPartyUId)
                    .Publish(context => new ReceiveResponseCreateStudentThirdPartyUId
                    {
                        RequestUId = context.Message.RequestUId,
                        StudentUId = context.Message.StudentUId,

                        CorrelationId = context.Message.CorrelationId
                    }),
                When(ResponseCreateStudentThirdPartyUIdReceived)
                    .Then(context =>
                    {
                        context.Saga.ResponseCreateStudentThirdPartyUIdReceivedAt = DateTime.UtcNow;
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
                    .Then(context => context.Saga.StudentThirdPartyUIdUpdatedAt = DateTime.UtcNow)
                    .Finalize());
        }
    }
}
