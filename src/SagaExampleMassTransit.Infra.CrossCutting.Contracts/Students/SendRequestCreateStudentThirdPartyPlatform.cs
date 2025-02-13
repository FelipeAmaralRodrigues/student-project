
namespace SagaExampleMassTransit.Contracts
{
    public record SendRequestCreateStudentThirdPartyPlatform
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
    }

    public record RequestCreateStudentThirdPartyPlatformSendedEvent
    {
        public Guid RequestUId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
    }

    public record SendRequestCreateStudentThirdPartyPlatformFailedEvent
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }

        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
    }
}
