using Identity.Persistence.Entity;
using Microsoft.AspNetCore.Identity;

namespace Identity.Persistence.Contracts.Repositories
{
    public interface IAuthRepositoryAsync
    {
        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user, string code);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);

        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
    }
}