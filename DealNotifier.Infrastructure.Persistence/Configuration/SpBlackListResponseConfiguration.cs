using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DealNotifier.Core.Application.DTOs;

namespace DealNotifier.Infrastructure.Persistence.Configuration
{
    public class SpBlackListResponseConfiguration : IEntityTypeConfiguration<BlackListDto>
    {
        public void Configure(EntityTypeBuilder<BlackListDto> builder)
        {
            builder.HasNoKey();
        }
    }
}