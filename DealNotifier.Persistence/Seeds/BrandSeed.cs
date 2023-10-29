
using Enums = Catalog.Application.Enums;
using Catalog.Domain.Entities;

namespace Catalog.Persistence.Seeds
{
    public static class BrandSeed
    {
        public static List<Brand> Data { get; } =
            Enum.GetValues<Enums.Brand>()
            .Select(e => new Brand { Id = (int)e, Name = e.ToString() })
            .ToList();
    }
}