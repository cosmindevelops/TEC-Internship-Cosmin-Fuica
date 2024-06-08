using ApiApp.Common.Dto;
using ApiApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User,Admin")]
public class PersonController : BaseController
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPersons()
    {
        var persons = await _personService.GetAllPersonsAsync();
        return Ok(persons);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerson(int id)
    {
        var person = await _personService.GetPersonAsync(id);
        if (person == null)
        {
            return NotFound();
        }

        return Ok(person);
    }

    [HttpGet("total")]
    public async Task<IActionResult> GetTotalPersons()
    {
        var totalPersons = await _personService.GetTotalPersonsAsync();
        return Ok(new { TotalPersons = totalPersons });
    }

    [HttpPost]
    public async Task<IActionResult> CreatePerson([FromBody] CreateUpdatePersonDto personDto)
    {
        var validationResult = ValidateModel(personDto);
        if (validationResult != null)
        {
            return validationResult;
        }

        var createdPerson = await _personService.CreatePersonAsync(personDto);
        return CreatedAtAction(nameof(GetPerson), new { id = createdPerson.Id }, createdPerson);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePerson(int id, [FromBody] CreateUpdatePersonDto personDto)
    {
        var validationResult = ValidateModel(personDto);
        if (validationResult != null)
        {
            return validationResult;
        }

        var updated = await _personService.UpdatePersonAsync(id, personDto);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        var deleted = await _personService.DeletePersonAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}