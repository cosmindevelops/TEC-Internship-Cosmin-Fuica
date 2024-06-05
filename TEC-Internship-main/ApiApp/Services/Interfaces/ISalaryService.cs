using ApiApp.Common.Dto;

namespace ApiApp.Services.Interfaces;

public interface ISalaryService
{
    Task<IEnumerable<SalaryWithFullNameDto>> GetAllSalariesAsync();

    Task<SalaryDto> GetSalaryByPersonIdAsync(int personId);

    Task<bool> DeleteSalaryAsync(int personId);

    Task<bool> UpdateSalaryAsync(int personId, int newSalaryAmount);
}