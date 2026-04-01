using System;
using System.Collections.Generic;
using MediaCore.Enums;

namespace Common.Models.ResponseModels;

public class MediaMetaInfoResponse
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public MediaType MediaType { get; set; }
    public MediaStatus Status { get; set; }
    
    public long ViewCount { get; set; }
    public long LikesCount  { get; set; }
    public long CommentsCount { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<string> MediaTags { get; set; }
}