using BookHub.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Infrastructure;

/// <summary>
/// Represents a generic repository for performing CRUD operations on entities.
/// This class provides methods to interact with the database using Entity Framework.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationContext _context;
    private readonly DbSet<T> _dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{T}"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public Repository(ApplicationContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <summary>
    /// Gets an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity.</returns>
    /// <exception cref="NullReferenceException">Thrown when the entity is not found.</exception>
    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id) ?? throw new NullReferenceException("Entity is null");
    }

    /// <summary>
    /// Gets all entities asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only collection of entities.</returns>
    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public void UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
    }

    /// <summary>
    /// Removes an existing entity.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    public async void Remove(Guid id)
    {
        var entity = await _dbSet.FindAsync(id) ?? throw new NullReferenceException("Entity not found");
        _dbSet.Remove(entity);    
    }
}