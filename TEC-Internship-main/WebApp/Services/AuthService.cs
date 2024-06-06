using ApiApp.Common.Dto;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApp.Services.Interfaces;

namespace WebApp.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;

    public AuthService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiUrl = configuration["ApiSettings:ApiUrl"];
    }

    public async Task<AuthResponseDto> LoginAsync(LoginModelDto model)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/auth/login", model);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
    }

    public async Task RegisterAsync(RegisterModelDto model)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/auth/register", model);
        response.EnsureSuccessStatusCode();
    }
}