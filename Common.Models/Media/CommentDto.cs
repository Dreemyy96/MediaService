using System;

namespace Common.Models.Media;

public class CommentDto
{
    public Guid Id { get; set; }
    public Guid MediaId { get; set; }
    public string UserName { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
}