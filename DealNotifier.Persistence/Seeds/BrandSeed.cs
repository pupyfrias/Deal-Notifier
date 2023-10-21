using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Persistence.Seeds
{
    public static class BrandSeed
    {
        public static List<Brand> Data { get; } = Enum.GetValues<Enums.Brand>()
                                                .Select(e => new Brand { Id = (int)e, Name = e.ToString() })
                                                .ToList();
    }
}