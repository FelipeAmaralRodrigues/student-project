
namespace SagaExampleMassTransit.Contracts
{
    public record UpdateStudentThirdPartyUId
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
        public Guid ThirdPlatformStudentUId { get; set; }
    }

    public record StudentThirdPartyUIdUpdated
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
        public Guid ThirdPlatformStudentUId { get; set; }
    }

    public record UpdateStudentThirdPartyUIdFailed
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
        public Guid ThirdPlatformStudentUId { get; set; }

        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
    }
}
