using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Services.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<WebApp.Models.DepartmentDto>> GetAllDepartmentsAsync();

    Task<bool> UpdateDepartmentAsync(int departmentId, string newDepartmentName);

    Task<bool> CreateDepartmentAsync(string departmentName);

    Task<bool> DeleteDepartmentAsync(int departmentId);
}