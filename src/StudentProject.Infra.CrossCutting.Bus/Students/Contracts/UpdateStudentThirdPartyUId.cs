
namespace StudentProject.Contracts
{
    public record UpdateStudentThirdPartyUId
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
        public Guid ThirdPartyUId { get; set; }
    }

    public record StudentThirdPartyUIdUpdated
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
        public Guid ThirdPartyUId { get; set; }
    }
    public record UpdateStudentThirdPartyUIdValidationFailed
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
        public Guid ThirdPartyUId { get; set; }
        public Dictionary<string, string> ValidationErrors { get; set; }
    }

    public record UpdateStudentThirdPartyUIdFailed
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
        public Guid ThirdPartyUId { get; set; }

        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
    }
}
