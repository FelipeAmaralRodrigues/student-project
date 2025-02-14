using MediatR;
using SagaExampleMassTransit.Domain.Entities;
using SagaExampleMassTransit.Domain.Students.Repositories;

namespace SagaExampleMassTransit.Domain.Students.Queries
{
    public class GetByUIdQuery : IRequest<Student?>
    {
        public Guid UId { get; set; }
    }

    public class GetByUIdQueryHandler : IRequestHandler<GetByUIdQuery, Student?>
    {
        private readonly IStudentReadOnlyRepository _studentReadOnlyRepository;
        public GetByUIdQueryHandler(IStudentReadOnlyRepository studentReadOnlyRepository)
        {
            _studentReadOnlyRepository = studentReadOnlyRepository;
        }
        public async Task<Student?> Handle(GetByUIdQuery request, CancellationToken cancellationToken)
        {
            return await _studentReadOnlyRepository.GetByUIdAsync(request.UId, cancellationToken);
        }
    }
}
