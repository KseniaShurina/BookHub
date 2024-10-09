namespace BookHub.Core.Entities
{
    public class UserExternalLoginProvider
    {
        public Guid Id { get; init; }
        public string Scheme { get; init; } = null!;
        public string ExternalId { get; init; } = null!;

        public Guid UserId { get; init; }
        public User User { get; init; } = null!;
    }
}