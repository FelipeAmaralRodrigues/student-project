using MassTransit;
using StudentProject.Contracts;

namespace StudentProject.Services.Worker.Sagas
{
    public class StudentCreatedThirdPartyRegistrationSaga 
        : MassTransitStateMachine<StudentCreatedThirdPartyRegistrationSagaData>
    {
        public State RequestingThirdPartyPlatform {  get; set; }
        public State ReceivingResponseThirdPartyPlatform {  get; set; }
        public State WaitingResponseThirdPartyPlatform {  get; set; }
        public State UpdatingStudentThirdPartyUId {  get; set; }

        public Event<StudentCreated> StudentCreated { get; set; }
        public Event<RequestCreateStudentThirdPartyPlatformSended> RequestCreateStudentThirdPartyPlatformSended { get; set; }
        public Event<ResponseCreateStudentThirdPartyPlatformWaited> ResponseCreateStudentThirdPartyPlatformWaited { get; set; }
        public Event<ResponseCreateStudentThirdPartyPlatformReceived> ResponseCreateStudentThirdPartyPlatformReceived { get; set; }
        public Event<StudentThirdPartyUIdUpdated> StudentThirdPartyUIdUpdated { get; set; }

        public StudentCreatedThirdPartyRegistrationSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => StudentCreated, e => e.CorrelateById(m => m.Message.UId));
            Event(() => RequestCreateStudentThirdPartyPlatformSended, e => e.CorrelateById(m => m.Message.StudentUId));
            Event(() => ResponseCreateStudentThirdPartyPlatformWaited, e => e.CorrelateById(m => m.Message.StudentUId));
            Event(() => ResponseCreateStudentThirdPartyPlatformReceived, e => e.CorrelateById(m => m.Message.StudentUId));
            Event(() => StudentThirdPartyUIdUpdated, e => e.CorrelateById(m => m.Message.StudentUId));

            Initially(
                When(StudentCreated)
                    .Then(context =>
                    {
                        context.Saga.StudentUId = context.Message.UId;
                    })
                    .TransitionTo(RequestingThirdPartyPlatform)
                    .Send(context => new SendRequestCreateStudentThirdPartyPlatform
                    {
                        UId = context.Message.UId,
                        FirstName = context.Message.FirstName,
                        LastName = context.Message.LastName,
                        BirthDate = context.Message.BirthDate,
                        Email = context.Message.Email
                    }));

            During(RequestingThirdPartyPlatform,
                When(RequestCreateStudentThirdPartyPlatformSended)
                    .Then(context => context.Saga.RequestThirdPartyPlatformSended = true)
                    .TransitionTo(ReceivingResponseThirdPartyPlatform)
                    .Send(context => new ReceiveResponseCreateStudentThirdPartyPlatform
                    {
                       RequestUId = context.Message.RequestUId,
                       StudentUId = context.Message.StudentUId
                    }));

            During(ReceivingResponseThirdPartyPlatform,
                When(ResponseCreateStudentThirdPartyPlatformWaited)
                    .Then(context => context.Saga.RequestThirdPartyPlatformWaited = true)
                    .TransitionTo(ReceivingResponseThirdPartyPlatform)
                    .Send(context => new ReceiveResponseCreateStudentThirdPartyPlatform
                    {
                        RequestUId = context.Message.RequestUId,
                        StudentUId = context.Message.StudentUId
                    }),
                When(ResponseCreateStudentThirdPartyPlatformReceived)
                    .Then(context =>
                    {
                        context.Saga.RequestThirdPartyPlatformWaited = false;
                        context.Saga.RequestThirdPartyPlatformReceived = true;
                    })
                    .TransitionTo(UpdatingStudentThirdPartyUId)
                    .Send(context => new UpdateStudentThirdPartyUId
                    {
                        RequestUId = context.Message.RequestUId,
                        StudentUId = context.Message.StudentUId,
                        ThirdPlatformStudentUId = context.Message.UId
                    }));

            During(UpdatingStudentThirdPartyUId,
                When(StudentThirdPartyUIdUpdated)
                    .Then(context => context.Saga.StudentThirdPartyPlatformIdUpdated = true)
                    .Finalize());

        }
    }
}
