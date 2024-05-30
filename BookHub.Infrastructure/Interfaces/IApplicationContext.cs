using BookHub.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents the application context interface.
    /// This interface defines the contract for the application context, providing access to the DbSet properties for entities.
    /// </summary>
    public interface IApplicationContext
    {
        DbSet<Book> Books { get; }
        DbSet<Author> Authors { get; }
    }
}