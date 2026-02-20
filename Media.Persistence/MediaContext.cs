using MediaCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Media.Persistence;

public class MediaContext : DbContext
{
    public MediaContext(DbContextOptions<MediaContext> options) : base(options)
    {
    }

    public DbSet<ContentItem> Medias { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Tag> Tags { get; set; }
}