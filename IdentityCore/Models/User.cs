using System;
using IdentityCore.Enums;

namespace IdentityCore.Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    /// <summary>
    /// User password hash
    /// </summary>
    public string Password { get; set; }
    
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}