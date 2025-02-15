
namespace StudentProject.Contracts
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
        public Guid UId { get; set; }
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