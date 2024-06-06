using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public DashboardService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiSettings:ApiUrl"];
        }

        public async Task<int> GetTotalPersonsAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/person/total");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TotalPersonsResponse>();
            return result.TotalPersons;
        }

        public async Task<int> GetTotalDepartmentsAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/department/total");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TotalDepartmentsResponse>();
            return result.TotalDepartments;
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
}