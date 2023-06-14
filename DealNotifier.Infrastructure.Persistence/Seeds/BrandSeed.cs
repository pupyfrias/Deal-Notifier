using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Infrastructure.Persistence.Seeds
{
    public static class BrandSeed
    {
        public static List<Brand> data { get; set; } = new List<Brand>()
        {
             new Brand
            {
                Id = (int)Enums.Brand.Unknown,
                Name = Enums.Brand.Unknown.ToString()
            },
            new Brand
            {
                Id = (int)Enums.Brand.Samsung,
                Name = Enums.Brand.Samsung.ToString()
            },
            new Brand
            {
                Id = (int)Enums.Brand.Apple,
                Name = Enums.Brand.Apple.ToString()
            },
            new Brand
            {
                Id = (int)Enums.Brand.Motorola,
                Name = Enums.Brand.Motorola.ToString()
            }
             ,
            new Brand
            {
                Id = (int)Enums.Brand.LG,
                Name = Enums.Brand.LG.ToString()
            }
             ,
            new Brand
            {
                Id = (int)Enums.Brand.Huawei,
                Name = Enums.Brand.Huawei.ToString()
            }
             ,
            new Brand
            {
                Id = (int)Enums.Brand.Alcatel,
                Name = Enums.Brand.Alcatel.ToString()
            }
             ,
            new Brand
            {
                Id = (int)Enums.Brand.Xiaomi,
                Name = Enums.Brand.Xiaomi.ToString()
            }
              ,
            new Brand
            {
                Id = (int)Enums.Brand.HTC,
                Name = Enums.Brand.HTC.ToString()
            }
        };
    }
}