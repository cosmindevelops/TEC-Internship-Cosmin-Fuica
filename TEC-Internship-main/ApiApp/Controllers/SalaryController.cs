using ApiApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalaryController : BaseController
{
    private readonly ISalaryService _salaryService;

    public SalaryController(ISalaryService salaryService)
    {
        _salaryService = salaryService ?? throw new ArgumentNullException(nameof(salaryService));
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllSalaries()
    {
        var salaries = await _salaryService.GetAllSalariesAsync();
        return Ok(salaries);
    }

    [Authorize(Roles = "User,Admin")]
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

    [Authorize(Roles = "User,Admin")]
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

    [Authorize(Roles = "User,Admin")]
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