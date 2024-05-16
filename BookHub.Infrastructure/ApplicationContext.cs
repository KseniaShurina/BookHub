using System.Collections.Generic;
using System.Data.Common;
using BookHub.Domain.Entities;
using BookHub.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Infrastructure
{
    /// <summary>
    /// This class connects the application to the database PostgresSQL.
    /// </summary>
    internal class ApplicationContext : DbContext, IApplicationContext
    {
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        //
        public DbConnection GetDbConnection()
        {
            return Database.GetDbConnection();
        }
    }
}