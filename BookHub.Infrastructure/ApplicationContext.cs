using System.Data.Common;
using BookHub.Domain.Entities;
using BookHub.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Infrastructure
{
    /// <summary>
    /// This class connects the application to the database PostgresSQL.
    /// The DbContext base class creates a database context and provides access to Entity Framework functionality.
    /// </summary>
    internal class ApplicationContext : DbContext, IApplicationContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
    }
}