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

            Event(() => StudentCreated, e => e.CorrelateById(m => m.Message.UId));
            Event(() => RequestCreateStudentThirdPartyUIdSended, e => e.CorrelateById(m => m.Message.UId));
            Event(() => ResponseCreateStudentThirdPartyUIdNotReceived, e => e.CorrelateById(m => m.Message.StudentUId));
            Event(() => ResponseCreateStudentThirdPartyUIdReceived, e => e.CorrelateById(m => m.Message.StudentUId));
            Event(() => StudentThirdPartyUIdUpdated, e => e.CorrelateById(m => m.Message.StudentUId));

            Initially(
                When(StudentCreated)
                    .TransitionTo(RequestingCreateStudentThirdPartyUId)
                    .Publish(context => new SendRequestCreateStudentThirdPartyUId
                    {
                        UId = context.Message.UId,
                        FirstName = context.Message.FirstName,
                        LastName = context.Message.LastName,
                        BirthDate = context.Message.BirthDate,
                        Email = context.Message.Email
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
                       StudentUId = context.Message.UId
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
                        StudentUId = context.Message.StudentUId
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
                        ThirdPartyUId = context.Message.ThirdPartyUId
                    }));

            During(UpdatingStudentThirdPartyUId,
                When(StudentThirdPartyUIdUpdated)
                    .Then(context => context.Saga.StudentThirdPartyUIdUpdatedAt = DateTime.UtcNow)
                    .Finalize());

        }
    }
}
