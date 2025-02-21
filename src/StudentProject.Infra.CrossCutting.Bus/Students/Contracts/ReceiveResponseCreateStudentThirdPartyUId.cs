
using MassTransit;

namespace StudentProject.Contracts
{
    public record ReceiveResponseCreateStudentThirdPartyUId : CorrelatedBy<Guid>
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }

        public Guid CorrelationId { get; set; }
    }
    public record ResponseCreateStudentThirdPartyUIdNotReceived : CorrelatedBy<Guid>
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }

        public Guid CorrelationId { get; set; }
    }

    public record ResponseCreateStudentThirdPartyUIdReceived : CorrelatedBy<Guid>
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }
        public Guid ThirdPartyUId { get; set; }

        public Guid CorrelationId { get; set; }
    }

    public record ReceiveResponseCreateStudentThirdPartyUIdFailed : CorrelatedBy<Guid>
    {
        public Guid RequestUId { get; set; }
        public Guid StudentUId { get; set; }

        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }

        public Guid CorrelationId { get; set; }
    }
}