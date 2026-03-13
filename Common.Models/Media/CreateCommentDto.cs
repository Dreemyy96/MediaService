using System;

namespace Common.Models.Media;

public class CreateCommentDto
{
    public Guid UserId { get; set; }
    public Guid MediaId { get; set; }
    public string Text { get; set; }
}