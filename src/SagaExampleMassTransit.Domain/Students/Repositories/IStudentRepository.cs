
using SagaExampleMassTransit.Domain.Entities;

namespace SagaExampleMassTransit.Domain.Students.Repositories
{
    public interface IStudentRepository
    {
        void Create(Student student);
        Task CreateAsync(Student student, CancellationToken cancellationToken);
        void UpdateThirdPartyStudentUIdByStudentId(long studentId, Guid thirdPartyStudentUId);
        Task UpdateThirdPartyStudentUIdByStudentIdAsync(long studentId, Guid thirdPartyStudentUId, CancellationToken cancellationToken);
    }
}
