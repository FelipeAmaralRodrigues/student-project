using MassTransit;

namespace StudentProject.Contracts
{
    public class StudentCreatedThirdPartyRegistrationSagaData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public Guid? StudentUId { get; set; }
        public Guid? RequestUId { get; set; }
        public DateTime? RequestCreateStudentThirdPartyUIdSendedAt { get; set; }
        public DateTime? ResponseCreateStudentThirdPartyUIdNotReceivedLastAt { get; set; }
        public DateTime? ResponseCreateStudentThirdPartyUIdReceivedAt { get; set; }
        public DateTime? StudentThirdPartyUIdUpdatedAt { get; set; }
    }
}