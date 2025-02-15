using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentProject.Domain.Entities;
using StudentProject.Domain.Students.Repositories;
using StudentProject.Infra.Data.Context;

namespace StudentProject.Infra.Data.Repositories
{
    public class StudentReadOnlyRepository
        : GenericReadOnlyRepository<Student>,
        IStudentReadOnlyRepository
    {
        public StudentReadOnlyRepository(
            ApplicationDbContext applicationDbContext, 
            ILogger<StudentReadOnlyRepository> logger) : base(applicationDbContext, logger)
        {
        }

        public IEnumerable<Student> GetAll()
        {
            return (from l in Entities
                    select l).ToList();
        }

        public async Task<IEnumerable<Student>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await
                   (from l in Entities
                    select l).ToListAsync(cancellationToken);
        }

        public Student? GetByEmail(string email)
        {
            return (from l in Entities
                    where l.Email == email
                    select l).FirstOrDefault();
        }

        public async Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await
                    (from l in Entities
                     where l.Email == email
                     select l).FirstOrDefaultAsync(cancellationToken);
        }

        public Student? GetById(long id)
        {
            return (from l in Entities
                    where l.Id == id
                    select l).FirstOrDefault();
        }

        public async Task<Student?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await
                   (from l in Entities
                    where l.Id == id
                    select l).FirstOrDefaultAsync(cancellationToken);
        }

        public Student? GetByUId(Guid uid)
        {
            return (from l in Entities
                    where l.UId == uid
                    select l).FirstOrDefault();
        }

        public async Task<Student?> GetByUIdAsync(Guid uid, CancellationToken cancellationToken)
        {
            return await
                   (from l in Entities
                    where l.UId == uid
                    select l).FirstOrDefaultAsync(cancellationToken);
        }
    }
}