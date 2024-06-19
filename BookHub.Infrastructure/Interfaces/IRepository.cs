using System.Linq.Expressions;

namespace BookHub.Infrastructure.Interfaces;

/// <summary>
/// Represents a generic repository interface for performing CRUD operations on entities.
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id, string includeProperties = "");

    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, 
            IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

    Task AddAsync(T entity);
    void UpdateAsync(T entity);
    void Remove(Guid id);
}