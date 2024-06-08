using System.ComponentModel.DataAnnotations;

namespace ApiApp.Common.Dto;

public class RegisterModelDto
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, ErrorMessage = "Username must be between 3 and 50 characters.", MinimumLength = 3)]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "Password must be between 6 and 100 characters.", MinimumLength = 6)]
    public string Password { get; set; }
}