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

        public void UpdateThirdPartyUIdById(long studentId, Guid thirdPartyUId)
        {
            DbSet.ExecuteUpdate(a => a.SetProperty(a => a.ThirdPartyUId, thirdPartyUId));
        }

        public async Task UpdateThirdPartyUIdByIdAsync(long studentId, Guid thirdPartyUId, CancellationToken cancellationToken)
        {
            await DbSet.Where(a => a.Id == studentId).ExecuteUpdateAsync(a => a.SetProperty(a => a.ThirdPartyUId, thirdPartyUId), cancellationToken);

        }
    }
}