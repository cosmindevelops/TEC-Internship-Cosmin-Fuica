namespace ApiApp.Common.Dto;

public class SalaryWithFullNameDto
{
    public int PersonId { get; set; }
    public string FullName { get; set; }
    public decimal Amount { get; set; }
}