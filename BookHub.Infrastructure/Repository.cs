using System.Linq.Expressions;
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
    /// 
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    /// <summary>
    /// Asynchronously retrieves an entity by its unique identifier, optionally including related properties.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="includeProperties">A comma-separated list of related properties to include in the query.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public virtual async Task<T> GetByIdAsync(Guid id, string includeProperties = "")
    {
        //Creating a Basic Query to _dbSet
        IQueryable<T> query = _dbSet;

        //Adding Related Properties to a Query
        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        return await query.SingleOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
    }

    /// <summary>
    /// Asynchronously retrieves a collection of entities from the database with optional filtering, sorting, and inclusion of related properties.
    /// </summary>
    /// <param name="filter">An optional filter expression to apply to the query.</param>
    /// <param name="orderBy">An optional function to sort the query results.</param>
    /// <param name="includeProperties">A comma-separated list of related properties to include in the query.</param>
    /// <returns>A collection of entities that match the specified criteria.</returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "")
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
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
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    /// <summary>
    /// Removes an existing entity.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    public async Task RemoveAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id) ?? throw new NullReferenceException("Entity not found");
        _dbSet.Remove(entity);
    }
}