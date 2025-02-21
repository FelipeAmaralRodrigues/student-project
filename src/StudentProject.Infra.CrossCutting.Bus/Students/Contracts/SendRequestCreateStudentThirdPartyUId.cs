
using MassTransit;

namespace StudentProject.Contracts
{
    public record SendRequestCreateStudentThirdPartyUId : CorrelatedBy<Guid>
    {
        public Guid UId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }

        public Guid CorrelationId { get; set; }
    }

    public record RequestCreateStudentThirdPartyUIdSended : CorrelatedBy<Guid>
    {
        public Guid RequestUId { get; set; }
        public Guid UId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }

        public Guid CorrelationId { get; set; }
    }

    public record SendRequestCreateStudentThirdPartyUIdFailed : CorrelatedBy<Guid>
    {
        public Guid UId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }

        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
