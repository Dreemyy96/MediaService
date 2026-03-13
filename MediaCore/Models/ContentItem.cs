using System;
using System.Collections.Generic;
using MediaCore.Enums;

namespace MediaCore.Models;

public class ContentItem
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public MediaType MediaType { get; set; }
    public MediaStatus Status { get; set; }
    public long Size { get; set; }
    public long ViewCount { get; set; }
    public long LikesCount  { get; set; }
    public long CommentsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<MediaTag> MediaTags { get; set; }
}