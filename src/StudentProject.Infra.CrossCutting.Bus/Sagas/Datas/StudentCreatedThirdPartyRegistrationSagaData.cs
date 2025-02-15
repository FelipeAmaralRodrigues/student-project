using MassTransit;

namespace StudentProject.Infra.CrossCutting.Bus.Sagas.Datas
{
    public class StudentCreatedThirdPartyRegistrationSagaData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public Guid StudentUId { get; set; }
        public DateTime? RequestCreateStudentThirdPartyUIdSendedAt { get; set; }
        public DateTime? ResponseCreateStudentThirdPartyUIdWaitedLastAt { get; set; }
        public DateTime? ResponseCreateStudentThirdPartyUIdReceivedAt { get; set; }
        public DateTime? StudentThirdPartyUIdUpdatedAt { get; set; }
    }
}