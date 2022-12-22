using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebScraping.Core.Application.DTOs;

namespace WebScraping.Infrastructure.Persistence.Configuration
{
    public class SpBlackListResponseConfiguration : IEntityTypeConfiguration<BlackListDTO>
    {
        public void Configure(EntityTypeBuilder<BlackListDTO> builder)
        {
            builder.HasNoKey();
        }
    }
}
