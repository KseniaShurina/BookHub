namespace BookHub.Infrastructure.Interfaces;

/// <summary>
/// Represents a generic repository interface for performing CRUD operations on entities.
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}