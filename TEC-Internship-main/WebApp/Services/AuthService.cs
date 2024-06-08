using ApiApp.Common.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApp.Services.Interfaces;

namespace WebApp.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _apiUrl;

    public AuthService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _apiUrl = configuration["ApiSettings:ApiUrl"];
    }

    public async Task<AuthResponseDto> LoginAsync(LoginModelDto model)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/auth/login", model);
        response.EnsureSuccessStatusCode();
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

        // Store token and username in session
        _httpContextAccessor.HttpContext.Session.SetString("Token", authResponse.Token);
        _httpContextAccessor.HttpContext.Session.SetString("Username", authResponse.Username);

        return authResponse;
    }

    public async Task RegisterAsync(RegisterModelDto model)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/auth/register", model);
        response.EnsureSuccessStatusCode();
    }

    public string GetToken()
    {
        return _httpContextAccessor.HttpContext.Session.GetString("Token");
    }
}