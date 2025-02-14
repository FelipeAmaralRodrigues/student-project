using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using SagaExampleMassTransit.Domain.Data;
using SagaExampleMassTransit.Infra.Data.Context;
using SagaExampleMassTransit.Domain.Students.Repositories;

namespace SagaExampleMassTransit.Infra.Data.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private bool _disposed = false;
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction = null;
        public IStudentRepository StudentRepository { get; private set; }
        public UnitOfWork(
            ApplicationDbContext context,
            IStudentRepository studentRepository)
        {
            _context = context;
            StudentRepository = studentRepository;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void RejectChanges()
        {
            foreach (var entry in _context.ChangeTracker.Entries()
                  .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }

        public IDbContextTransaction Begin()
        {
            _transaction = _context.Database.BeginTransaction();
            return _transaction;
        }

        public bool Commit()
        {
            bool r = false;

            try
            {
                _context.SaveChanges();
                _transaction.Commit();
                r = true;
            }
            catch
            {
                _transaction.Rollback();
                r = false;
            }
            finally
            {
                _transaction.Dispose();
            }

            return r;
        }

        public async Task<bool> ExecuteStrategyAsync(IEnumerable<Func<Task>> actions, CancellationToken cancellationToken)
        {
            var executionStrategy = _context.Database.CreateExecutionStrategy();
            bool response = false;
            await executionStrategy.ExecuteAsync(async (cancellationToken) =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        foreach (Func<Task> action in actions)
                        {
                            await action();
                        }

                        _context.SaveChanges();

                        transaction.Commit();
                        response = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }, cancellationToken);

            return response;
        }
    }
}
