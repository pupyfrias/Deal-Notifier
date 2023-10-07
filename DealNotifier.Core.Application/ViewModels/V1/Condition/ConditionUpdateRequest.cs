using DealNotifier.Core.Application.Interfaces;

namespace DealNotifier.Core.Application.ViewModels.V1.Condition
{
    public class ConditionUpdateRequest : IHasId<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}