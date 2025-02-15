using Microsoft.EntityFrameworkCore.Storage;
using StudentProject.Domain.Students.Repositories;

namespace StudentProject.Domain.Data
{
    public interface IUnitOfWork
    {
        IStudentRepository StudentRepository { get; }
        IDbContextTransaction Begin();
        bool Commit();
        void Save();
        Task SaveAsync(CancellationToken cancellationToken);
        void RejectChanges();
        Task<bool> ExecuteStrategyAsync(IEnumerable<Func<Task>> actions, CancellationToken cancellationToken);
    }
}
