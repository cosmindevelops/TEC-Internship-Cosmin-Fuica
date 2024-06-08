using ApiApp.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

public class PersonController : Controller
{
    private readonly IPersonService _personService;
    private readonly IDepartmentService _departmentService;

    public PersonController(IPersonService personService, IDepartmentService departmentService)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
    }

    public async Task<IActionResult> Index()
    {
        var persons = await _personService.GetAllPersonsAsync();
        return View(persons);
    }

    public async Task<IActionResult> Create()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        ViewData["Departments"] = departments;
        return View("PersonDepartmentManagement");
    }

    [HttpPost]
    public async Task<IActionResult> CreatePerson([FromBody] CreateUpdatePersonDto personDto)
    {
        var success = await _personService.CreatePersonAsync(personDto);
        if (!success)
        {
            return BadRequest("Failed to create person");
        }
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePerson(int id, [FromBody] CreateUpdatePersonDto personDto)
    {
        var success = await _personService.UpdatePersonAsync(id, personDto);
        if (!success)
        {
            return BadRequest("Failed to update person");
        }
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> DeletePerson(int id)
    {
        var success = await _personService.DeletePersonAsync(id);
        if (!success)
        {
            return BadRequest("Failed to delete person");
        }
        return NoContent();
    }
}