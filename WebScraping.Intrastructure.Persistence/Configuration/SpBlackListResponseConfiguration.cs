using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Application.Dtos;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class SpBlackListResponseConfiguration : IEntityTypeConfiguration<BlackListDto>
    {
        public void Configure(EntityTypeBuilder<BlackListDto> builder)
        {
            builder.HasNoKey();
        }
    }
}