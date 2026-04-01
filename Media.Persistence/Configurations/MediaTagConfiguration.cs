using MediaCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Media.Persistence.Configurations;

public class MediaTagConfiguration : IEntityTypeConfiguration<MediaTag>
{
    public void Configure(EntityTypeBuilder<MediaTag> builder)
    {
        builder.HasKey(mt => new { mt.MediaId, mt.TagId });

        builder.HasOne(mt => mt.Media)
            .WithMany(m => m.MediaTags)
            .HasForeignKey(mt => mt.MediaId);
        
        builder.HasOne(mt => mt.Tag)
            .WithMany(t => t.MediaTags)
            .HasForeignKey(mt => mt.TagId);
    }
}