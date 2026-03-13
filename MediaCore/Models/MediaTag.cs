using System;

namespace MediaCore.Models;

public class MediaTag
{
    public Guid Id { get; set; }
    public Guid TagId { get; set; }
    public Tag Tag { get; set; }
    public Guid MediaId { get; set; }
    public ContentItem Media { get; set; }
}