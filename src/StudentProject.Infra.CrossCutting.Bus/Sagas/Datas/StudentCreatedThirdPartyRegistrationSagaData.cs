using MassTransit;
using MassTransit.Middleware;

namespace StudentProject.Contracts
{
    public class StudentCreatedThirdPartyRegistrationSagaData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public Guid? StudentUId { get; set; }
        public Guid? RequestUId { get; set; }
        public DateTime? SagaInitAt { get; set; }
        public DateTime? RequestCreateStudentThirdPartyUIdSendedAt { get; set; }
        public DateTime? ResponseCreateStudentThirdPartyUIdNotReceivedLastAt { get; set; }
        public Guid? ResponseCreateStudentThirdPartyUIdNotReceivedScheduleTokenId { get; set; }
        public int? ResponseCreateStudentThirdPartyUIdNotReceivedRetryCount { get; set; }
        public DateTime? ResponseCreateStudentThirdPartyUIdReceivedAt { get; set; }
        public DateTime? StudentThirdPartyUIdUpdatedAt { get; set; }
    }

    public class StudentCreatedThirdPartyRegistrationSagaDataDefinition : SagaDefinition<StudentCreatedThirdPartyRegistrationSagaData>
    {
        private const int ConcurrencyLimit = 20;
        private const int RateLimit = 5000;

        public StudentCreatedThirdPartyRegistrationSagaDataDefinition()
        {
            Endpoint(e =>
            {
                e.Name = $"student_created_third_party_registration_saga";
                e.PrefetchCount = ConcurrencyLimit;
            });
        }

        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<StudentCreatedThirdPartyRegistrationSagaData> sagaConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseConcurrencyLimit(ConcurrencyLimit);
            endpointConfigurator.UseRateLimit(RateLimit, TimeSpan.FromMinutes(1));
            endpointConfigurator.UseKillSwitch(options => options
                .SetActivationThreshold(10)
                .SetTripThreshold(0.15)
                .SetRestartTimeout(m: 1));
        }
    }
}