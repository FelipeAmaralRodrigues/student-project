
namespace SagaExampleMassTransit.Contracts
{
    public record UpdateExternalIdLocalStudent
    {
        public Guid RequestUId { get; set; }
        public Guid ThirdPlatformStudentUId { get; set; }
    }

    public record ExternalIdLocalStudentUpdatedEvent
    {
        public Guid RequestUId { get; set; }
        public long StudentId { get; set; }
        public Guid StudentUId { get; set; }
        public Guid ThirdPlatformStudentUId { get; set; }
    }

    public record UpdateExternalIdLocalStudentFailedEvent
    {
        public Guid RequestUId { get; set; }
        public Guid ThirdPlatformStudentUId { get; set; }

        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
    }
}
