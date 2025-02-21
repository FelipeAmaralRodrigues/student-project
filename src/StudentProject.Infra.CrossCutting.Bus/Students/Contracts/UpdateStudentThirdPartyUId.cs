
using MassTransit;

namespace StudentProject.Contracts
{
    public record UpdateStudentThirdPartyUId : CorrelatedBy<Guid>
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
        public Guid ThirdPartyUId { get; set; }

        public Guid CorrelationId { get; set; }
    }

    public record StudentThirdPartyUIdUpdated : CorrelatedBy<Guid>
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
        public Guid ThirdPartyUId { get; set; }

        public Guid CorrelationId { get; set; }
    }
    public record UpdateStudentThirdPartyUIdValidationFailed : CorrelatedBy<Guid>
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
        public Guid ThirdPartyUId { get; set; }
        public Dictionary<string, string> ValidationErrors { get; set; }

        public Guid CorrelationId { get; set; }
    }

    public record UpdateStudentThirdPartyUIdFailed : CorrelatedBy<Guid>
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
        public Guid ThirdPartyUId { get; set; }

        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
