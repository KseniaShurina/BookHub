using BookHub.Core.Entities;

namespace BookHub.Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRepository<Author> Authors { get; }
    IRepository<Book> Books { get; }
    Task<int> SaveChangesAsync();
    int SaveChanges();

    //Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}