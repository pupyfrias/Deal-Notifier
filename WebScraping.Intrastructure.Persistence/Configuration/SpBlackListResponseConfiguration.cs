using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Application.Models;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class SpBlackListResponseConfiguration : IEntityTypeConfiguration<SpBlackListResponse>
    {
        public void Configure(EntityTypeBuilder<SpBlackListResponse> builder)
        {
            builder.HasNoKey();
        }
    }
}
