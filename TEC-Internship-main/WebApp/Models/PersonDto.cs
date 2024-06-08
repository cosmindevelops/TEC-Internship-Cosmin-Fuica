using System;

namespace WebApp.Models;

public class PersonDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string? PositionName { get; set; }
    public string? DepartmentName { get; set; }
    public int? SalaryAmount { get; set; }
    public DateTime? BirthDay { get; set; }
    public string? PersonCity { get; set; }
}