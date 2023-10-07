using DealNotifier.Core.Application.ViewModels.V1.Email;

namespace DealNotifier.Core.Application.Constants
{
    public interface IEmailServiceAsync
    {
        Task SendAsync(EmailDto emailDTO);
    }
}