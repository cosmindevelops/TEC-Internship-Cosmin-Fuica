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

public class PersonService : IPersonService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _apiUrl;

    public PersonService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _apiUrl = configuration["ApiSettings:ApiUrl"];
    }

    public async Task<IEnumerable<WebApp.Models.PersonDto>> GetAllPersonsAsync()
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl}/person");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var apiPersons = await response.Content.ReadFromJsonAsync<IEnumerable<PersonDto>>();

            var webPersons = apiPersons.Select(p => new WebApp.Models.PersonDto
            {
                Id = p.Id,
                Name = p.Name,
                Surname = p.Surname,
                Age = p.Age,
                Email = p.Email,
                Address = p.Address,
                PositionName = p.Position?.Name,
                DepartmentName = p.Position?.Department?.DepartmentName,
                SalaryAmount = p.Salary?.Amount ?? 0,
                BirthDay = p.PersonDetails?.BirthDay,
                PersonCity = p.PersonDetails?.PersonCity
            });

            return webPersons;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching persons: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> CreatePersonAsync(CreateUpdatePersonDto personDto)
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiUrl}/person")
            {
                Content = JsonContent.Create(personDto)
            };
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating person: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UpdatePersonAsync(int personId, CreateUpdatePersonDto personDto)
    {
        var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
        var request = new HttpRequestMessage(HttpMethod.Put, $"{_apiUrl}/person/{personId}")
        {
            Content = JsonContent.Create(personDto)
        };
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePersonAsync(int personId)
    {
        var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{_apiUrl}/person/{personId}");
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
}