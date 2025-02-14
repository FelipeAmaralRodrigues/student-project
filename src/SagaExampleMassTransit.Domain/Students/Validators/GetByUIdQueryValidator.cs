using FluentValidation;
using SagaExampleMassTransit.Domain.Students.Queries;

namespace SagaExampleMassTransit.Domain.Students.Validators
{
    public class GetByUIdQueryValidator : AbstractValidator<GetByUIdQuery>
    {
        public GetByUIdQueryValidator()
        {
            RuleFor(a => a.UId)
                .Must(a => a != Guid.Empty).WithMessage("UId must be a valid GUID");
        }
    }
}
