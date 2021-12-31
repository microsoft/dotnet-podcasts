using System.ComponentModel.DataAnnotations;

namespace Podcast.API.Models;

public class RegisterModel
{
    [Required]
    [StringLength(50)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string Password { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
}

