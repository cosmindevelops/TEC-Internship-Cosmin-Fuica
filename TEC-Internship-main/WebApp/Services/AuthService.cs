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

    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="model">The login model containing user credentials.</param>
    /// <returns>The authentication response containing the token and username.</returns>
    /// <exception cref="HttpRequestException">Thrown when an HTTP request error occurs.</exception>
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

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="model">The registration model containing user details.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="HttpRequestException">Thrown when an HTTP request error occurs.</exception>
    public async Task RegisterAsync(RegisterModelDto model)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_apiUrl}/auth/register", model);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Gets the current authentication token from the session.
    /// </summary>
    /// <returns>The authentication token as a string.</returns>
    public string GetToken()
    {
        return _httpContextAccessor.HttpContext.Session.GetString("Token");
    }
}