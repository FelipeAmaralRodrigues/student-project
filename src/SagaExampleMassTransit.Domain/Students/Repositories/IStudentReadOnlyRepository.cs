
using SagaExampleMassTransit.Domain.Entities;

namespace SagaExampleMassTransit.Domain.Students.Repositories
{
    public interface IStudentReadOnlyRepository
    {
        IEnumerable<Student> GetAll();
        Task<IEnumerable<Student>> GetAllAsync(CancellationToken cancellationToken);
        Student? GetById(long id);
        Task<Student?> GetByIdAsync(long id, CancellationToken cancellationToken);
        Student? GetByUId(Guid uid);
        Task<Student?> GetByUIdAsync(Guid uid, CancellationToken cancellationToken);
        Student? GetByEmail(string email);
        Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
