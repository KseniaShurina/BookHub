using BookHub.Core.Entities;
using BookHub.Infrastructure.Interfaces;

namespace BookHub.Infrastructure;

/// <summary>
/// Implements the Unit of Work pattern to manage transactions and repositories.
/// Provides a way to coordinate the work of multiple repositories by committing changes to the database in a single transaction.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _context;
    private IRepository<Author>? _authorRepository;
    private IRepository<Book>? _bookRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UnitOfWork(ApplicationContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets the repository for <see cref="Author"/> entities.
    /// </summary>
    public IRepository<Author> Authors => _authorRepository ??= new Repository<Author>(_context);

    /// <summary>
    /// Gets the repository for <see cref="Book"/> entities.
    /// </summary>
    public IRepository<Book> Books => _bookRepository ??= new Repository<Book>(_context);

    /// <summary>
    /// Saves all changes made in this context to the database asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous save operation.
    /// The task result contains the number of state entries written to the database.</returns>
    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }


    /// <summary>
    /// Disposes the context and releases all resources used by the <see cref="UnitOfWork"/>.
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
        //prevents the finalizer from being called on an object
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Asynchronously disposes the context and releases all resources used by the <see cref="UnitOfWork"/>.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}