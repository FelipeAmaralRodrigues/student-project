using FluentValidation;
using StudentProject.Domain.Students.Queries;

namespace StudentProject.Domain.Students.Validators
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
