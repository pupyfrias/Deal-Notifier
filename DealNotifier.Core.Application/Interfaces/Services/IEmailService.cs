using Catalog.Application.ViewModels.V1.Email;

namespace Catalog.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto emailDto);
    }
}