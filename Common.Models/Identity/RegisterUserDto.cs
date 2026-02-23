using System.ComponentModel.DataAnnotations;

namespace Common.Models.Identity;

public class RegisterUserDto
{
    [Required] public string Name { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords mismatch")]
    public string RepeatPassword { get; init; }
}