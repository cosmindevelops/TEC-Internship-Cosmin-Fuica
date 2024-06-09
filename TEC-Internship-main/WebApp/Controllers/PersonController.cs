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

    /// <summary>
    /// Displays the person management view.
    /// </summary>
    /// <returns>The person management view with a list of persons.</returns>
    public async Task<IActionResult> Index()
    {
        var persons = await _personService.GetAllPersonsAsync();
        return View(persons);
    }

    /// <summary>
    /// Displays the person creation view.
    /// </summary>
    /// <returns>The person creation view with a list of departments.</returns>
    public async Task<IActionResult> Create()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        ViewData["Departments"] = departments;
        return View("PersonDepartmentManagement");
    }

    /// <summary>
    /// Creates a new person.
    /// </summary>
    /// <param name="personDto">The DTO containing person data.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the creation operation.</returns>
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

    /// <summary>
    /// Updates an existing person.
    /// </summary>
    /// <param name="id">The ID of the person to update.</param>
    /// <param name="personDto">The DTO containing updated person data.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
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

    /// <summary>
    /// Deletes a person.
    /// </summary>
    /// <param name="id">The ID of the person to delete.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the deletion operation.</returns>
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