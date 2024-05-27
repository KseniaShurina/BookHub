using BookHub.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace BookHub.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _context;
    public UnitOfWork(ApplicationContext context)
    {
        _context = context;
    }

    public DbConnection GetDbConnection()
    {
        return _context.Database.GetDbConnection();
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }
}