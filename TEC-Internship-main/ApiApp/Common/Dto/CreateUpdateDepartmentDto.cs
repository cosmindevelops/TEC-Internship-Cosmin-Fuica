using System.ComponentModel.DataAnnotations;

namespace ApiApp.Common.Dto;

public class CreateUpdateDepartmentDto
{
    [Required]
    [StringLength(50)]
    public string DepartmentName { get; set; }
}