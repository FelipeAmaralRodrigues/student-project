
using StudentProject.Domain.Entities;

namespace StudentProject.Domain.Students.Repositories
{
    public interface IStudentRepository
    {
        void Create(Student student);
        Task CreateAsync(Student student, CancellationToken cancellationToken);
        void UpdateThirdPartyUIdById(long id, Guid thirdPartyUId);
        Task UpdateThirdPartyUIdByIdAsync(long id, Guid thirdPartyUId, CancellationToken cancellationToken);
    }
}
