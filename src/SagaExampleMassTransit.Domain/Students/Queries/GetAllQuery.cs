using MediatR;
using SagaExampleMassTransit.Domain.Entities;
using SagaExampleMassTransit.Domain.Students.Repositories;

namespace SagaExampleMassTransit.Domain.Students.Queries
{
    public class GetAllQuery : IRequest<IEnumerable<Student>>
    {
    }

    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, IEnumerable<Student>>
    {
        private readonly IStudentReadOnlyRepository _studentReadOnlyRepository;
        public GetAllQueryHandler(IStudentReadOnlyRepository studentReadOnlyRepository)
        {
            _studentReadOnlyRepository = studentReadOnlyRepository;
        }
        public async Task<IEnumerable<Student>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            return await _studentReadOnlyRepository.GetAllAsync(cancellationToken);
        }
    }
}
