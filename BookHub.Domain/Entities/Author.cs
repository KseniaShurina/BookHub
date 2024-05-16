using System.ComponentModel.DataAnnotations;

namespace BookHub.Domain.Entities
{
    public class Author
    {
        public Guid Id { get; init; }

        [MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [MaxLength(100)]
        public string LastName { get; set; } = null!;

        [MaxLength(1000)]
        public string Description { get; set; } = null!;
        public List<Book> Books { get; set; } = null!;
    }
}