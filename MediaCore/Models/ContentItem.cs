using System;
using MediaCore.Enums;

namespace MediaCore.Models;

public class ContentItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public MediaType MediaType { get; set; }
    public MediaStatus Status { get; set; }
    public long Size { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}