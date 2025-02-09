using Microsoft.Extensions.Logging;
using SagaExampleMassTransit.Domain.Entities;
using SagaExampleMassTransit.Domain.Repositories;
using SagaExampleMassTransit.Infra.Data.Context;
using SagaExampleMassTransit.Infra.Data.Data;

namespace SagaExampleMassTransit.Infra.Data.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(
            ApplicationDbContext context, 
            ILogger<StudentRepository> logger) : base(context, logger)
        {
        }
    }
}