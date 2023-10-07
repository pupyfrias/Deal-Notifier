using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Entities;

namespace WebApi.Controllers.V1
{
    public interface IOnlineStoreServiceAsync: IGenericServiceAsync<OnlineStore, int>
    {
    }
}