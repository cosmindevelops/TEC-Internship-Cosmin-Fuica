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

    /// <summary>
    /// Gets all persons.
    /// </summary>
    /// <returns>A list of all persons.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllPersons()
    {
        var persons = await _personService.GetAllPersonsAsync();
        return Ok(persons);
    }

    /// <summary>
    /// Gets a person by their ID.
    /// </summary>
    /// <param name="id">The ID of the person to retrieve.</param>
    /// <returns>The person with the specified ID.</returns>
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

    /// <summary>
    /// Gets the total number of persons.
    /// </summary>
    /// <returns>The total number of persons.</returns>
    [HttpGet("total")]
    public async Task<IActionResult> GetTotalPersons()
    {
        var totalPersons = await _personService.GetTotalPersonsAsync();
        return Ok(new { TotalPersons = totalPersons });
    }

    /// <summary>
    /// Creates a new person.
    /// </summary>
    /// <param name="personDto">The DTO containing person data.</param>
    /// <returns>The created person.</returns>
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

    /// <summary>
    /// Updates a person's information.
    /// </summary>
    /// <param name="id">The ID of the person to update.</param>
    /// <param name="personDto">The DTO containing updated person data.</param>
    /// <returns>No content if the update was successful.</returns>
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

    /// <summary>
    /// Deletes a person.
    /// </summary>
    /// <param name="id">The ID of the person to delete.</param>
    /// <returns>No content if the deletion was successful.</returns>
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