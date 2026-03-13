using System;

namespace MediaCore.Models;

public class UserMediaLike
{
    public Guid Id { get; set; }
    public Guid MediaId { get; set; }
    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }
}