using System.ComponentModel.DataAnnotations;

namespace ApiApp.Common.Dto;

public class CreateUpdatePersonDetailsDto
{
    [Required]
    public int PersonId { get; set; }

    [Required]
    public DateTime BirthDay { get; set; }

    [Required]
    [StringLength(100)]
    public string PersonCity { get; set; }
}