using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApp.Services.Interfaces;

namespace WebApp.Services;

public class DashboardService : IDashboardService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _apiUrl;

    public DashboardService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _apiUrl = configuration["ApiSettings:ApiUrl"];
    }

    /// <summary>
    /// Retrieves the total number of persons.
    /// </summary>
    /// <returns>The total number of persons as an integer.</returns>
    /// <exception cref="HttpRequestException">Thrown when an HTTP request error occurs.</exception>
    public async Task<int> GetTotalPersonsAsync()
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl}/person/total");

            if (!string.IsNullOrEmpty(token)) request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TotalPersonsResponse>();
            return result.TotalPersons;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Error fetching total persons", ex);
        }
    }

    /// <summary>
    /// Retrieves the total number of departments.
    /// </summary>
    /// <returns>The total number of departments as an integer.</returns>
    /// <exception cref="HttpRequestException">Thrown when an HTTP request error occurs.</exception>
    public async Task<int> GetTotalDepartmentsAsync()
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl}/department/total");

            if (!string.IsNullOrEmpty(token))  request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TotalDepartmentsResponse>();
            return result.TotalDepartments;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Error fetching total departments", ex);
        }
    }

    private class TotalPersonsResponse
    {
        public int TotalPersons { get; set; }
    }

    private class TotalDepartmentsResponse
    {
        public int TotalDepartments { get; set; }
    }
}