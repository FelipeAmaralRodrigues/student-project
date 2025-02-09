
namespace SagaExampleMassTransit.Contracts
{
    public record ReceiveResponseCreateStudentThirdPartyPlatformCommand
    {
        public Guid RequestUId { get; set; }
    }

    public record ResponseCreateStudentThirdPartyPlatformWaitingEvent
    {
        public Guid RequestUId { get; set; }
    }

    public record ResponseCreateStudentThirdPartyPlatformReceivedEvent
    {
        public Guid RequestUId { get; set; }

        public long Id { get; set; }
        public Guid UId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
    }

    public record ReceiveResponseCreateStudentThirdPartyPlatformFailedEvent
    {
        public Guid RequestUId { get; set; }

        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
    }
}