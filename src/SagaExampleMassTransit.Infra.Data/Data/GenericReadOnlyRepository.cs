using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SagaExampleMassTransit.Infra.Data.Context;

namespace SagaExampleMassTransit.Infra.Data.Data
{
    public class GenericReadOnlyRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _applicationDbContext;
        private DbSet<TEntity> DbSet;
        protected readonly ILogger _logger;
        public IQueryable<TEntity> Entities => DbSet;

        public GenericReadOnlyRepository(ApplicationDbContext applicationDbContext, ILogger logger)
        {
            _applicationDbContext = applicationDbContext;
            DbSet = _applicationDbContext.Set<TEntity>();
            _logger = logger;
        }
    }
}