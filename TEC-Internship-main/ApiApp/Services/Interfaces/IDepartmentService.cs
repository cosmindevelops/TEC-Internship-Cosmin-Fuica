using ApiApp.Common.Dto;

namespace ApiApp.Services.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();

    Task<DepartmentDto> GetDepartmentByIdAsync(int id);

    Task<bool> DeleteDepartmentAsync(int departmentId);

    Task<DepartmentDto> CreateDepartmentAsync(CreateUpdateDepartmentDto departmentDto);

    Task<bool> ChangePersonDepartmentAsync(int personId, string newDepartmentName);

    Task<int> GetTotalDepartmentsAsync();
}