using BookHub.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Infrastructure;

public class Repository<T> : IRepository<T> where T : class
{
    private ApplicationContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id) ?? throw new NullReferenceException("Entity is null");
    }

    public Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
    public Task AddAsync(T entity)
    {
        throw new NotImplementedException();
    }
    public void Update(T entity)
    {
        throw new NotImplementedException();
    }
    public void Remove(T entity)
    {
        throw new NotImplementedException();
    }
}