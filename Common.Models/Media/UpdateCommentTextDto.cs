using System;

namespace Common.Models.Media;

public class UpdateCommentTextDto
{
    public Guid CommentId { get; set; }
    public Guid UserId { get; set; }
    public string Text { get; set; }
}