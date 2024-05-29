using BookHub.Core.Entities;
using BookHub.Infrastructure.Interfaces;

namespace BookHub.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _context;
    private IRepository<Author>? _authorRepository;
    private IRepository<Book>? _bookRepository;

    public UnitOfWork(ApplicationContext context)
    {
        _context = context;
    }

    public IRepository<Author> Authors => _authorRepository ??= new Repository<Author>(_context);
    public IRepository<Book> Books => _bookRepository ??= new Repository<Book>(_context);

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
        //prevents the finalizer from being called on an object
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}