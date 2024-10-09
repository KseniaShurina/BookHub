using System.ComponentModel.DataAnnotations;

namespace BookHub.Application.Models
{
    public class UserExternalProviderRegisterModel
    {
        [Required]
        public required string Email { get; set; } = null!;
        public required string Scheme { get; init; } = null!;
        public required string ExternalId { get; init; } = null!;

        public Guid UserId { get; init; }
    }
}
