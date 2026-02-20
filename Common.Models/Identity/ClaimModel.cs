using System;

namespace Common.Models.Identity;

public class ClaimModel
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}