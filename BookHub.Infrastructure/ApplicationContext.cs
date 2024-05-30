using BookHub.Core.Entities;
using BookHub.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Infrastructure
{
    /// <summary>
    /// This class connects the application to the database PostgresSQL.
    /// The DbContext base class creates a database context and provides access to Entity Framework functionality.
    /// </summary>
    public class ApplicationContext : DbContext, IApplicationContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        /// <summary>
        /// Gets or sets the Books DbSet.
        /// This represents the collection of all <see cref="Book"/> entities in the context.
        /// </summary>
        public DbSet<Book> Books { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Authors DbSet.
        /// This represents the collection of all <see cref="Author"/> entities in the context.
        /// </summary>
        public DbSet<Author> Authors { get; set; } = null!;
    }
}