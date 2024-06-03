using ApiApp.Common.Dto;

namespace ApiApp.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginModelDto model);

    Task RegisterAsync(RegisterModelDto model);
}