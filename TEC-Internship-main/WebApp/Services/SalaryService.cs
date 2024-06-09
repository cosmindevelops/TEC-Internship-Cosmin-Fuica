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

    /// <summary>
    /// Retrieves all salaries from the API.
    /// </summary>
    /// <returns>A list of <see cref="WebApp.Models.SalaryWithFullNameDto"/>.</returns>
    /// <exception cref="HttpRequestException">Thrown when an HTTP request error occurs.</exception>
    public async Task<IEnumerable<WebApp.Models.SalaryWithFullNameDto>> GetAllSalariesAsync()
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl}/salary");

            if (!string.IsNullOrEmpty(token)) request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var apiSalaries = await response.Content.ReadFromJsonAsync<IEnumerable<SalaryWithFullNameDto>>();

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
            throw new HttpRequestException("Error fetching salaries", ex);
        }
    }

    /// <summary>
    /// Updates the salary of a person.
    /// </summary>
    /// <param name="personId">The ID of the person to update the salary for.</param>
    /// <param name="newAmount">The new salary amount.</param>
    /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
    /// <exception cref="HttpRequestException">Thrown when an HTTP request error occurs.</exception>
    public async Task<bool> UpdateSalaryAsync(int personId, int newAmount)
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{_apiUrl}/salary/UpdateSalary?personId={personId}&newSalaryAmount={newAmount}");

            if (!string.IsNullOrEmpty(token)) request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Error updating salary", ex);
        }
    }
}