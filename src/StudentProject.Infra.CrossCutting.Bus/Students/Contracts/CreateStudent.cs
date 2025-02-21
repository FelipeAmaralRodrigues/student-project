
using MassTransit;

namespace StudentProject.Contracts
{
    public record CreateStudent : CorrelatedBy<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }

        public Guid CorrelationId { get; set; }
    }

    public record StudentCreated : CorrelatedBy<Guid>
    {
        public Guid UId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }

        public Guid CorrelationId { get; set; }
}

    public record CreateStudentValidationFailed : CorrelatedBy<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public Dictionary<string, string> ValidationErrors { get; set; }
        public Guid CorrelationId { get; set; }
    }

    public record CreateStudentFailed : CorrelatedBy<Guid>
    {
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
