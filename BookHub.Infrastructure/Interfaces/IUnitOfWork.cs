using BookHub.Core.Entities;

namespace BookHub.Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRepository<Author> Authors { get; }
    IRepository<Book> Books { get; }
    IRepository<User> Users { get; }
    IRepository<UserExternalLoginProvider> UserExternalLoginProviders { get; }
    Task<int> SaveChangesAsync();
    int SaveChanges();
}