using System.ComponentModel.DataAnnotations;

namespace ApiApp.Common.Dto;

public class CreateUpdatePersonDto
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Surname { get; set; }

    [Required]
    [Range(1, 120)]
    public int Age { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100)]
    public string Address { get; set; }

    public CreateUpdatePersonDetailsDto PersonDetails { get; set; }

    public CreateUpdatePositionDto Position { get; set; }

    public CreateUpdateSalaryDto Salary { get; set; }
}