using System.Data.Common;

namespace BookHub.Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    DbConnection GetDbConnection();
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}