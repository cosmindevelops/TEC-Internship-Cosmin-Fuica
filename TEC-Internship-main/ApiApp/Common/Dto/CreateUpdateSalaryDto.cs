using System.ComponentModel.DataAnnotations;

namespace ApiApp.Common.Dto;

public class CreateUpdateSalaryDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int Amount { get; set; }
}