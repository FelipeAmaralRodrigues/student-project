
namespace StudentProject.Contracts
{
    public record ReceiveResponseCreateStudentThirdPartyUId
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
    }
    public record ResponseCreateStudentThirdPartyUIdNotReceived
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
    }

    public record ResponseCreateStudentThirdPartyUIdReceived
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
        public Guid ThirdPartyUId { get; set; }
    }

    public record ReceiveResponseCreateStudentThirdPartyUIdFailed
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }

        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
    }
}