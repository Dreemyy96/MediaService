using System;
using System.Collections.Generic;
using MediaCore.Enums;

namespace Common.Models.Media;

public class FeedItemDto
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public MediaType MediaType { get; set; }
    public string FileUrl { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public long ViewCount { get; set; }
    public long LikesCount  { get; set; }
    public long CommentsCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
    public bool IsSavedByCurrentUser { get; set; }
    public List<string> Tags { get; set; }
    public DateTime CreatedAt { get; set; }
}