using BookHub.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Infrastructure.Interfaces
{
    public interface IApplicationContext
    {
        DbSet<Book> Books { get; }
        DbSet<Author> Authors { get; }
    }
}