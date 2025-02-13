using FluentValidation;
using SagaExampleMassTransit.Domain.Student.Commands;

namespace SagaExampleMassTransit.Domain.Student.Validators
{
    public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentCommandValidator()
        {
            
        }
    }
}
