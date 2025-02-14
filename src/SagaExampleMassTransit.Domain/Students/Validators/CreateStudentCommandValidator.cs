using FluentValidation;
using SagaExampleMassTransit.Domain.Students.Commands;
using SagaExampleMassTransit.Domain.Students.Repositories;

namespace SagaExampleMassTransit.Domain.Students.Validators
{
    public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentCommandValidator(IStudentReadOnlyRepository studentReadOnlyRepository)
        {
            RuleFor(a => a.FirstName)
                .Must(a => !string.IsNullOrEmpty(a)).WithMessage("First name must not be empty");

            RuleFor(a => a.LastName)
                .Must(a => !string.IsNullOrEmpty(a)).WithMessage("Last name must not be empty");

            RuleFor(a => a.BirthDate)
                .Must(a => a != DateTime.MinValue).WithMessage("Birth date must be a valid date");

            When(a => a.BirthDate != DateTime.MinValue, () =>
            {
                RuleFor(a => a.BirthDate)
                    .Must(a => a < DateTime.UtcNow).WithMessage("Birth date must be in the past");
            });
           
            RuleFor(a => a.Email)
                .Must(a => !string.IsNullOrEmpty(a)).WithMessage("Email must not be empty");

            When(a => !string.IsNullOrEmpty(a.Email), () =>
            {
                RuleFor(a => a.Email)
                    .EmailAddress().WithMessage("Invalid email format");

                RuleFor(a => a)
                    .Must(a =>
                    {
                        var s = studentReadOnlyRepository.GetByEmail(a.Email.Trim().ToLower());
                        return s == null;
                    }).WithMessage("Email is already in use").WithName("Email");
            });
        }
    }
}
