using ApiApp.Common.Dto;
using System.Threading.Tasks;

namespace WebApp.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginModelDto model);

    Task RegisterAsync(RegisterModelDto model);
}