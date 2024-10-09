using BookHub.Application.Models;

namespace BookHub.Application.Interfaces
{
    public interface IAccountService
    {
        Task RegisterUserByExternalProviderAsync(string email, string scheme, string externalId);
        Task<bool> IsExistsInExternalLoginProviderUserAsync(string email);
        Task<bool> IsExistsUserAsync(string email);
        Task<string> GenerateTokenAsync(string email);
        Task<bool> CheckPassword(string email, string password);
        Task RegisterUserAsync(UserRegisterModel model);
    }
}
