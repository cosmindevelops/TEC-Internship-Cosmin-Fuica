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

public class DepartmentService : IDepartmentService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _apiUrl;

    public DepartmentService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _apiUrl = configuration["ApiSettings:ApiUrl"];
    }

    public async Task<IEnumerable<WebApp.Models.DepartmentDto>> GetAllDepartmentsAsync()
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl}/department");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var apiDepartments = await response.Content.ReadFromJsonAsync<IEnumerable<DepartmentDto>>();

            var webDepartments = apiDepartments.Select(d => new WebApp.Models.DepartmentDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName
            });

            return webDepartments;
        }
        catch (Exception ex)
        {
            // Log error
            Console.WriteLine($"Error fetching departments: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> CreateDepartmentAsync(string departmentName)
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var createData = new { DepartmentName = departmentName };
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiUrl}/department")
            {
                Content = JsonContent.Create(createData)
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
            // Log error
            Console.WriteLine($"Error creating department: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UpdateDepartmentAsync(int departmentId, string newDepartmentName)
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var updateData = new { DepartmentName = newDepartmentName };
            var request = new HttpRequestMessage(HttpMethod.Put, $"{_apiUrl}/department/{departmentId}")
            {
                Content = JsonContent.Create(updateData)
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
            // Log error
            Console.WriteLine($"Error updating department: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteDepartmentAsync(int departmentId)
    {
        try
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_apiUrl}/department/{departmentId}");
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
            Console.WriteLine($"Error deleting department: {ex.Message}");
            throw;
        }
    }
}