using MassTransit;

namespace SagaExampleMassTransit.Services.Worker.Sagas
{
    public class StudentCreatedThirdPartyRegistrationSagaData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }

        public Guid StudentUId { get; set; }

        public bool? RequestThirdPartyPlatformSended { get; set; }
        public bool? RequestThirdPartyPlatformWaited { get; set; }
        public bool? RequestThirdPartyPlatformReceived { get; set; }
        public bool? StudentThirdPartyPlatformIdUpdated { get; set; }
    }
}
