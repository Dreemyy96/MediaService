using System.ComponentModel.DataAnnotations;

namespace Common.Models.Identity;

public class AuthModel
{
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }
}