using ApiApp.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

public class PersonController : Controller
{
    private readonly IConfiguration _config;
    private readonly string _apiBaseUrl;
    private readonly IPersonService _personService;

    public PersonController(IConfiguration config, IPersonService personService)
    {
        _config = config;
        _apiBaseUrl = _config.GetValue<string>("ApiSettings:ApiUrl");
        _personService = personService;
    }

    public async Task<IActionResult> Index()
    {
        var persons = await _personService.GetPersonsAsync();
        var personInfos = persons.Select(dto => new PersonInformation
        {
            Id = dto.Id,
            Name = dto.Name,
            Surname = dto.Surname,
            PositionName = dto.Position?.Name,
            DepartmentName = dto.Position?.Department?.DepartmentName,
            Salary = dto.Salary?.Amount ?? 0
        }).ToList();

        return View(personInfos);
    }

    public IActionResult Add()
    {
        return View(new PersonViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Add(PersonViewModel personViewModel)
    {
        if (ModelState.IsValid)
        {
            var createUpdatePersonDto = new CreateUpdatePersonDto
            {
                Name = personViewModel.Name,
                Surname = personViewModel.Surname,
                Age = personViewModel.Age,
                Email = personViewModel.Email,
                Address = personViewModel.Address,
                PersonDetails = new CreateUpdatePersonDetailsDto
                {
                    PersonId = personViewModel.PersonDetails.PersonId,
                    BirthDay = personViewModel.PersonDetails.BirthDay,
                    PersonCity = personViewModel.PersonDetails.PersonCity
                },
                Position = new CreateUpdatePositionDto { Name = personViewModel.Position.Name },
                Salary = new CreateUpdateSalaryDto { Amount = personViewModel.Salary.Amount }
            };

            HttpClient client = new HttpClient();
            var jsonPerson = JsonConvert.SerializeObject(createUpdatePersonDto);
            StringContent content = new StringContent(jsonPerson, Encoding.UTF8, "application/json");
            HttpResponseMessage message = await client.PostAsync($"{_apiBaseUrl}/person", content);
            if (message.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "There is an API Error");
                return View(personViewModel);
            }
        }
        else
        {
            return View(personViewModel);
        }
    }

    public async Task<IActionResult> Update(int id)
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage message = await client.GetAsync($"{_apiBaseUrl}/person/{id}");
        if (message.IsSuccessStatusCode)
        {
            var jstring = await message.Content.ReadAsStringAsync();
            var personDto = JsonConvert.DeserializeObject<PersonDto>(jstring);

            var personViewModel = new PersonViewModel
            {
                Id = personDto.Id,
                Name = personDto.Name,
                Surname = personDto.Surname,
                Age = personDto.Age,
                Email = personDto.Email,
                Address = personDto.Address,
                PositionId = personDto.PositionId,
                SalaryId = personDto.SalaryId,
                Position = personDto.Position,
                Salary = personDto.Salary,
                PersonDetails = personDto.PersonDetails
            };

            return View(personViewModel);
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, PersonViewModel personViewModel)
    {
        if (ModelState.IsValid)
        {
            var createUpdatePersonDto = new CreateUpdatePersonDto
            {
                Name = personViewModel.Name,
                Surname = personViewModel.Surname,
                Age = personViewModel.Age,
                Email = personViewModel.Email,
                Address = personViewModel.Address,
                PersonDetails = new CreateUpdatePersonDetailsDto
                {
                    PersonId = personViewModel.PersonDetails.PersonId,
                    BirthDay = personViewModel.PersonDetails.BirthDay,
                    PersonCity = personViewModel.PersonDetails.PersonCity
                },
                Position = new CreateUpdatePositionDto { Name = personViewModel.Position.Name },
                Salary = new CreateUpdateSalaryDto { Amount = personViewModel.Salary.Amount }
            };

            HttpClient client = new HttpClient();
            var jsonPerson = JsonConvert.SerializeObject(createUpdatePersonDto);
            StringContent content = new StringContent(jsonPerson, Encoding.UTF8, "application/json");
            HttpResponseMessage message = await client.PutAsync($"{_apiBaseUrl}/person/{id}", content);
            if (message.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "There is an API Error");
                return View(personViewModel);
            }
        }
        else
        {
            return View(personViewModel);
        }
    }
}