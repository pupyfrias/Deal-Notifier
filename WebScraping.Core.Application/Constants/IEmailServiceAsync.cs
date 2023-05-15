using WebScraping.Core.Application.DTOs.Email;

namespace WebScraping.Core.Application.Constants
{
    public interface IEmailServiceAsync
    {
        Task SendAsync(EmailDto emailDTO);
    }
}