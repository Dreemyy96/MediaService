using System;

namespace MediaCore.Models;

public class Comment
{
    public Guid Id { get; set; }
    public Guid MediaId { get; set; }
    public Guid UserId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}