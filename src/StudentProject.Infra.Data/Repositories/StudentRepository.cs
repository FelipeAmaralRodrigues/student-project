using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentProject.Domain.Entities;
using StudentProject.Domain.Students.Repositories;
using StudentProject.Infra.Data.Context;
using StudentProject.Infra.Data.Data;

namespace StudentProject.Infra.Data.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(
            ApplicationDbContext context, 
            ILogger<StudentRepository> logger) : base(context, logger)
        {
        }

        public void Create(Student student)
        {
            DbSet.Add(student);
        }

        public async Task CreateAsync(Student student, CancellationToken cancellationToken)
        {
            await DbSet.AddAsync(student);
        }

        public void UpdateThirdPartyStudentUIdByStudentId(long studentId, Guid thirdPartyStudentUId)
        {
            DbSet.ExecuteUpdate(a => a.SetProperty(a => a.ThirdPartyStudentUId, thirdPartyStudentUId));
        }

        public async Task UpdateThirdPartyStudentUIdByStudentIdAsync(long studentId, Guid thirdPartyStudentUId, CancellationToken cancellationToken)
        {
            await DbSet.ExecuteUpdateAsync(a => a.SetProperty(a => a.ThirdPartyStudentUId, thirdPartyStudentUId), cancellationToken);

        }
    }
}