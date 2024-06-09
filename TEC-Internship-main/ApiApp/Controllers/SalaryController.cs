using ApiApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User,Admin")]
public class SalaryController : BaseController
{
    private readonly ISalaryService _salaryService;

    public SalaryController(ISalaryService salaryService)
    {
        _salaryService = salaryService ?? throw new ArgumentNullException(nameof(salaryService));
    }

    /// <summary>
    /// Gets all salaries.
    /// </summary>
    /// <returns>A list of all salaries.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllSalaries()
    {
        var salaries = await _salaryService.GetAllSalariesAsync();
        return Ok(salaries);
    }

    /// <summary>
    /// Gets the salary of a person by their ID.
    /// </summary>
    /// <param name="personId">The ID of the person to retrieve the salary for.</param>
    /// <returns>The salary of the specified person.</returns>
    [HttpGet("{personId}")]
    public async Task<IActionResult> GetSalaryByPersonId(int personId)
    {
        var salary = await _salaryService.GetSalaryByPersonIdAsync(personId);
        if (salary == null)
        {
            return NotFound();
        }

        return Ok(salary);
    }

    /// <summary>
    /// Deletes the salary of a person.
    /// </summary>
    /// <param name="personId">The ID of the person to delete the salary for.</param>
    /// <returns>No content if the deletion was successful.</returns>
    [HttpPut("{personId}")]
    public async Task<IActionResult> DeleteSalary(int personId)
    {
        var deleted = await _salaryService.DeleteSalaryAsync(personId);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Updates the salary of a person.
    /// </summary>
    /// <param name="personId">The ID of the person to update the salary for.</param>
    /// <param name="newSalaryAmount">The new salary amount.</param>
    /// <returns>No content if the update was successful.</returns>
    [HttpPut("UpdateSalary")]
    public async Task<IActionResult> UpdateSalary(int personId, int newSalaryAmount)
    {
        var updated = await _salaryService.UpdateSalaryAsync(personId, newSalaryAmount);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}