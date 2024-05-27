using System.ComponentModel.DataAnnotations;

namespace BookHub.Core.Entities
{
    public class Book
    {
        public Guid Id { get; init; }

        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [MaxLength(1000)]
        public string Description { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public Author Author { get; set; } = null!;
    }
}