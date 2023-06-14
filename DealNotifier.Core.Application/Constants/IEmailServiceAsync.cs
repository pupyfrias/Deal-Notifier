using DealNotifier.Core.Application.DTOs.Email;

namespace DealNotifier.Core.Application.Constants
{
    public interface IEmailServiceAsync
    {
        Task SendAsync(EmailDto emailDTO);
    }
}