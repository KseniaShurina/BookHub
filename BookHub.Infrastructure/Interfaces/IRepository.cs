using System.Linq.Expressions;

namespace BookHub.Infrastructure.Interfaces;

/// <summary>
/// Represents a generic repository interface for performing CRUD operations on entities.
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Checks if any entity in the DbSet matches the specified predicate.
    /// </summary>
    /// <param name="predicate">A lambda expression representing the condition to check.</param>
    /// <returns>true if any entity matches the predicate. Otherwise, false.</returns>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Returns the first entity that matches the specified condition.
    /// </summary>
    /// <param name="expression">A lambda expression representing the condition to match.</param>
    /// <returns>The first entity that matches the condition, or null if no entity matches.</returns>
    Task<T?> FindByConditionAsync(Expression<Func<T, bool>> expression);

    /// <summary>
    /// Asynchronously retrieves an entity by its unique identifier, optionally including related properties.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="includeProperties">A comma-separated list of related properties to include in the query.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<T> GetByIdAsync(Guid id, string includeProperties = "");

    /// <summary>
    /// Asynchronously retrieves a collection of entities from the database with optional filtering, sorting, and inclusion of related properties.
    /// </summary>
    /// <param name="filter">An optional filter expression to apply to the query.</param>
    /// <param name="orderBy">An optional function to sort the query results.</param>
    /// <param name="includeProperties">A comma-separated list of related properties to include in the query.</param>
    /// <returns>A collection of entities that match the specified criteria.</returns>
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(T entity);

    /// <summary>
    /// Removes an existing entity.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    Task RemoveAsync(Guid id);
}