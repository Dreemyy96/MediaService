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
    public DbSet<MediaTag> MediaTags { get; set; }
    public DbSet<UserMediaLike> UserMediaLike { get; set; }
    public DbSet<UserSavedMedia> UserSavedMedia { get; set; }
    public DbSet<UserViewHistory> UserViewHistory { get; set; }
}