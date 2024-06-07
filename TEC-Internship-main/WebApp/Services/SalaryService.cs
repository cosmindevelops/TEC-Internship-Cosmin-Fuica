using ApiApp.Common.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApp.Services.Interfaces;

namespace WebApp.Services;

public class SalaryService : ISalaryService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _apiUrl;

    public SalaryService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _apiUrl = configuration["ApiSettings:ApiUrl"];
    }

    public async Task<IEnumerable<WebApp.Models.SalaryWithFullNameDto>> GetAllSalariesAsync()
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl}/salary");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var apiSalaries = await response.Content.ReadFromJsonAsync<IEnumerable<SalaryWithFullNameDto>>();

            // Manually map ApiApp.Common.Dto.SalaryWithFullNameDto to WebApp.Models.SalaryWithFullNameDto
            var webSalaries = apiSalaries.Select(s => new WebApp.Models.SalaryWithFullNameDto
            {
                PersonId = s.PersonId,
                FullName = s.FullName,
                Amount = s.Amount
            });

            return webSalaries;
        }
        catch (Exception ex)
        {
            // Log error
            Console.WriteLine($"Error fetching salaries: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UpdateSalaryAsync(int personId, int newAmount)
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{_apiUrl}/salary/UpdateSalary?personId={personId}&newSalaryAmount={newAmount}");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            // Log error
            Console.WriteLine($"Error updating salary: {ex.Message}");
            throw;
        }
    }
}