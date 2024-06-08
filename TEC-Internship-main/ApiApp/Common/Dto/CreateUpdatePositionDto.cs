using System.ComponentModel.DataAnnotations;

namespace ApiApp.Common.Dto;

public class CreateUpdatePositionDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public CreateUpdateDepartmentDto Department { get; set; }
}