using ApiApp.Common.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Services.Interfaces;

namespace WebApp.Services;

public class PersonService : IPersonService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public PersonService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiBaseUrl = config.GetValue<string>("ApiSettings:ApiUrl");
    }

    public async Task<List<PersonDto>> GetPersonsAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{_apiBaseUrl}/person");
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<PersonDto>>(jsonResponse);
    }
}