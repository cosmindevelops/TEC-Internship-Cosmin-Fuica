namespace ApiApp.Common.Dto;

public class PositionDto
{
    public int PositionId { get; set; }
    public string Name { get; set; }
    public int DepartmentId { get; set; }
    public DepartmentDto Department { get; set; }
}