using ApiApp.Common.Dto;

namespace WebApp.Models;

public class CreateUpdatePersonDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public CreateUpdatePositionDto Position { get; set; }
    public CreateUpdateSalaryDto Salary { get; set; }
    public CreateUpdatePersonDetailsDto PersonDetails { get; set; }
}