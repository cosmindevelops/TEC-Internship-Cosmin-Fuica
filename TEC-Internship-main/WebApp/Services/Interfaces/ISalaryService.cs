using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Services.Interfaces;

public interface ISalaryService
{
    Task<IEnumerable<WebApp.Models.SalaryWithFullNameDto>> GetAllSalariesAsync();

    Task<bool> UpdateSalaryAsync(int personId, int newAmount);
}