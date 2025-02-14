
namespace SagaExampleMassTransit.Contracts
{
    public record ReceiveResponseCreateStudentThirdPartyPlatform
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
    }

    public record ResponseCreateStudentThirdPartyPlatformWaited
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
    }

    public record ResponseCreateStudentThirdPartyPlatformReceived
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }

        public long Id { get; set; }
        public Guid UId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
    }

    public record ReceiveResponseCreateStudentThirdPartyPlatformFailed
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }

        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
    }
}