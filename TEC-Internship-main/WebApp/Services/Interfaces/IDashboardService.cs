using System.Threading.Tasks;

namespace WebApp.Services.Interfaces;

public interface IDashboardService
{
    Task<int> GetTotalPersonsAsync();

    Task<int> GetTotalDepartmentsAsync();
}