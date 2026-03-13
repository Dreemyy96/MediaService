using System;

namespace MediaCore.Models;

public class UserViewHistory
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MediaId { get; set; }

    public DateTime CreatedAt { get; set; }
}