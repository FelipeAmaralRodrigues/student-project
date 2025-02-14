using FluentValidation;
using SagaExampleMassTransit.Domain.Students.Queries;

namespace SagaExampleMassTransit.Domain.Students.Validators
{
    public class GetAllQueryValidator : AbstractValidator<GetAllQuery>
    {
    }
}
