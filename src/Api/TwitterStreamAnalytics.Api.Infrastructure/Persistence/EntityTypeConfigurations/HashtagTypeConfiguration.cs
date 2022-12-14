using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TwitterStreamAnalytics.SharedKernel.Domain.Aggregates;

namespace TwitterStreamAnalytics.Api.Infrastructure.Persistence.EntityTypeConfigurations;

internal class HashtagTypeConfiguration : IEntityTypeConfiguration<Hashtag>
{
    public void Configure(EntityTypeBuilder<Hashtag> builder)
    {
        builder.HasKey(nameof(Hashtag.Tag));
    }
}