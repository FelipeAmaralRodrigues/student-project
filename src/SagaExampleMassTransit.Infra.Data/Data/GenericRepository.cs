using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SagaExampleMassTransit.Domain.Data;
using SagaExampleMassTransit.Infra.Data.Context;

namespace SagaExampleMassTransit.Infra.Data.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        protected DbSet<TEntity> DbSet;
        protected readonly ILogger _logger;
        public IQueryable<TEntity> Entities => DbSet;

        public GenericRepository(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            DbSet = _context.Set<TEntity>();
            _logger = logger;
        }
    }
}