using DealNotifier.Core.Application.ViewModels.V1.BanKeyword;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IBanKeywordService : IAsyncService<BanKeyword>
    {
        Task<BanKeywordResponse> BanAndRemoveItemsByKeywordAsync(BanKeywordCreateRequest request);
    }
}