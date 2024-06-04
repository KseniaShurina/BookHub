using System.ComponentModel.DataAnnotations;

namespace BookHub.Application.Models
{
    public class CreateBookModel
    {
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [MaxLength(1000)]
        public string Description { get; set; } = null!;
        public Guid AuthorId { get; set; }
    }
}
