using DealNotifier.Core.Application.Interfaces;

namespace DealNotifier.Core.Application.ViewModels.V1.BanLink
{
    public class BanLinkUpdateRequest: IHasId<int>
    {
        public int Id { get; set; }
        public string Link { get; set; }
    }
}